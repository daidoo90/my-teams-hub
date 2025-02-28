using Microsoft.EntityFrameworkCore;

using MyTeamsHub.Core.Application.Common;
using MyTeamsHub.Core.Application.Interfaces.Repositories;
using MyTeamsHub.Core.Domain.Users;

namespace MyTeamsHub.Core.Application.User.GetAll;

public sealed record GetAllUsersQuery(IEnumerable<Guid> Organizations) : IQuery<IEnumerable<ClientUser>>;

public class GetAllUsersQueryHandler(IEfRepository<UserEntity> users) : IQueryHandler<GetAllUsersQuery, IEnumerable<ClientUser>>
{
    private readonly IEfRepository<UserEntity> _users = users;

    public async Task<ServiceDataResult<IEnumerable<ClientUser>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _users
            .Query()
            .Include(u => u.TeamMembers)
            .ThenInclude(tm => tm.Team)
            .Where(u => u.TeamMembers.First().Team.OrganizationId == request.Organizations.First())
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
                         .Select(tm => new Core.Domain.Users.Team
                         {
                             TeamId = tm.TeamId,
                             Name = tm.Team.Name
                         })
            })
            .ToListAsync(cancellationToken);

        return ServiceDataResult<IEnumerable<ClientUser>>.WithData(users);
    }
}
