namespace DataAccessLibrary.Models
{
    public class BikeCreateModel : ProductCreateModel
    {
        public string Model { get; set; }
        public string Make { get; set; }
        public int Year { get; set; }
    }
}
