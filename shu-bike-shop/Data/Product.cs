using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shu_bike_shop.Data
{
    public abstract class Product
    {
        public abstract string Name { get; }
        public int Id { get; set; }
        public int Price { get; set; }
    }
}
