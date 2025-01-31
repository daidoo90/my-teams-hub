using Microsoft.EntityFrameworkCore;
using MyTeamsHub.Domain.Services.Common;
using MyTeamsHub.Persistence.Core.Repository;
using MyTeamsHub.Persistence.Models.Organizations;
using MyTeamsHub.Persistence.Models.Users;

namespace MyTeamsHub.Domain.Services.Organizations;

public interface ITeamService
{
    Task<ServiceDataResult<Guid>> CreateAsync(Guid organizationid, string teamName, string description, CancellationToken cancellationToken = default);

    Task<ServiceDataResult<(IEnumerable<TeamEntity>, int)>> GetAllAsync(Guid organizationId, int pageNumber, int pageSize, CancellationToken cancellationToken = default);

    Task<ServiceResult> UpdateAsync(Guid organizationId, Guid teamId, string name, string description, IEnumerable<(Guid?, string, bool)> users, CancellationToken cancellationToken = default);

    Task<ServiceResult> DeleteAsync(Guid teamId, CancellationToken cancellationToken = default);

    Task<ServiceDataResult<TeamEntity>> GetByIdAsync(Guid organizationId, Guid teamId, CancellationToken cancellationToken = default);
}

public class TeamService : ITeamService
{
    private readonly IEfRepository<TeamEntity> _teams;
    private readonly IEfRepository<OrganizationEntity> _organizations;
    private readonly IEfRepository<UserEntity> _users;

    public TeamService(
        IEfRepository<TeamEntity> teams,
        IEfRepository<OrganizationEntity> organizations,
        IEfRepository<UserEntity> users)
    {
        _teams = teams;
        _organizations = organizations;
        _users = users;
    }

    public async Task<ServiceDataResult<Guid>> CreateAsync(Guid organizationid, string teamName, string description, CancellationToken cancellationToken = default)
    {
        if (organizationid.Equals(Guid.Empty))
        {
            return ServiceDataResult<Guid>.WithError(ErrorCodes.InvalidOrganization);
        }

        var organization = await _organizations
            .Query()
            .Include(o => o.Teams)
            .FirstOrDefaultAsync(o => o.OrganizationId == organizationid, cancellationToken);

        if (organization == null)
        {
            return ServiceDataResult<Guid>.WithError(ErrorCodes.InvalidOrganization);
        }

        if (string.IsNullOrEmpty(teamName))
        {
            return ServiceDataResult<Guid>.WithError(ErrorCodes.InvalidTeamName);
        }

        if (string.IsNullOrEmpty(description))
        {
            return ServiceDataResult<Guid>.WithError(ErrorCodes.InvalidTeamDescription);
        }

        var team = organization.Teams.FirstOrDefault(t => t.Name.Contains(teamName));
        if (team != null &&
            string.Equals(team.Name, teamName, StringComparison.OrdinalIgnoreCase))
        {
            return ServiceDataResult<Guid>.WithError(ErrorCodes.TeamAlreadyExists);
        }

        var newTeam = new TeamEntity
        {
            OrganizationId = organizationid,
            Name = teamName,
            Description = description
        };

        await _teams.AddAsync(newTeam);
        return ServiceDataResult<Guid>.Created(newTeam.TeamId);
    }

    public async Task<ServiceResult> DeleteAsync(Guid teamId, CancellationToken cancellationToken = default)
    {
        var team = await _teams.FirstOrDefaultAsync(t => t.TeamId == teamId && !t.IsDeleted);
        if (team == null)
        {
            return ServiceResult.WithError(ErrorCodes.TeamNotFound);
        }

        team.DeletedOn = DateTime.UtcNow;
        team.IsDeleted = true;

        await _teams.UpdateAsync(team);
        return ServiceResult.Success;
    }

    public async Task<ServiceDataResult<(IEnumerable<TeamEntity>, int)>> GetAllAsync(Guid organizationId, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        if (organizationId == Guid.Empty)
        {
            return ServiceDataResult<(IEnumerable<TeamEntity>, int)>.WithError(ErrorCodes.InvalidOrganization);
        }

        var organizationExists = await _organizations.AnyAsync(o => o.OrganizationId == organizationId);
        if (!organizationExists)
        {
            return ServiceDataResult<(IEnumerable<TeamEntity>, int)>.WithError(ErrorCodes.InvalidOrganization);
        }

        var organization = await _organizations.Query()
            .Include(o => o.Teams)
            .ThenInclude(x => x.TeamMembers)
            .ThenInclude(x => x.User)
            .FirstOrDefaultAsync(o => o.OrganizationId == organizationId, cancellationToken);

        var teams = organization.Teams
            .Where(t => !t.IsDeleted);

        var totalTeams = teams.Count();

        return ServiceDataResult<(IEnumerable<TeamEntity>, int)>.WithData((teams.
            Skip((pageNumber - 1) * pageSize)
            .Take(pageSize), totalTeams));
    }

    public async Task<ServiceDataResult<TeamEntity>> GetByIdAsync(Guid organizationId, Guid teamId, CancellationToken cancellationToken = default)
    {
        var team = await _teams.Query()
            .Include(t => t.TeamMembers)
            .ThenInclude(tm => tm.User)
            .FirstOrDefaultAsync(t => t.OrganizationId == organizationId && t.TeamId == teamId, cancellationToken);

        return ServiceDataResult<TeamEntity>.WithData(team);
    }

    public async Task<ServiceResult> UpdateAsync(Guid organizationId, Guid teamId, string name, string description, IEnumerable<(Guid?, string, bool)> users, CancellationToken cancellationToken = default)
    {
        var team = await _teams
            .Query()
            .Include(t => t.TeamMembers)
            .FirstOrDefaultAsync(t => t.OrganizationId == organizationId && t.TeamId == teamId);

        if (team == null)
        {
            return ServiceResult.WithError(ErrorCodes.InvalidTeamId);
        }

        if(users.Count(u => u.Item3) > 1)
        {
            return ServiceResult.WithError(ErrorCodes.OnlyOneTeamLeadAllowed);
        }

        if(string.IsNullOrEmpty(name))
        {
            return ServiceResult.WithError(ErrorCodes.InvalidTeamName);
        }

        if (string.IsNullOrEmpty(description))
        {
            return ServiceResult.WithError(ErrorCodes.InvalidTeamDescription);
        }

        team.Name = name;
        team.Description = description;

        var newlyInvitedUsers = users
            .Where(u => !u.Item1.HasValue || u.Item1 == Guid.Empty)
            .Select(u => new UserEntity
            {
                UserStatus = Persistence.Models.Types.UserStatus.Invited,
                UserType = Persistence.Models.Types.UserType.ClientUser,
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
                MemberType = users.Single(user => 
                                        (user.Item1.HasValue && user.Item1.Value == u.UserId) ||
                                        (user.Item2.Equals(u.Email))).Item3 ? Persistence.Models.Types.RoleType.TeamLead : Persistence.Models.Types.RoleType.RegularMember
            })
            .ToList();

        members.AddRange(users
            .Where(u => u.Item1.HasValue && u.Item1 != Guid.Empty)
            .Select(u => new TeamMemberEntity
            {
                UserId = u.Item1!.Value,
                MemberType = u.Item3 ? Persistence.Models.Types.RoleType.TeamLead : Persistence.Models.Types.RoleType.RegularMember
            })
            .ToList());

        team.TeamMembers = members;

        await _teams.UpdateAsync(team, cancellationToken);
        return ServiceResult.Success;
    }
}
