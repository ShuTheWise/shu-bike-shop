namespace DataAccessLibrary.Models
{
    public class OrderModel
    {
        public int Id { get; set; }
        public string UserEmail { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus? OrderStatus { get; set; }
        public PaymentStatus? PaymentStatus { get; set; }
        public int? TransactionId { get; set; }
    }
}
