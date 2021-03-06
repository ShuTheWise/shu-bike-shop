using DataAccessLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public class ReceivedCreatePaymentResponseData : IReceivedCreatePaymentResponseData
    {
        private readonly ISqlDataAccess db;

        public ReceivedCreatePaymentResponseData(ISqlDataAccess db)
        {
            this.db = db;
        }

        public Task<List<OrderModel>> GetResponses()
        {
            string sql = @"select * from receivedcreatepaymentrespones order by id asc";
            return db.LoadData<OrderModel, dynamic>(sql, new { });
        }

        public Task<int> AddResponse(string json)
        {
            string sql = @$"insert into receivedcreatepaymentrespones (json) values (@Json) returning id";
            return db.SaveData<dynamic,int>(sql, new { json });
        }
    }
}
