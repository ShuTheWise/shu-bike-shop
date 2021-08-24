using DataAccessLibrary.Models;

namespace shu_bike_shop
{
    public class OrderProductDetailsModel
    {
        public ProductModel productModel { get; set; }
        public int Amount { get; set; }
    }
}