using DataAccessLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public class ProductData : IProductData
    {
        private readonly ISqlDataAccess _db;

        public ProductData(ISqlDataAccess db)
        {
            _db = db;
        }

        public Task<List<ProductModel>> GetProducts()
        {
            string sql = "SELECT * FROM bikesview";
            return _db.LoadData<ProductModel, dynamic>(sql, new { });
        }

        public Task DecrementProductAmount(int id, int amount)
        {
            string sql = "update products set amount = amount - @amount where id = @Id";
            return _db.SaveData(sql, new { id, amount });
        }
    }
}
