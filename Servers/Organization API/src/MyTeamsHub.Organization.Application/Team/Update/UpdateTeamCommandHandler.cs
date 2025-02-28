using Microsoft.EntityFrameworkCore;

using MyTeamsHub.Core.Application.Common;
using MyTeamsHub.Core.Application.Interfaces.Repositories;
using MyTeamsHub.Core.Domain.Organizations;
using MyTeamsHub.Core.Domain.Users;

namespace MyTeamsHub.Domain.Services.Team.Update;

public sealed record UpdateTeamCommand(string Name, string Description, Guid OrganizationId, Guid TeamId, IEnumerable<(Guid?, string, bool)> Users) : ICommand;

public class UpdateTeamCommandHandler(
    IEfRepository<TeamEntity> teams,
    IEfRepository<UserEntity> users)
    : ICommandHandler<UpdateTeamCommand>
{
    private readonly IEfRepository<TeamEntity> _teams = teams;
    private readonly IEfRepository<UserEntity> _users = users;

    public async Task<ServiceResult> Handle(UpdateTeamCommand request, CancellationToken cancellationToken)
    {
        var team = await _teams
            .Query()
            .Include(t => t.TeamMembers)
            .FirstOrDefaultAsync(t => t.OrganizationId == request.OrganizationId && t.TeamId == request.TeamId);

        if (team == null)
        {
            return ServiceResult.WithError(ErrorCodes.InvalidTeamId);
        }

        if (request.Users.Count(u => u.Item3) > 1)
        {
            return ServiceResult.WithError(ErrorCodes.OnlyOneTeamLeadAllowed);
        }

        if (string.IsNullOrEmpty(request.Name))
        {
            return ServiceResult.WithError(ErrorCodes.InvalidTeamName);
        }

        if (string.IsNullOrEmpty(request.Description))
        {
            return ServiceResult.WithError(ErrorCodes.InvalidTeamDescription);
        }

        team.Name = request.Name;
        team.Description = request.Description;

        var newlyInvitedUsers = request.Users
            .Where(u => !u.Item1.HasValue || u.Item1 == Guid.Empty)
            .Select(u => new UserEntity
            {
                UserStatus = UserStatus.Invited,
                UserType = UserType.ClientUser,
                Email = u.Item2
            })
            .ToArray();

        // TODO: Send invitation emails

        await _users.AddRangeAsync(newlyInvitedUsers);

        var members = newlyInvitedUsers
            .Select(u => new TeamMemberEntity
            {
                UserId = u.UserId,
                User = u,
                MemberType = request.Users.Single(user =>
                                        (user.Item1.HasValue && user.Item1.Value == u.UserId) ||
                                        (user.Item2.Equals(u.Email))).Item3 ? RoleType.TeamLead : RoleType.RegularMember
            })
            .ToList();

        members.AddRange(request.Users
            .Where(u => u.Item1.HasValue && u.Item1 != Guid.Empty)
            .Select(u => new TeamMemberEntity
            {
                UserId = u.Item1!.Value,
                MemberType = u.Item3 ? RoleType.TeamLead : RoleType.RegularMember
            })
            .ToList());

        team.TeamMembers = members;

        await _teams.UpdateAsync(team, cancellationToken);
        return ServiceResult.Success;
    }
}
