using DataAccessLibrary.Models;

namespace shu_bike_shop
{
    public class BasketItemModel
    {
        public ProductModel Product { get; set; }
        public int Quantity { get; set; } = 1;

        public decimal Price => Product.Price * Quantity;
    }
}
