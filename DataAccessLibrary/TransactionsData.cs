using DataAccessLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public class TransactionsData : ITransactionsData
    {
        private readonly ISqlDataAccess db;

        public TransactionsData(ISqlDataAccess db)
        {
            this.db = db;
        }

        public async Task<TransactionModel> AddTransaction(TransactionCreateModel model)
        {
            string sql = @$"insert into transactions (amount, paymentmethod, username, orderid) values (@Amount, @PaymentMethod, @Username, @OrderId) returning id";

            int transactionId = await db.SaveData<TransactionCreateModel, int>(sql, model);

            return new TransactionModel()
            {
                Id = transactionId,
                Amount = model.Amount,
                PaymentMethod = model.PaymentMethod,
                OrderId = model.OrderId,
                Username = model.Username
            };
        }

        public Task<List<TransactionModel>> GetTransactions()
        {
            string sql = "select * from transactions";
            return db.LoadData<TransactionModel, dynamic>(sql, new { });
        }
    }
}
