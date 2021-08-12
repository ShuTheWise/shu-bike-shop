using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;

namespace shu_bike_shop
{
    public class SecurityService : ISecurityService
    {
        private IHttpContextAccessor httpContextAccessor;

        public SecurityService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public User CurrentUser
        {
            get
            {
                var user = httpContextAccessor.HttpContext.User;

                if (user.Identity.Name == null)
                {
                    return null;
                }

                return new User()
                {
                    Name = user.Identity.Name,
                    Role = GetRole(user)
                };
            }
        }

        private Role GetRole(ClaimsPrincipal User)
        {
            var role = ((ClaimsIdentity)User.Identity).Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .FirstOrDefault();

            if (role == null)
            {
                return Role.None;
            }

            return Enum.Parse<Role>(role.Value);
        }
    }
}
