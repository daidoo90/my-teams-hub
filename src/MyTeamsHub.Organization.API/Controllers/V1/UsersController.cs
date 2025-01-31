using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyTeamsHub.APIs.Core.Results;
using MyTeamsHub.APIs.Core.Results.Base;
using MyTeamsHub.APIs.Core.Services;
using MyTeamsHub.Domain.Entities.Users;
using MyTeamsHub.Domain.Services.Common;
using MyTeamsHub.Domain.Services.Organizations;
using MyTeamsHub.Domain.Services.Users;
using MyTeamsHub.Organization.API.Extensions;
using MyTeamsHub.Organization.API.Models.V1.Users;
using MyTeamsHub.Persistence.Models.Types;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace MyTeamsHub.Organization.API.Controllers.V1;

/// <summary>
/// Users operations
/// </summary>
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/users")]
public class UsersController : BaseApiController
{
    private readonly IUserService _userService;
    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly IOrganizationService _organizationService;

    /// <summary>
    /// Constructor
    /// </summary>
    public UsersController(
        IUserService userService,
        ICurrentUserProvider currentUserProvider,
        IOrganizationService organizationService)
    {
        _userService = userService;
        _currentUserProvider = currentUserProvider;
        _organizationService = organizationService;
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
        ServiceDataResult<User> getUserResult = await _userService.GetAsync(requestDto.Email, cancellationToken);
        if (!getUserResult.HasFailed && getUserResult.Data != null)
        {
            return ApiBadRequest.WithErrorCode(ErrorCodes.InvalidEmail);
        }


        var serviceDataResult = await _userService.CreateAsync(new NewUser
        {
            Email = requestDto.Email,
            FirstName = requestDto.FirstName,
            LastName = requestDto.LastName,
            Password = requestDto.Password,
            PhoneNumber = requestDto.PhoneNumber,
            UserStatus = (int)UserStatus.Active,
            UserType = (int)UserType.ClientAdmin
        }, cancellationToken);

        return serviceDataResult.ToActionResult();
    }

    /// <summary>
    /// Get current user info
    /// </summary>
    [HttpGet("me")]
    [SwaggerResponse((int)HttpStatusCode.OK, "Get current user metadata", typeof(ApiDataResult<User>))]
    public async Task<IActionResult> GetMyProfileAsync(CancellationToken cancellationToken)
    {
        var serviceDataResult = await _userService.GetAsync(_currentUserProvider.CurrentUserId, cancellationToken);

        return serviceDataResult.ToActionResult();
    }

    /// <summary>
    /// Get all employees
    /// </summary>
    [HttpGet()]
    [SwaggerResponse((int)HttpStatusCode.OK, "Get all employees", typeof(ApiDataResult<IEnumerable<ClientUser>>))]
    public async Task<IActionResult> GetAllEmployeesAsync(CancellationToken cancellationToken)
    {
        var serviceDataResult = await _organizationService.GetAllAsync(_currentUserProvider.CurrentUserId, cancellationToken);
        if (serviceDataResult.HasFailed)
        {
            return ApiBadRequest.WithErrorCode(serviceDataResult.ErrorCode);
        }

        if (!serviceDataResult.Data.Any())
        {
            return ApiOk.WithData<IEnumerable<UsersResponseDto>>(Array.Empty<UsersResponseDto>());
        }

        var employeesResult = await _userService.GetAllAsync(serviceDataResult.Data.Select(x => x.OrganizationId), cancellationToken);

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
        var serviceResult = await _userService.UpdateAsync(requestDto.UserId, requestDto.FirstName, requestDto.LastName, requestDto.Phone, requestDto.Status, requestDto.Title, cancellationToken);

        return serviceResult.ToActionResult();
    }
}
