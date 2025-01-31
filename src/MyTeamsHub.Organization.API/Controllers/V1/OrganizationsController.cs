using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using MyTeamsHub.APIs.Core.Results;
using MyTeamsHub.APIs.Core.Services;
using MyTeamsHub.Domain.Services.Common;
using MyTeamsHub.Domain.Services.Organizations;
using MyTeamsHub.Organization.API.Extensions;
using MyTeamsHub.Organization.API.Models.V1.Organizations;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace MyTeamsHub.Organization.API.Controllers.V1;

/// <summary>
/// Organizations operations
/// </summary>
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/organizations")]
[ApiController]
public class OrganizationsController : ControllerBase
{
    private readonly IOrganizationService _organizationService;
    private readonly ITeamService _teamService;
    private readonly ICurrentUserProvider _currentUserProvider;

    /// <summary>
    /// Constructor
    /// </summary>
    public OrganizationsController(
        IOrganizationService organizationService,
        ITeamService teamService,
        ICurrentUserProvider currentUserProvider)
    {
        _organizationService = organizationService;
        _teamService = teamService;
        _currentUserProvider = currentUserProvider;
    }

    /// <summary>
    /// Create new organization
    /// </summary>
    /// <param name="newOrganizationRequest">New organization metadata</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns></returns>
    [HttpPost]
    [SwaggerResponse((int)HttpStatusCode.OK, "Create organization", typeof(MyTeamsHub.APIs.Core.Results.Base.ApiDataResult<Guid>))]
    public async Task<IActionResult> CreateOrganizationAsync([FromBody] NewOrganizationRequestDto newOrganizationRequest, CancellationToken cancellationToken)
    {
        var serviceResult = await _organizationService.CreateAsync(newOrganizationRequest.Name, newOrganizationRequest.Description, _currentUserProvider.CurrentUserId, cancellationToken);

        return serviceResult.ToActionResult();
    }

    /// <summary>
    /// Get organizations
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Organizations metadata</returns>
    [HttpGet]
    [SwaggerResponse((int)HttpStatusCode.OK, "Get organization", typeof(MyTeamsHub.APIs.Core.Results.Base.ApiDataResult<GetUserOrganizationResponseDto>))]
    public async Task<ActionResult> GetOrganizationsAsync(CancellationToken cancellationToken)
    {
        var serviceDataResult = await _organizationService.GetAllAsync(_currentUserProvider.CurrentUserId, cancellationToken);
        if (serviceDataResult.HasFailed)
        {
            return ApiBadRequest.WithErrorCode(serviceDataResult.ErrorCode);
        }

        return ApiOk.WithData(serviceDataResult.Data.Select(ou => new GetUserOrganizationResponseDto
        {
            OrganizationId = ou.OrganizationId,
            OrganizationName = ou.OrganizationName,
            Role = ou.Role
        }));
    }

    /// <summary>
    /// Delete organization
    /// </summary>
    /// <param name="organizationId">Organization identifier</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpDelete]
    [Route("{organizationId}")]
    public async Task<IActionResult> DeleteOrganizationAsync(Guid organizationId, CancellationToken cancellationToken)
    {
        var serviceDataResult = await _organizationService.GetAllAsync(_currentUserProvider.CurrentUserId, cancellationToken);
        if (serviceDataResult.HasFailed)
        {
            return ApiBadRequest.WithErrorCode(serviceDataResult.ErrorCode);
        }

        if (serviceDataResult.Data?.All(o => o.OrganizationId != organizationId) ?? true)
        {
            return ApiBadRequest.WithErrorCode(Domain.Services.Common.ErrorCodes.InvalidOrganization);
        }

        var deletionResult = await _organizationService.DeleteAsync(organizationId, cancellationToken);
        return deletionResult.ToActionResult();
    }

    /// <summary>
    /// Create new team
    /// </summary>
    /// <param name="organizationId">Organization identifier</param>
    /// <param name="newTeamRequest">New team metadata</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpPost]
    [Route("{organizationId}/teams")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Create new team", typeof(MyTeamsHub.APIs.Core.Results.Base.ApiDataResult<Guid>))]
    public async Task<IActionResult> CreateTeamAsync([FromRoute] Guid organizationId, [FromBody] NewTeamRequestDto newTeamRequest, CancellationToken cancellationToken)
    {
        var serviceResult = await _teamService.CreateAsync(organizationId, newTeamRequest.Name, newTeamRequest.Description, cancellationToken);

        return serviceResult.ToActionResult();
    }

    /// <summary>
    /// Get teams from organization
    /// </summary>
    /// <param name="organizationId">Organization identifier</param>
    /// <param name="pageNumber">Page number. Default: 1</param>
    /// <param name="pageSize">Page size. Default: 10</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Teams metadata</returns>
    [HttpGet]
    [Route("{organizationId}/teams")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Get teams metadata", typeof(MyTeamsHub.APIs.Core.Results.Base.ApiDataResult<GetTeamsResponseDto>))]
    public async Task<IActionResult> GetTeamsAsync([FromRoute] string organizationId, [FromQuery] int? pageNumber, [FromQuery] int? pageSize, CancellationToken cancellationToken)
    {
        pageNumber = pageNumber ?? 1;
        pageSize = pageSize ?? 10;
        var serviceResult = await _teamService.GetAllAsync(Guid.Parse(organizationId), pageNumber.Value, pageSize.Value, cancellationToken);
        if (serviceResult.HasFailed)
        {
            return ApiBadRequest.WithErrorCode(serviceResult.ErrorCode);
        }

        return ApiOk.WithData(new GetTeamsResponseDto
        {
            Total = serviceResult.Data.Item2,
            Teams = serviceResult.Data.Item1.Select(x => new Team
            {
                Name = x.Name,
                TeamId = x.TeamId,
                TeamMembers = x.TeamMembers.Select(t => new TeamMember
                {
                    MemberId = t.User.UserId,
                    FirstName = t.User.FirstName,
                    LastName = t.User.LastName,
                    Phone = t.User.PhoneNumber,
                    Email = t.User.Email,
                    Status = (int)t.User.UserStatus,
                    Role = (int)t.MemberType
                }).ToList()
            }).ToList()
        });
    }

    /// <summary>
    /// Update team
    /// </summary>
    /// <param name="organizationId">Organization identifier</param>
    /// <param name="teamId">Team identifier</param>
    /// <param name="requestDto">Team metadata</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpPatch]
    [Route("{organizationId}/teams/{teamId}")]
    public async Task<IActionResult> UpdateTeamAsync([FromRoute] Guid organizationId, [FromRoute] Guid teamId, [FromBody] UpdateTeamRequestDto requestDto, CancellationToken cancellationToken)
    {
        var members = requestDto.TeamMembers
            .Select(t => (t.TeamMemberId, t.Email, t.IsLead));

        var serviceResult = await _teamService.UpdateAsync(organizationId, teamId, requestDto.Name, requestDto.Description, members, cancellationToken);

        return serviceResult.ToActionResult();
    }

    /// <summary>
    /// Delete team
    /// </summary>
    /// <param name="organizationId">Organization identifier</param>
    /// <param name="teamId">Team identifier</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpDelete]
    [Route("{organizationId}/teams/{teamId}")]
    public async Task<IActionResult> DeleteTeamAsync([FromRoute] Guid organizationId, [FromRoute] Guid teamId, CancellationToken cancellationToken)
    {
        var allTeamsResult = await _teamService.GetAllAsync(organizationId, 1, Int32.MaxValue, cancellationToken);
        if (allTeamsResult.HasFailed)
        {
            return ApiBadRequest.WithErrorCode(allTeamsResult.ErrorCode);
        }

        if (!allTeamsResult.Data.Item1.Any(t => t.TeamId == teamId))
        {
            return ApiBadRequest.WithErrorCode(Domain.Services.Common.ErrorCodes.InvalidTeamId);
        }

        var deletionResult = await _teamService.DeleteAsync(teamId, cancellationToken);

        return deletionResult.ToActionResult();
    }

    /// <summary>
    /// Get team metadata
    /// </summary>
    /// <param name="organizationId">Organization identifier</param>
    /// <param name="teamId">Team identifier</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Team metadata</returns>
    [HttpGet]
    [Route("{organizationId}/teams/{teamId}")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Get team metadata", typeof(MyTeamsHub.APIs.Core.Results.Base.ApiDataResult<Team>))]

    public async Task<IActionResult> GetTeamsAsync([FromRoute] string organizationId, [FromRoute] string teamId, CancellationToken cancellationToken)
    {
        var serviceResult = await _teamService.GetByIdAsync(Guid.Parse(organizationId), Guid.Parse(teamId), cancellationToken);
        if (serviceResult.HasFailed)
        {
            return ApiBadRequest.WithErrorCode(serviceResult.ErrorCode);
        }

        return ApiOk.WithData(new Team
        {
            TeamId = serviceResult.Data.TeamId,
            Name = serviceResult.Data.Name,
            Description = serviceResult.Data.Description,
            TeamMembers = serviceResult.Data.TeamMembers.Select(t => new TeamMember
            {
                MemberId = t.User.UserId,
                FirstName = t.User.FirstName,
                LastName = t.User.LastName,
                Phone = t.User.PhoneNumber,
                Email = t.User.Email,
                Status = (int)t.User.UserStatus,
                Role = (int)t.MemberType
            }).ToList()
        });
    }
}
