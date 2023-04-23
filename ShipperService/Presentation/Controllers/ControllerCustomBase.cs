using IdentityModel;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace ShipperService.Presentation.Controllers
{
    public class ControllerCustomBase : ControllerBase
    {
        protected long? GetCurrentUserId()
        {
            var idClaim = User.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Id);
            if (idClaim == null)
                return null;
            return long.Parse(idClaim.Value);
        }
    }
}
