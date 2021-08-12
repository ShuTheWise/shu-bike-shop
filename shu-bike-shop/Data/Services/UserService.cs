using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace shu_bike_shop
{
    public class UserService : IUserService
    {
        public static string ADMINISTRATION_ROLE = "Administrators";
        public static string USER_ROLE = "Users";

        private UserManager<IdentityUser> userManager;
        private RoleManager<IdentityRole> roleManager;

        public UserService(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;


            var x =
            userManager.Users;

            foreach (var item in x)
            {
            }
        }

        public async Task AssignRole(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return;
            }

            var user = await userManager.FindByNameAsync(username);
            if (user != null)
            {
                var newRole = (username.ToLower().StartsWith("admin")) ? ADMINISTRATION_ROLE : USER_ROLE;

                var role = await roleManager.FindByNameAsync(USER_ROLE);
                if (role == null)
                {
                    await roleManager.CreateAsync(new IdentityRole(USER_ROLE));
                }

                var userResult = await userManager.IsInRoleAsync(user, newRole);
                if (!userResult)
                {
                    await userManager.AddToRoleAsync(user, newRole);
                }
            }
            return;
        }
    }
}
