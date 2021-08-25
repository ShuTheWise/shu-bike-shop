using System.Threading.Tasks;

namespace shu_bike_shop
{
    public interface ICookieService
    {
        Task<T> GetCookie<T>(string name);
        Task WriteCookie(string name, object value, int days);
    }
}