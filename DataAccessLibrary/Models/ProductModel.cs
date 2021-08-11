namespace DataAccessLibrary.Models
{
    public abstract class ProductModel
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public int Amount { get; set; }

        public abstract string Name { get; }
    }
}
