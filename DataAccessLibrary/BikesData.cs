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

        public async Task<BikeModel> AddBike(NewBikeModel model)
        {
            var product = await productData.AddProduct(new NewProductModel { Price = model.Price, Amount = model.Amount });
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
            string sql = @" select b.id, p.price, b.model, b.make, p.amount, b.year from bikes b left join products p on b.id = p.id;";
            return db.LoadData<BikeModel, dynamic>(sql, new { });
        }
    }
}
