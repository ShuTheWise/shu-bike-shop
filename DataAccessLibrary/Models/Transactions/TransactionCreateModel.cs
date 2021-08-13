namespace DataAccessLibrary.Models
{
    public class TransactionCreateModel
    {
        public decimal Amount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string Username { get; set; }
        public int OrderId { get; set; }
    }
}
