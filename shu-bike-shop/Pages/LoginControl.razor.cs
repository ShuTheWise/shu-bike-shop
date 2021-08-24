using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace shu_bike_shop.Pages
{
    public partial class LoginControl
    {
        private static IEnumerable<string> GetRoles(ClaimsPrincipal User)
        {
            var roles = ((ClaimsIdentity)User.Identity).Claims
                    .Where(c => c.Type == ClaimTypes.Role)
                    .Select(c => c.Value);

            return roles;
        }
    }
}
