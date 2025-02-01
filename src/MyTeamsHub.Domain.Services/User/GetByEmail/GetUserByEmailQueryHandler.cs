using MyTeamsHub.Domain.Services.Common;
using MyTeamsHub.Persistence.Core.Repository;
using MyTeamsHub.Persistence.Models.Users;

namespace MyTeamsHub.Domain.Services.User.GetByEmail;

public sealed record GetUserByEmailQuery(string Email): IQuery<Entities.Users.User>;

public class GetUserByEmailQueryHandler(IEfRepository<UserEntity> users) : IQueryHandler<GetUserByEmailQuery, Entities.Users.User>
{
    private readonly IEfRepository<UserEntity> _users = users;

    public async Task<ServiceDataResult<Entities.Users.User>> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
    {
        var user = await _users.FirstOrDefaultAsync(u => u.Email == request.Email);

        var userResult = user == null ? null : new Entities.Users.User
        {
            UserId = user.UserId,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email
        };
        return ServiceDataResult<Entities.Users.User>.WithData(userResult);
    }
}
