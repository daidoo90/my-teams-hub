using MyTeamsHub.Domain.Services.Common;
using MyTeamsHub.Persistence.Core.Repository;
using MyTeamsHub.Persistence.Models.Users;

namespace MyTeamsHub.Domain.Services.User.GetById;

public sealed record GetUserByIdQuery(Guid UserId) : IQuery<Entities.Users.User>;

public class GetUserByIdQueryHandler(IEfRepository<UserEntity> users) : IQueryHandler<GetUserByIdQuery, Entities.Users.User>
{
    private readonly IEfRepository<UserEntity> _users = users;

    public async Task<ServiceDataResult<Entities.Users.User>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _users.FirstOrDefaultAsync(u => u.UserId == request.UserId, cancellationToken);
        if (user == null)
            return ServiceDataResult<Entities.Users.User>.WithError(ErrorCodes.InvalidUser);

        return ServiceDataResult<Entities.Users.User>.WithData(new Entities.Users.User
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            UserId = request.UserId,
            PhoneNumber = user.PhoneNumber,
            Title = user.RoleTitle,
            Status = (int)user.UserStatus,
            UserType = (int)user.UserType
        });
    }
}
