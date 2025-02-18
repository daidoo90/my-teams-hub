using MyTeamsHub.Core.Application.Common;
using MyTeamsHub.Core.Application.Interfaces.Repositories;
using MyTeamsHub.Core.Domain.Users;

namespace MyTeamsHub.Core.Application.User.Update;

public sealed record UpdateUserCommand(Guid UserId, string FirstName, string LastName, string PhoneNumber, int Status, string Title) : ICommand;

public class UpdateUserCommandHandler(IEfRepository<UserEntity> users) : ICommandHandler<UpdateUserCommand>
{
    private readonly IEfRepository<UserEntity> _users = users;

    public async Task<ServiceResult> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _users.FirstOrDefaultAsync(u => u.UserId == request.UserId, cancellationToken);
        if (user == null)
            return ServiceResult.WithError(ErrorCodes.InvalidUser);

        if (string.IsNullOrEmpty(request.FirstName))
        {
            return ServiceResult.WithError(ErrorCodes.InvalidFirstName);
        }

        if (string.IsNullOrEmpty(request.LastName))
        {
            return ServiceResult.WithError(ErrorCodes.InvalidLastName);
        }

        if (string.IsNullOrEmpty(request.PhoneNumber))
        {
            return ServiceResult.WithError(ErrorCodes.InvalidPhoneNumber);
        }

        if (request.Status != (int)UserStatus.Inactive &&
            request.Status != (int)UserStatus.Active)
        {
            return ServiceResult.WithError(ErrorCodes.UserStatusInvalid);
        }

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.PhoneNumber = request.PhoneNumber;
        user.UserStatus = (UserStatus)request.Status;
        user.LastUpdatedOn = DateTime.UtcNow;
        user.RoleTitle = request.Title;

        await _users.UpdateAsync(user, cancellationToken);

        return ServiceResult.Success;
    }
}
