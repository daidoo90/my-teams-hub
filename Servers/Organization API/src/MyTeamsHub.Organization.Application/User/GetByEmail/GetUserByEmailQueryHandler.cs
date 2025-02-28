using MyTeamsHub.Core.Application.Common;
using MyTeamsHub.Core.Application.Interfaces.Repositories;
using MyTeamsHub.Core.Domain.Users;

namespace MyTeamsHub.Core.Application.User.GetByEmail;

public sealed record GetUserByEmailQuery(string Email) : IQuery<Domain.Users.User>;

public class GetUserByEmailQueryHandler(IEfRepository<UserEntity> users) : IQueryHandler<GetUserByEmailQuery, Domain.Users.User>
{
    private readonly IEfRepository<UserEntity> _users = users;

    public async Task<ServiceDataResult<Domain.Users.User>> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
    {
        var user = await _users.FirstOrDefaultAsync(u => u.Email == request.Email);

        var userResult = user == null ? null : new Domain.Users.User
        {
            UserId = user.UserId,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email
        };
        return ServiceDataResult<Domain.Users.User>.WithData(userResult);
    }
}
