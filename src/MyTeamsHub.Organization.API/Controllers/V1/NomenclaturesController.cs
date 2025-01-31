using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyTeamsHub.APIs.Core.Results;
using MyTeamsHub.Domain.Services.Common;

namespace MyTeamsHub.Organization.API.Controllers.V1;

/// <summary>
/// Nomenclatures
/// </summary>
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/nomenclatures")]
[Authorize]
public class NomenclaturesController : BaseApiController
{
    /// <summary>
    /// Get all available codes
    /// </summary>
    [HttpGet]
    [Route("codes")]
    public async Task<IActionResult> GetCodesAsync(CancellationToken cancellationToken)
    {
        var instance = Activator.CreateInstance<ErrorCodes>();
        var codes = typeof(ErrorCodes).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);

        var codeValues = codes.Select(c => c.GetValue(instance));

        return ApiOk.WithData(codeValues);
    }
}
