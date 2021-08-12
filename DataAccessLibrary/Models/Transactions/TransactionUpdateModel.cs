namespace DataAccessLibrary.Models
{
    public class TransactionUpdateModel
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }
}
