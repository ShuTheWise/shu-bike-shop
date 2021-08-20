using DataAccessLibrary.Models;
using System.Collections.Generic;
using System.Linq;
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
            string sql = @"select * from orders order by id asc";
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

        public async Task UpdateOrderByOrderId(int orderId, dynamic updateModel)
        {
            object o = updateModel;
            var set = string.Join(", ", o.GetType().GetProperties().Select(p => $"{p.Name} = @{p.Name}"));
            string sql = $"update orders set {set} where id = '{orderId}'";
            await db.SaveData(sql, updateModel);
        }

        public async Task UpdatePaymentStatus(int orderId, PaymentStatus status)
        {
            string sql = @"update orders set paymentstatus = @status where id = @orderid";
            await db.SaveData(sql, new { orderId, status });
        }

        public Task<OrderModel> GetOrder(int id, string userEmail)
        {
            string sql = @"select * from orders where id = @id and useremail = @userEmail";
            return db.LoadSingle<OrderModel, dynamic>(sql, new { userEmail, id });
        }

        private Task<List<OrderProductModel>> GetOrderProducts(int orderId)
        {
            string sql = "select * from  orderproducts where orderid = @orderId";
            return db.LoadData<OrderProductModel, dynamic>(sql, new { orderId });
        }
    }
}
