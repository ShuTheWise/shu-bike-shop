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
            string sql = "SELECT * FROM products";
            return db.LoadData<ProductModel, dynamic>(sql, new { });
        }

        public Task DecrementProductAmount(int id, int amount)
        {
            string sql = "update products set amount = amount - @amount where id = @Id";
            return db.SaveData(sql, new { id, amount });
        }

        public async Task<ProductModel> AddProduct(ProductCreateModel model)
        {
            string sql = @$"insert into products (price, amount) values (@Price, @Amount) returning id";

            int productId = await db.SaveData<ProductCreateModel, int>(sql, model);

            return new ProductModel()
            {
                Id = productId,
                Amount = model.Amount,
                Price = model.Price
            };
        }

        public async Task<ProductModel> UpdateProduct(ProductUpdateModel model)
        {
            string sql = @"update products set amount = @Amount, price = @Price where id = @id";

            await db.SaveData(sql, model);

            var product = new ProductModel()
            {
                Id = model.Id,
                Amount = model.Amount,
                Price = model.Price,
            };

            return product;
        }

        public Task RemoveProduct(int id)
        {
            string sql = "delete into products where id = @id";
            return db.SaveData(sql, id);
        }
    }
}
