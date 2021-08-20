using System.Collections.Generic;

namespace DataAccessLibrary.Models
{
    public class OrderUpdateModel
    {
        public int Id { get; set; }
        public OrderStatus? OrderStatus { get; set; }
        public PaymentStatus? PaymentStatus { get; set; }
    }
}
