using DataAccessLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLibrary
{

    public class ProductData : IProductData
    {
        private readonly ISqlDataAccess db;

        public ProductData(ISqlDataAccess db)
        {
            this.db = db;
        }

        public Task<List<ProductModel>> GetProducts()
        {
            string sql = "SELECT * FROM bikesview";
            return db.LoadData<ProductModel, dynamic>(sql, new { });
        }

        public Task DecrementProductAmount(int id, int amount)
        {
            string sql = "update products set amount = amount - @amount where id = @Id";
            return db.SaveData(sql, new { id, amount });
        }

        public async Task<ProductModel> AddProduct(NewProductModel model)
        {
            string sql = @$"insert into products (price, amount) values (@Price, @Amount) returning id";
            
            int productId = await db.SaveData<NewProductModel, int>(sql, model);

            return new ProductModel()
            {
                Id = productId,
                Amount = model.Amount,
                Price = model.Price
            };
        }
    }
}
