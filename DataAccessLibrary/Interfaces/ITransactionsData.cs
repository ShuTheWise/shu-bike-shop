using DataAccessLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public interface ITransactionsData
    {
        Task<TransactionModel> AddTransaction(TransactionCreateModel model);
        Task<TransactionModel> GetTransactionByPaymentId(long paymentId);
        Task<List<TransactionModel>> GetTransactions();
        Task UpdateTransactionByPaymentId(long paymentId, dynamic updateModel);
    }
}