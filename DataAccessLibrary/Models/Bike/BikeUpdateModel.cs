namespace DataAccessLibrary.Models
{
    public class BikeUpdateModel
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public int Amount { get; set; }
        public string Model { get; set; }
        public string Make { get; set; }
        public int Year { get; set; }
    }
}
