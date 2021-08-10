using DataAccessLibrary;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace shu_bike_shop
{
    public class UserService : IUserService
    {
        private IUserData userData;

        public UserService(IUserData userData)
        {
            this.userData = userData;
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
    }
}