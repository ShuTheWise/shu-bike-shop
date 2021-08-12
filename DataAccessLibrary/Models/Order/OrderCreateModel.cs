using System.Collections.Generic;

namespace DataAccessLibrary.Models
{
    public class OrderCreateModel
    {
        public string UserEmail { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderProductModel> Items { get; set; }
    }
}
