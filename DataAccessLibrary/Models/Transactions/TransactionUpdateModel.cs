namespace DataAccessLibrary.Models
{
    public class TransactionUpdateModel
    {
        public int Id { get; set; }
        public string ErrorMessage { get; set; }
        public string ResponseMessage { get; set; }
        public string Status { get; set; }
    }
}
