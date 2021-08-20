namespace DataAccessLibrary.Models
{
    public class TransactionCreateModel
    {
        public decimal Amount { get; set; }
        public int PaymentProductId { get; set; }
        public string Username { get; set; }
        public string CardholderName { get; set; }
        public int OrderId { get; set; }
        public string ErrorMessage { get; set; }
        public string ResponseMessage { get; set; }
        public string Status { get; set; }
        public long PaymentId { get; set; }
        public int PaymentIdSuffix { get; set; }
    }
}
