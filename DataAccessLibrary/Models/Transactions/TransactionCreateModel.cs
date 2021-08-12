namespace DataAccessLibrary.Models
{
    public class TransactionCreateModel
    {
        public decimal Amount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }
}
