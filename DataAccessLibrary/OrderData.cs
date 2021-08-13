using DataAccessLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public class OrderData : IOrderData
    {
        private readonly ISqlDataAccess db;

        public OrderData(ISqlDataAccess db)
        {
            this.db = db;
        }

        public Task<List<OrderModel>> GetOrders(string userEmail)
        {
            string sql = @"select * from orders where useremail = @userEmail order by id asc";
            return db.LoadData<OrderModel, dynamic>(sql, new { userEmail });
        }

        public Task<List<OrderModel>> GetOrders()
        {
            string sql = @"select * from orders";
            return db.LoadData<OrderModel, dynamic>(sql, new { });
        }

        public async Task<OrderModel> AddOrder(OrderCreateModel orderModel)
        {
            var orderStatus = OrderStatus.New;
            var paymentStatus = PaymentStatus.NotPaid;

            string sql = @$"insert into orders (useremail, orderstatus, paymentstatus, totalamount) values (@UserEmail, '{orderStatus}', '{paymentStatus}', @TotalAmount) returning id";
            var orderId = await db.SaveData<OrderCreateModel, int>(sql, orderModel);

            foreach (var item in orderModel.Items)
            {
                await AddOrderProducts(item, orderId);
            }

            return new OrderModel()
            {
                Id = orderId,
                UserEmail = orderModel.UserEmail,
                TotalAmount = orderModel.TotalAmount,
                OrderStatus = orderStatus,
                PaymentStatus = paymentStatus
            };
        }

        private Task AddOrderProducts(OrderProductModel orderProductModel, int orderId)
        {
            string sql = @$"insert into orderproducts (orderid, productid, amount, unitprice) values ({orderId}, @ProductId, @Amount, @UnitPrice)";
            return db.SaveData(sql, orderProductModel);
        }

        public async Task UpdateOrder(OrderUpdateModel orderModel)
        {
            string sql = @"update orders set orderStatus = @OrderStatus, paymentstatus = @PaymentStatus where id = @Id";
            await db.SaveData(sql, orderModel);
        }
    }
}
