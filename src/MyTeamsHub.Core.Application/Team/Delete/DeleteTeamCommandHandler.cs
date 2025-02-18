
using MyTeamsHub.Core.Application.Common;
using MyTeamsHub.Core.Application.Interfaces.Repositories;
using MyTeamsHub.Core.Domain.Organizations;

namespace MyTeamsHub.Domain.Services.Team.Delete;

public sealed record DeleteTeamCommand(Guid TeamId) : ICommand;

public class DeleteTeamCommandHandler(IEfRepository<TeamEntity> teams) : ICommandHandler<DeleteTeamCommand>
{
    private readonly IEfRepository<TeamEntity> _teams = teams;

    public async Task<ServiceResult> Handle(DeleteTeamCommand request, CancellationToken cancellationToken)
    {
        var team = await _teams.FirstOrDefaultAsync(t => t.TeamId == request.TeamId && !t.IsDeleted);
        if (team == null)
        {
            return ServiceResult.WithError(ErrorCodes.TeamNotFound);
        }

        team.DeletedOn = DateTime.UtcNow;
        team.IsDeleted = true;

        await _teams.UpdateAsync(team);
        return ServiceResult.Success;
    }
}
