using DataAccessLibrary;
using DataAccessLibrary.Models;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Security.Claims;
using System;

namespace shu_bike_shop
{
    public class UserService : IUserService
    {
        private IUserData userData;
        private IHttpContextAccessor httpContextAccessor;

        public UserService(IUserData userData, IHttpContextAccessor httpContextAccessor)
        {
            this.userData = userData;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<UserModel> Login(UserLoginModel user)
        {
            var authernicatedUser = await userData.AuthenticateUser(user.EmailAddress, user.EncryptedPassword);
            return authernicatedUser;
        }

        public async Task RegisterUser(UserSignupModel user)
        {
            var encryptedPassword = Utility.Encrypt(user.Password);

            var userModel = new UserModel()
            {
                Email = user.EmailAddress,
                EncryptedPassword = encryptedPassword,
                Role = Role.User
            };

            await userData.AddUser(userModel);
        }

        public Task<User> CurrentUser
        {
            get
            {
                var user = httpContextAccessor.HttpContext.User;

                return Task.FromResult(new User()
                {
                    Email = user.Identity.Name,
                    Role = GetRole(user)
                });
            }
        }

        private Role GetRole(ClaimsPrincipal User)
        {
            var role = ((ClaimsIdentity)User.Identity).Claims
                    .Where(c => c.Type == ClaimTypes.Role)
                    .First().Value;

            return (Role)Enum.Parse(typeof(Role), role);
        }
    }
}