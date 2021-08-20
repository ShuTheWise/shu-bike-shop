using System.Threading.Tasks;

namespace shu_bike_shop
{
    public interface ILogger
    {
        Task Log(string content);
    }
}