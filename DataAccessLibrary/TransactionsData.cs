using DataAccessLibrary.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public class TransactionsData : ITransactionsData
    {
        private readonly ISqlDataAccess db;
        private readonly IOrderData orderData;

        public TransactionsData(ISqlDataAccess db, IOrderData orderData)
        {
            this.db = db;
            this.orderData = orderData;
        }

        public async Task<TransactionModel> AddTransaction(TransactionCreateModel model)
        {
            string sql = "insert into transactions (amount, paymentproductid, username, cardholdername, orderid, errormessage, responsemessage, status, paymentid) values (@Amount, @PaymentProductId, @Username, @CardholderName, @OrderId, @ErrorMessage, @ResponseMessage, @Status, @PaymentId) returning id";

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
                ResponseMessage = model.ResponseMessage
            };
        }

        public Task<List<TransactionModel>> GetTransactions()
        {
            string sql = "select * from transactions";
            return db.LoadData<TransactionModel, dynamic>(sql, new { });
        }

        public Task<TransactionModel> GetTransactionByPaymentId(long paymentId)
        {
            string sql = "select * from transactions where paymentid = @paymentId";
            return db.LoadSingleOrDefault<TransactionModel, dynamic>(sql, new { paymentId });
        }

        public Task UpdateTransactionByPaymentId(long paymentId, dynamic updateModel)
        {
            object o = updateModel;
            var set = string.Join(", ", o.GetType().GetProperties().Select(p => $"{p.Name} = @{p.Name}"));
            string sql = $"update transactions set {set} where paymentid = '{paymentId}'";
            return db.SaveData(sql, updateModel);
        }
    }
}
