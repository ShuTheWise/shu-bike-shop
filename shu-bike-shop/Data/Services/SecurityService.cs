using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace shu_bike_shop
{
    public class SecurityService : ISecurityService
    {
        private IHttpContextAccessor httpContextAccessor;
        private IUserService userService;

        public SecurityService(IHttpContextAccessor httpContextAccessor, IUserService userService)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.userService = userService;
        }

        public async Task<User> GetCurrentUser()
        {
            var user = httpContextAccessor.HttpContext?.User;
            if (user?.Identity?.Name == null)
            {
                return null;
            }

            var role = GetRole(user);
            if (role == Role.None)
            {
                await userService.AssignRole(user.Identity.Name);
            }

            return new User()
            {
                Name = user.Identity.Name,
                Role = GetRole(user)
            };
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
