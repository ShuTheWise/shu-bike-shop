using System.Threading.Tasks;

namespace shu_bike_shop
{
    public interface IModal
    {
        Task<ModalResult> GetModalResult();
    }
}