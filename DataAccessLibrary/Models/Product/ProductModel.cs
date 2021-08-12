namespace DataAccessLibrary.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public int Amount { get; set; }
        public virtual string Name { get; }
    }
}
