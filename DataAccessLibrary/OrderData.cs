﻿using DataAccessLibrary.Models;
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

        public Task<List<OrderModel>> GetOrders(string userEmail)
        {
            string sql = @"select * from orders where useremail = @userEmail";
            return _db.LoadData<OrderModel, dynamic>(sql, new { userEmail });
        }

        public Task<List<OrderModel>> GetOrders()
        {
            string sql = @"select * from orders";
            return _db.LoadData<OrderModel, dynamic>(sql, new { });
        }

        public async Task<OrderModel> AddOrder(NewOrderModel orderModel)
        {
            var orderStatus = OrderStatus.New;
            var paymentStatus = PaymentStatus.NotPaid;

            string sql = @$"insert into orders (useremail, orderstatus, paymentstatus, totalamount) values (@UserEmail, '{orderStatus}', '{paymentStatus}', @TotalAmount) returning id";

            var orderId = await _db.SaveData<NewOrderModel, int>(sql, orderModel);

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
            return _db.SaveData(sql, orderProductModel);
        }
    }
}
