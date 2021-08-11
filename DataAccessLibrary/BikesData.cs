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
            string sql = "SELECT * FROM bikesview";
            return _db.LoadData<BikeModel, dynamic>(sql, new { });
        }
    }
}
