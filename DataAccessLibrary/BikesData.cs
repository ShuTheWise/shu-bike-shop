using DataAccessLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public class BikesData : IBikesData
    {
        private readonly ISqlDataAccess db;
        private readonly IProductData productData;

        public BikesData(ISqlDataAccess db, IProductData productData)
        {
            this.db = db;
            this.productData = productData;
        }

        public async Task<BikeModel> AddBike(BikeCreateModel model)
        {
            var product = await productData.AddProduct(new ProductCreateModel { Price = model.Price, Amount = model.Amount });
            string sql = @"insert into bikes (id, make, model, year) values (@Id, @Make, @Model, @Year)";

            var bike = new BikeModel
            {
                Id = product.Id,
                Make = model.Make,
                Model = model.Model,
                Year = model.Year,
                Price = model.Price,
                Amount = model.Amount
            };

            await db.SaveData<BikeModel, dynamic>(sql, bike);
            return bike;
        }

        public Task<List<BikeModel>> GetBikes()
        {
            string sql = @"select b.id, p.price, b.model, b.make, p.amount, b.year from bikes b left join products p on b.id = p.id order by b.id;";
            return db.LoadData<BikeModel, dynamic>(sql, new { });
        }

        public async Task RemoveBike(int id)
        {
            string sql = "delete into bikes where id = @id";
            await productData.RemoveProduct(id);
            await db.SaveData(sql, id);
        }

        public async Task<BikeModel> UpdateBike(BikeUpdateModel model)
        {
            string sql = @"update bikes set make = @Make, model = @Model, year = @Year where id = @Id";

            await db.SaveData(sql, model);

            var product = new ProductUpdateModel()
            {
                Id = model.Id,
                Amount = model.Amount,
                Price = model.Price,
            };

            await productData.UpdateProduct(product);

            var bike = new BikeModel
            {
                Id = model.Id,
                Make = model.Make,
                Model = model.Model,
                Year = model.Year,
                Price = model.Price,
                Amount = model.Amount
            };

            return bike;
        }
    }
}
