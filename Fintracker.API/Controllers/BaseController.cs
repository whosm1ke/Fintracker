using Fintracker.Application.BusinessRuleConstraints;
using Microsoft.AspNetCore.Mvc;

namespace Fintracker.API.Controllers;

[ApiController]
public class BaseController : ControllerBase
{

    [NonAction]
    protected Guid GetCurrentUserId()
    {
        var uid = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypeConstants.Uid)?.Value;
        if (Guid.TryParse(uid, out var currentUserId))
            return currentUserId;
        return Guid.Empty;
    }
}