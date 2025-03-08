using Microsoft.AspNetCore.Mvc;

using MyTeamsHub.Core.Application.Common;
using MyTeamsHub.Organization.API.Models.Common;

namespace MyTeamsHub.Organization.API.Controllers.V1;

/// <summary>
/// Nomenclatures
/// </summary>
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/nomenclatures")]
public class NomenclaturesController : BaseApiController
{
    /// <summary>
    /// Get all available codes
    /// </summary>
    [HttpGet]
    [Route("codes")]
    public IActionResult GetCodesAsync(CancellationToken cancellationToken)
    {
        var instance = Activator.CreateInstance<ErrorCodes>();
        var codes = typeof(ErrorCodes).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);

        var codeValues = codes.Select(c => c.GetValue(instance));

        return ApiOk.WithData(codeValues);
    }
}
