namespace DataAccessLibrary.Models
{
    public class BikeCreateModel
    {
        public string Model { get; set; }
        public string Make { get; set; }
        public int Year { get; set; }
        public decimal Price { get; set; }
        public int Amount { get; set; }
    }
}
