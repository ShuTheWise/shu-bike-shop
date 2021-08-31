using DataAccessLibrary.Models;
using System.Threading.Tasks;

namespace shu_bike_shop
{
    public interface IBasketModalService
    {
        Task AddProductToBasket(ProductModel productModel);
    }
}