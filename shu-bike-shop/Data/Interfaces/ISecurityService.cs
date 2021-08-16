using System.Threading.Tasks;

namespace shu_bike_shop
{
    public interface ISecurityService
    {
        Task<User> GetCurrentUser();
    }
}