namespace DataAccessLibrary.Models
{
    public class BikeModel : ProductModel
    {
        public string Model { get; set; }
        public string Make { get; set; }
        public int Year { get; set; }

        public override string Name => $"{Make} {Model} {Year}";
    }
}
