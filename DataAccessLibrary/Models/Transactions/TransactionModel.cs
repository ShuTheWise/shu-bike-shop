namespace DataAccessLibrary.Models
{
    public class TransactionModel
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public int PaymentProductId { get; set; }
        public string Username { get; set; }
        public string CardholderName { get; set; }
        public int OrderId { get; set; }
        public string ErrorMessage { get; set; }
        public string ResponseMessage { get; set; }
    }
}
