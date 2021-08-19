using DataAccessLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public interface IOrderData
    {
        Task<OrderModel> AddOrder(OrderCreateModel orderModel);
        Task<List<OrderModel>> GetOrders();
        Task<OrderModel> GetOrder(int id, string userEmail);
        Task<List<OrderModel>> GetOrders(string userEmail);
        Task UpdateOrder(OrderUpdateModel orderModel);
    }
}