using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public abstract class ProductModel
    {
        public int Id { get; set; }
        public int Price { get; set; }

        //public abstract string Name { get; }
    }
}
