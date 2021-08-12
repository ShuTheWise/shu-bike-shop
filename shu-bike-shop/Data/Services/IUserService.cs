using System.Threading.Tasks;

namespace shu_bike_shop
{
    public interface IUserService
    {
        Task AssignRole(string username);
    }
}