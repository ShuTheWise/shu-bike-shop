using DataAccessLibrary;
using DataAccessLibrary.Models;
using System.Threading.Tasks;

namespace shu_bike_shop
{
    public interface IUserService
    {
        User CurrentUser { get; }
        Task<UserModel> Login(UserLoginModel user);
        Task RegisterUser(UserSignupModel user);
    }
}