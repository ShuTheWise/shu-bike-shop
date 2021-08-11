using System;
using System.Linq;
using System.Text;

namespace DataAccessLibrary.Models
{
    public class OrderModel
    {
        public OrderModel(int id, NewOrderModel newOrderModel)
        {
            Id = id;
            UserEmail = newOrderModel.UserEmail;
            TotalAmount = newOrderModel.TotalAmount;
            Status = OrderStatus.New;
        }

        public int Id { get; set; }
        public string UserEmail { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public bool Paid { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }
    }
}
