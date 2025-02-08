// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Net;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using MyTeamsHub.Core.Application.Organization.Create;
using MyTeamsHub.Core.Application.Organization.Delete;
using MyTeamsHub.Core.Application.Organization.GetAll;
using MyTeamsHub.Domain.Services.Team.Delete;
using MyTeamsHub.Domain.Services.Team.GetAll;
using MyTeamsHub.Domain.Services.Team.GetById;
using MyTeamsHub.Domain.Services.Team.Update;
using MyTeamsHub.Organization.API.Extensions;
using MyTeamsHub.Organization.API.Models.Common;
using MyTeamsHub.Organization.API.Models.Common.Base;
using MyTeamsHub.Organization.API.Models.V1.Organizations;
using MyTeamsHub.Organization.API.Models.V1.Teams;
using MyTeamsHub.Organization.API.Services;

using Swashbuckle.AspNetCore.Annotations;

namespace MyTeamsHub.Organization.API.Controllers.V1;

/// <summary>
/// Organizations operations
/// </summary>
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/organizations")]
[ApiController]
public class OrganizationsController : ControllerBase
{
    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly IMediator _mediator;

    /// <summary>
    /// Constructor
    /// </summary>
    public OrganizationsController(
        ICurrentUserProvider currentUserProvider,
        IMediator mediator)
    {
        _currentUserProvider = currentUserProvider;
        _mediator = mediator;
    }

    /// <summary>
    /// Create new organization
    /// </summary>
    /// <param name="newOrganizationRequest">New organization metadata</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns></returns>
    [HttpPost]
    [SwaggerResponse((int)HttpStatusCode.OK, "Create organization", typeof(ApiDataResult<Guid>))]
    public async Task<IActionResult> CreateOrganizationAsync([FromBody] NewOrganizationRequestDto newOrganizationRequest, CancellationToken cancellationToken)
    {
        var command = new CreateOrganizationCommand(newOrganizationRequest.Name, newOrganizationRequest.Description, _currentUserProvider.CurrentUserId);
        var serviceResult = await _mediator.Send(command, cancellationToken);

        return serviceResult.ToActionResult();
    }

    /// <summary>
    /// Get organizations
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Organizations metadata</returns>
    [HttpGet]
    [SwaggerResponse((int)HttpStatusCode.OK, "Get organization", typeof(ApiDataResult<GetUserOrganizationResponseDto>))]
    public async Task<ActionResult> GetOrganizationsAsync(CancellationToken cancellationToken)
    {
        var query = new GetAllOrganizationsQuery(_currentUserProvider.CurrentUserId);
        var serviceDataResult = await _mediator.Send(query, cancellationToken);
        if (serviceDataResult.HasFailed)
        {
            return ApiBadRequest.WithErrorCode(serviceDataResult.ErrorCode);
        }

        return ApiOk.WithData(serviceDataResult.Data.Select(ou => new GetUserOrganizationResponseDto(ou.OrganizationId, ou.OrganizationName, ou.Role)));
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
        var command = new DeleteOrganizationCommand(organizationId);

        // todo: check if user has access to this organization?
        var deletionResult = await _mediator.Send(command, cancellationToken);
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
    [SwaggerResponse((int)HttpStatusCode.OK, "Create new team", typeof(ApiDataResult<Guid>))]
    public async Task<IActionResult> CreateTeamAsync([FromRoute] Guid organizationId, [FromBody] NewTeamRequestDto newTeamRequest, CancellationToken cancellationToken)
    {
        var command = new CreateOrganizationCommand(newTeamRequest.Name, newTeamRequest.Description, _currentUserProvider.CurrentUserId);

        var serviceResult = await _mediator.Send(command, cancellationToken);
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
    [SwaggerResponse((int)HttpStatusCode.OK, "Get teams metadata", typeof(ApiDataResult<GetTeamsResponseDto>))]
    public async Task<IActionResult> GetTeamsAsync([FromRoute] string organizationId, [FromQuery] int? pageNumber, [FromQuery] int? pageSize, CancellationToken cancellationToken)
    {
        pageNumber = pageNumber ?? 1;
        pageSize = pageSize ?? 10;
        var query = new GetAllTeamsQuery(Guid.Parse(organizationId), pageNumber.Value, pageSize.Value);
        var serviceResult = await _mediator.Send(query, cancellationToken);
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

        var command = new UpdateTeamCommand(requestDto.Name, requestDto.Description, organizationId, teamId, members);
        var serviceResult = await _mediator.Send(command, cancellationToken);

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
        // TODO: Validate that user has access to this team

        var command = new DeleteTeamCommand(teamId);
        var deletionResult = await _mediator.Send(command, cancellationToken);

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
    [SwaggerResponse((int)HttpStatusCode.OK, "Get team metadata", typeof(ApiDataResult<Team>))]

    public async Task<IActionResult> GetTeamsAsync([FromRoute] string organizationId, [FromRoute] string teamId, CancellationToken cancellationToken)
    {
        var query = new GetTeamByIdQuery(Guid.Parse(organizationId), Guid.Parse(teamId));
        var serviceResult = await _mediator.Send(query, cancellationToken);
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
