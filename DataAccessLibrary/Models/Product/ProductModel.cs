using System.Collections.Generic;
using System.Linq;

namespace DataAccessLibrary.Models
{
    public abstract class ProductModel
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public int Amount { get; set; }
        public string Category { get; set; }
        public string ImageUrl { get; set; }
        public abstract string Name { get; }

        public Dictionary<string, object> GetDescFields()
        {
            return GetType().GetProperties().Where(x => x.GetCustomAttributes(true).Any(x => x is DescriptionAttribute)).ToDictionary(x => x.Name, x => x.GetValue(this));
        }
    }
}
