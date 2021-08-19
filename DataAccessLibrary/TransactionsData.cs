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
            string sql = @$"insert into transactions (amount, paymentproductid, username, cardholdername, orderid, errormessage, responsemessage, status) values (@Amount, @PaymentProductId, @Username, @CardholderName, @OrderId, @ErrorMessage, @ResponseMessage, @Status) returning id";

            int transactionId = await db.SaveData<TransactionCreateModel, int>(sql, model);

            return new TransactionModel()
            {
                Id = transactionId,
                Amount = model.Amount,
                PaymentProductId = model.PaymentProductId,
                OrderId = model.OrderId,
                Username = model.Username,
                CardholderName = model.CardholderName,
                ErrorMessage = model.ErrorMessage,
                ResponseMesssage = model.ResponseMessage
            };
        }

        public Task<List<TransactionModel>> GetTransactions()
        {
            string sql = "select * from transactions";
            return db.LoadData<TransactionModel, dynamic>(sql, new { });
        }

        public async Task UpdateTransaction(TransactionUpdateModel transactionModel)
        {
            string sql = @"update transactions set paymentMethod = @PaymentMethod where id = @Id";
            await db.SaveData(sql, transactionModel);
        }
    }
}
