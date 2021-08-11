using DataAccessLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public class OrderData : IOrderData
    {
        private readonly ISqlDataAccess _db;

        public OrderData(ISqlDataAccess db)
        {
            _db = db;
        }

        public Task<List<OrderModel>> GetOrders(int userId)
        {
            string sql = @"select * from orders where userid = @UserId";
            return _db.LoadData<OrderModel, dynamic>(sql, new { userId });
        }

        public Task<List<OrderModel>> GetOrders()
        {
            string sql = @"select * from orders";
            return _db.LoadData<OrderModel, dynamic>(sql, new { });
        }

        public async Task<OrderModel> AddOrder(NewOrderModel orderModel)
        {
            string sql = @$"insert into orders (userid, status, totalamount) values (@UserId, '{OrderStatus.New}', @TotalAmount) returning id";
            var orderId = await _db.SaveData<NewOrderModel, int>(sql, orderModel);

            return new OrderModel(orderId, orderModel);
        }
    }
}
