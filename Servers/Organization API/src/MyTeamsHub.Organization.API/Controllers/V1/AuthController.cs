//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;

//using MyTeamsHub.APIs.Core.Results;
//using MyTeamsHub.Core.Application.Auth;
//using MyTeamsHub.Domain.Services.Auth;
//using MyTeamsHub.Domain.Services.Users;
//using MyTeamsHub.Organization.API.Extensions;
//using MyTeamsHub.Organization.API.Models.V1.Auth;

//namespace MyTeamsHub.Organization.API.Controllers.V1;

///// <summary>
///// Authentication operations
///// </summary>
//[ApiVersion("1.0")]
//[Route("api/v{version:apiVersion}/auth")]
//public class AuthController : BaseApiController
//{
//    private readonly IIdentityService _identityService;
//    private readonly IUserService _userService;

//    /// <summary>
//    /// Constructor
//    /// </summary>
//    public AuthController(
//        IIdentityService authService,
//        IUserService identityService)
//    {
//        _identityService = authService;
//        _userService = identityService;
//    }

//    /// <summary>
//    /// Authenticate
//    /// </summary>
//    /// <returns>User's metadata</returns>
//    [HttpPost]
//    [Route("login")]
//    [AllowAnonymous]
//    public async Task<IActionResult> LoginAsync([FromBody] LoginRequestDto loginRequestDto, CancellationToken cancellationToken)
//    {
//        var serviceResult = await _userService.TryGetAsync(loginRequestDto.Email, loginRequestDto.Password, cancellationToken);
//        if (serviceResult.HasFailed)
//        {
//            return ApiBadRequest.WithErrorCode(serviceResult.ErrorCode!);
//        }

//        var tokenServiceResult = _identityService.GetToken(serviceResult.Data);

//        return tokenServiceResult.ToActionResult();
//    }
//}
