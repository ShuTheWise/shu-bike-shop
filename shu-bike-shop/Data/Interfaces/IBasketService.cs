using DataAccessLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace shu_bike_shop
{
    public interface IBasketService
    {
        Task<bool> AddProduct(ProductModel productModel);
        Task ClearBasket();
        Task<BasketItemModel> GetBasketItem(int productId);
        Task<List<BasketItemModel>> GetBasketItems();
        Task RaiseQuantity(int productId);
        Task RemoveProduct(int productId);
        Task SetBasketItems(List<BasketItemModel> basketItems);
    }
}