using MyTeamsHub.Core.Application.Common;
using MyTeamsHub.Core.Application.Interfaces.Repositories;
using MyTeamsHub.Core.Application.Interfaces.Shared;
using MyTeamsHub.Core.Domain.Users;

namespace MyTeamsHub.Core.Application.User.Create;

public sealed record CreateUserCommand(string FirstName, string LastName, string Email, string PhoneNumber, string Password, int UserStatus, int UserType) : ICommand<Guid>;

public class CreateUserCommandHandler(
    ICryptoService cryptoService,
    IEfRepository<UserEntity> users) : ICommandHandler<CreateUserCommand, Guid>
{
    private readonly ICryptoService _cryptoService = cryptoService;
    private readonly IEfRepository<UserEntity> _users = users;

    public async Task<ServiceDataResult<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.FirstName))
            return ServiceDataResult<Guid>.WithError(ErrorCodes.InvalidFirstName);

        if (string.IsNullOrEmpty(request.LastName))
            return ServiceDataResult<Guid>.WithError(ErrorCodes.InvalidLastName);

        if (string.IsNullOrEmpty(request.Email))
            return ServiceDataResult<Guid>.WithError(ErrorCodes.InvalidEmail);

        if (string.IsNullOrEmpty(request.PhoneNumber))
            return ServiceDataResult<Guid>.WithError(ErrorCodes.InvalidPhoneNumber);

        if (string.IsNullOrEmpty(request.Password))
            return ServiceDataResult<Guid>.WithError(ErrorCodes.InvalidPassword);

        var hashedPassword = _cryptoService.HashPassword(request.Password, out var salt);

        var newUser = new UserEntity
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            PasswordHashBase64 = hashedPassword,
            PasswordSaltBase64 = Convert.ToBase64String(salt),
            PasswordExpiresOn = DateTime.UtcNow.AddDays(3),
            CreatedOn = DateTime.UtcNow,
            UserStatus = (UserStatus)request.UserStatus,
            UserType = (UserType)request.UserType
        };
        await _users.AddAsync(newUser, cancellationToken);


        return ServiceDataResult<Guid>.Created(newUser.UserId);
    }
}
