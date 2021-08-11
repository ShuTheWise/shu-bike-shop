using DataAccessLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public class BikesData : IBikesData
    {
        private readonly ISqlDataAccess _db;

        public BikesData(ISqlDataAccess db)
        {
            _db = db;
        }

        public Task<List<BikeModel>> GetBikes()
        {
            string sql = @" select b.id, p.price, b.model, b.make, p.amount, b.year from bikes b left join products p on b.id = p.id;";
            return _db.LoadData<BikeModel, dynamic>(sql, new { });
        }
    }
}
