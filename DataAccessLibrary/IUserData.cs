using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public interface IUserData
    {
        Task AddUser(UserModel userModel);
        Task<List<UserModel>> GetUsers();
        Task<UserModel> AuthenticateUser(string email, string encryptedPassword);
    }
}
