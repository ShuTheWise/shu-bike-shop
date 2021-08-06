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
            string sql = @"select b.""Id"", ""Price"", ""Model"", ""Make"" from public.""Bikes"" b left join public.""Products"" p on b.""Id"" = p.""Id""";
            var x = _db.LoadData<BikeModel, dynamic>(sql, new { });

            //var x =_db.LoadData<>
            //return _db.LoadData<BikeModel, dynamic>(sql, new { });

            return x;
        }
    }
}
