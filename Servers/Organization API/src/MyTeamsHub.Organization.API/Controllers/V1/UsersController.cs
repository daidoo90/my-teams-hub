using System.Net;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using MyTeamsHub.Core.Application.Organization.GetAll;
using MyTeamsHub.Core.Application.User.Create;
using MyTeamsHub.Core.Application.User.GetAll;
using MyTeamsHub.Core.Application.User.GetById;
using MyTeamsHub.Core.Application.User.Update;
using MyTeamsHub.Core.Domain.Users;
using MyTeamsHub.Organization.API.Extensions;
using MyTeamsHub.Organization.API.Models.Common;
using MyTeamsHub.Organization.API.Models.Common.Base;
using MyTeamsHub.Organization.API.Models.V1.Users;
using MyTeamsHub.Organization.API.Services;

using Swashbuckle.AspNetCore.Annotations;

namespace MyTeamsHub.Organization.API.Controllers.V1;

/// <summary>
/// Users operations
/// </summary>
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/users")]
public class UsersController : BaseApiController
{
    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly IMediator _mediator;

    /// <summary>
    /// Constructor
    /// </summary>
    public UsersController(
        ICurrentUserProvider currentUserProvider,
        IMediator mediator)
    {
        _currentUserProvider = currentUserProvider;
        _mediator = mediator;
    }

    /// <summary>
    /// Sign up
    /// </summary>
    /// <returns>Organizations metadata</returns>
    [HttpPost("signup")]
    [AllowAnonymous]
    [SwaggerResponse((int)HttpStatusCode.OK, "Signup", typeof(ApiDataResult<NewUser>))]
    public async Task<IActionResult> SignUpAsync([FromBody] UserSignupRequestDto requestDto, CancellationToken cancellationToken)
    {
        //ServiceDataResult<User> getUserResult = await _userService.GetAsync(requestDto.Email, cancellationToken);
        //if (!getUserResult.HasFailed && getUserResult.Data != null)
        //{
        //    return ApiBadRequest.WithErrorCode(ErrorCodes.InvalidEmail);
        //}

        var command = new CreateUserCommand(requestDto.FirstName, requestDto.LastName, requestDto.PhoneNumber, requestDto.Email, requestDto.Password, (int)UserStatus.Active, (int)UserType.ClientAdmin);
        var serviceDataResult = await _mediator.Send(command, cancellationToken);

        return serviceDataResult.ToActionResult();
    }

    /// <summary>
    /// Get current user info
    /// </summary>
    [HttpGet("me")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Get current user metadata", typeof(ApiDataResult<User>))]
    public async Task<IActionResult> GetMyProfileAsync(CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery(_currentUserProvider.CurrentUserId);
        var serviceDataResult = await _mediator.Send(query, cancellationToken);

        return serviceDataResult.ToActionResult();
    }

    /// <summary>
    /// Get all employees
    /// </summary>
    [HttpGet()]
    [SwaggerResponse((int)HttpStatusCode.OK, "Get all employees", typeof(ApiDataResult<IEnumerable<ClientUser>>))]
    public async Task<IActionResult> GetAllEmployeesAsync(CancellationToken cancellationToken)
    {
        var organizationsQuery = new GetAllOrganizationsQuery(_currentUserProvider.CurrentUserId);
        var serviceDataResult = await _mediator.Send(organizationsQuery, cancellationToken);
        if (serviceDataResult.HasFailed)
        {
            return ApiBadRequest.WithErrorCode(serviceDataResult.ErrorCode);
        }

        if (!serviceDataResult.Data.Any())
        {
            return ApiOk.WithData<IEnumerable<UsersResponseDto>>(Array.Empty<UsersResponseDto>());
        }

        var query = new GetAllUsersQuery(serviceDataResult.Data.Select(x => x.OrganizationId));
        var employeesResult = await _mediator.Send(query, cancellationToken);

        return employeesResult.ToActionResult();
    }

    /// <summary>
    /// Update user's metadata
    /// </summary>
    /// <param name="requestDto">User's metadata</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [HttpPatch]
    public async Task<IActionResult> UpdateAsync(UpdateUserRequestDto requestDto, CancellationToken cancellationToken)
    {
        var command = new UpdateUserCommand(requestDto.UserId, requestDto.FirstName, requestDto.LastName, requestDto.Phone, requestDto.Status, requestDto.Title);
        var serviceResult = await _mediator.Send(command, cancellationToken);

        return serviceResult.ToActionResult();
    }
}
