namespace shu_bike_shop.Data
{
    public class Bike : Product
    {
        public string Model { get; set; }
        public string Make { get; set; }
        public override string Name => Make + " " + Model; 
    }
}
