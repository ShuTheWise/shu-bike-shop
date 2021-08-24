using DataAccessLibrary.Models;

namespace shu_bike_shop
{
    public class OrderProductDetailsModel
    {
        public ProductModel ProductModel { get; set; }
        public OrderProductModel OrderProduct { get; set; }
    }
}