namespace DataAccessLibrary.Models
{
    public class OrderProductModel
    {
        public int ProductId { get; set; }
        public int Amount { get; set; }
        public decimal UnitPrice { get; set; }

        public decimal TotalPrice => UnitPrice * Amount;
    }
}
