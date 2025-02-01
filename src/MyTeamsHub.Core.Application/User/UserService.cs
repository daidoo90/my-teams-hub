//using Microsoft.EntityFrameworkCore;
//using MyTeamsHub.Core.Domain.Users;
//using MyTeamsHub.Domain.Services.Common;
//using MyTeamsHub.Domain.Services.Organizations;
//using MyTeamsHub.Persistence.Core.Repository;
//using MyTeamsHub.Persistence.Models.Types;
//using MyTeamsHub.Persistence.Models.Users;

//namespace MyTeamsHub.Domain.Services.Users;

//public interface IUserService
//{
//    Task<ServiceDataResult<User>> TryGetAsync(string email, string password, CancellationToken cancellationToken = default);

//    Task<ServiceDataResult<Guid>> CreateAsync(NewUser user, CancellationToken cancellationToken = default);

//    Task<ServiceDataResult<User>> GetAsync(Guid userId, CancellationToken ccancellationTokent = default);

//    Task<ServiceDataResult<User>> GetAsync(string email, CancellationToken cancellationToken = default);

//    Task<ServiceDataResult<IEnumerable<ClientUser>>> GetAllAsync(IEnumerable<Guid> organizationIds, CancellationToken cancellationToken = default);

//    Task<ServiceResult> UpdateAsync(Guid userId, string firstName, string lastName, string phoneNumber, int status, string title, CancellationToken cancellationToken = default);
//}

//public class UserService : IUserService
//{
//    private readonly IEfRepository<UserEntity> _users;
//    private readonly ICryptoService _cryptoService;
//    private readonly IOrganizationService _organizationService;

//    public UserService(IEfRepository<UserEntity> users,
//        ICryptoService cryptoService,
//        IOrganizationService organizationService)
//    {
//        _users = users;
//        _cryptoService = cryptoService;
//        _organizationService = organizationService;
//    }

//    public async Task<ServiceDataResult<User>> GetAsync(string email, CancellationToken cancellationToken = default)
//    {
        
//    }

//    public async Task<ServiceDataResult<User>> TryGetAsync(string email, string password, CancellationToken cancellationToken = default)
//    {
//        var user = await _users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
//        if (user == null)
//            return ServiceDataResult<User>.WithError(ErrorCodes.InvalidUsernameOrPassword);

//        var isValidPassword = _cryptoService.VerifyPassword(password, user.PasswordHashBase64, user.PasswordSaltBase64);
//        if (!isValidPassword)
//            return ServiceDataResult<User>.WithError(ErrorCodes.InvalidUsernameOrPassword);

//        return ServiceDataResult<User>.WithData(new User
//        {
//            Email = email,
//            FirstName = user.FirstName,
//            LastName = user.LastName,
//            UserId = user.UserId
//        });
//    }
//}
