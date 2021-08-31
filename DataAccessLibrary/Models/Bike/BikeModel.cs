using System.Collections.Generic;

namespace DataAccessLibrary.Models
{
    public class BikeModel : ProductModel
    {
        [Description("General")]
        public string Make { get; set; }

        [Description("General")]
        public string Model { get; set; }

        [Description("General")]
        public int Year { get; set; }

        public override string Name => $"{Make} {Model} {Year}";
    }
}
