using Microsoft.EntityFrameworkCore;
using MyTeamsHub.IdentityServer.API.DAL;
using MyTeamsHub.IdentityServer.API.Dtos;

namespace MyTeamsHub.IdentityServer.API.Services;

public class UserService : IUserService
{
    private readonly ICryptoService _cryptoService;
    private readonly IdentityDbContext _dbContext;

    public UserService(
        ICryptoService cryptoService
        , IdentityDbContext dbContext)
    {
        _cryptoService = cryptoService;
        _dbContext = dbContext;
    }

    public async Task<Guid> CreateAsync(UserSignupRequestDto request, CancellationToken cancellationToken)
    {
        //if (string.IsNullOrEmpty(request.FirstName))
        //    return ServiceDataResult<Guid>.WithError(ErrorCodes.InvalidFirstName);

        //if (string.IsNullOrEmpty(request.LastName))
        //    return ServiceDataResult<Guid>.WithError(ErrorCodes.InvalidLastName);

        //if (string.IsNullOrEmpty(request.Email))
        //    return ServiceDataResult<Guid>.WithError(ErrorCodes.InvalidEmail);

        //if (string.IsNullOrEmpty(request.PhoneNumber))
        //    return ServiceDataResult<Guid>.WithError(ErrorCodes.InvalidPhoneNumber);

        //if (string.IsNullOrEmpty(request.Password))
        //    return ServiceDataResult<Guid>.WithError(ErrorCodes.InvalidPassword);

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
            UserStatus = UserStatus.Active,
            UserType = UserType.ClientAdmin
        };

        await _dbContext.Users.AddAsync(newUser, cancellationToken);


        return newUser.UserId;
    }

    public async Task<User> TryGetAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Email == email, cancellationToken);
        //if (user == null)
        //    return ServiceDataResult<User>.WithError(ErrorCodes.InvalidUsernameOrPassword);

        //var isValidPassword = _cryptoService.VerifyPassword(password, user.PasswordHashBase64, user.PasswordSaltBase64);
        //if (!isValidPassword)
        //    return ServiceDataResult<User>.WithError(ErrorCodes.InvalidUsernameOrPassword);

        return new User
        {
            Email = email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            UserId = user.UserId
        };
    }
}
