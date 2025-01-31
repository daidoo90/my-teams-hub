using Microsoft.EntityFrameworkCore;
using MyTeamsHub.Domain.Entities.Users;
using MyTeamsHub.Domain.Services.Common;
using MyTeamsHub.Domain.Services.Organizations;
using MyTeamsHub.Persistence.Core.Repository;
using MyTeamsHub.Persistence.Models.Types;
using MyTeamsHub.Persistence.Models.Users;

namespace MyTeamsHub.Domain.Services.Users;

public interface IUserService
{
    Task<ServiceDataResult<User>> TryGetAsync(string email, string password, CancellationToken cancellationToken = default);

    Task<ServiceDataResult<Guid>> CreateAsync(NewUser user, CancellationToken cancellationToken = default);

    Task<ServiceDataResult<User>> GetAsync(Guid userId, CancellationToken ccancellationTokent = default);

    Task<ServiceDataResult<User>> GetAsync(string email, CancellationToken cancellationToken = default);

    Task<ServiceDataResult<IEnumerable<ClientUser>>> GetAllAsync(IEnumerable<Guid> organizationIds, CancellationToken cancellationToken = default);

    Task<ServiceResult> UpdateAsync(Guid userId, string firstName, string lastName, string phoneNumber, int status, string title, CancellationToken cancellationToken = default);
}

public class UserService : IUserService
{
    private readonly IEfRepository<UserEntity> _users;
    private readonly ICryptoService _cryptoService;
    private readonly IOrganizationService _organizationService;

    public UserService(IEfRepository<UserEntity> users,
        ICryptoService cryptoService,
        IOrganizationService organizationService)
    {
        _users = users;
        _cryptoService = cryptoService;
        _organizationService = organizationService;
    }

    public async Task<ServiceDataResult<Guid>> CreateAsync(NewUser user, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(user.FirstName))
            return ServiceDataResult<Guid>.WithError(ErrorCodes.InvalidFirstName);

        if (string.IsNullOrEmpty(user.LastName))
            return ServiceDataResult<Guid>.WithError(ErrorCodes.InvalidLastName);

        if (string.IsNullOrEmpty(user.Email))
            return ServiceDataResult<Guid>.WithError(ErrorCodes.InvalidEmail);

        if (string.IsNullOrEmpty(user.PhoneNumber))
            return ServiceDataResult<Guid>.WithError(ErrorCodes.InvalidPhoneNumber);

        if (string.IsNullOrEmpty(user.Password))
            return ServiceDataResult<Guid>.WithError(ErrorCodes.InvalidPassword);

        var hashedPassword = _cryptoService.HashPassword(user.Password, out var salt);

        var newUser = new UserEntity
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            PasswordHashBase64 = hashedPassword,
            PasswordSaltBase64 = Convert.ToBase64String(salt),
            PasswordExpiresOn = DateTime.UtcNow.AddDays(3),
            CreatedOn = DateTime.UtcNow,
            UserStatus = (UserStatus)user.UserStatus,
            UserType = (UserType)user.UserType
        };
        await _users.AddAsync(newUser, cancellationToken);


        return ServiceDataResult<Guid>.Created(newUser.UserId);
    }

    public async Task<ServiceDataResult<IEnumerable<ClientUser>>> GetAllAsync(IEnumerable<Guid> organizationIds, CancellationToken cancellationToken = default)
    {
        var users = await _users
            .Query()
            .Include(u => u.TeamMembers)
            .ThenInclude(tm => tm.Team)
            .Where(u => u.TeamMembers.First().Team.OrganizationId == organizationIds.First())
            .Select(u => new ClientUser
            {
                UserId = u.UserId,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                Title = u.RoleTitle,
                Status = (int)u.UserStatus,
                UserType = (int)u.UserType,
                CreationDate = u.CreatedOn,
                Teams = u.TeamMembers
                         .Where(tm => !tm.Team.IsDeleted)
                         .Select(tm => new Team
                         {
                             TeamId = tm.TeamId,
                             Name = tm.Team.Name
                         })
            })
            .ToListAsync(cancellationToken);

        return ServiceDataResult<IEnumerable<ClientUser>>.WithData(users);
    }

    public async Task<ServiceDataResult<User>> GetAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await _users.FirstOrDefaultAsync(u => u.UserId == userId, cancellationToken);
        if (user == null)
            return ServiceDataResult<User>.WithError(ErrorCodes.InvalidUser);

        return ServiceDataResult<User>.WithData(new User
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            UserId = userId,
            PhoneNumber = user.PhoneNumber,
            Title = user.RoleTitle,
            Status = (int)user.UserStatus,
            UserType = (int)user.UserType
        });
    }

    public async Task<ServiceDataResult<User>> GetAsync(string email, CancellationToken cancellationToken = default)
    {
        var user = await _users.FirstOrDefaultAsync(u => u.Email == email);

        var userResult = user == null ? null : new User
        {
            UserId = user.UserId,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email
        };
        return ServiceDataResult<User>.WithData(userResult);
    }

    public async Task<ServiceDataResult<User>> TryGetAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        var user = await _users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        if (user == null)
            return ServiceDataResult<User>.WithError(ErrorCodes.InvalidUsernameOrPassword);

        var isValidPassword = _cryptoService.VerifyPassword(password, user.PasswordHashBase64, user.PasswordSaltBase64);
        if (!isValidPassword)
            return ServiceDataResult<User>.WithError(ErrorCodes.InvalidUsernameOrPassword);

        return ServiceDataResult<User>.WithData(new User
        {
            Email = email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            UserId = user.UserId
        });
    }

    public async Task<ServiceResult> UpdateAsync(Guid userId, string firstName, string lastName, string phoneNumber, int status, string title, CancellationToken cancellationToken = default)
    {
        var user = await _users.FirstOrDefaultAsync(u => u.UserId == userId, cancellationToken);
        if (user == null)
            return ServiceResult.WithError(ErrorCodes.InvalidUser);

        if (string.IsNullOrEmpty(firstName))
        {
            return ServiceResult.WithError(ErrorCodes.InvalidFirstName);
        }

        if (string.IsNullOrEmpty(lastName))
        {
            return ServiceResult.WithError(ErrorCodes.InvalidLastName);
        }

        if (string.IsNullOrEmpty(phoneNumber))
        {
            return ServiceResult.WithError(ErrorCodes.InvalidPhoneNumber);
        }

        if (status != (int)UserStatus.Inactive &&
            status != (int)UserStatus.Active)
        {
            return ServiceResult.WithError(ErrorCodes.UserStatusInvalid);
        }

        user.FirstName = firstName;
        user.LastName = lastName;
        user.PhoneNumber = phoneNumber;
        user.UserStatus = (UserStatus)status;
        user.LastUpdatedOn = DateTime.UtcNow;
        user.RoleTitle = title;

        await _users.UpdateAsync(user, cancellationToken);

        return ServiceResult.Success;
    }
}
