using DataAccessLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public interface ITransactionsData
    {
        Task<TransactionModel> AddTransaction(TransactionCreateModel model);
        Task<TransactionModel> GetTransactionByPaymentId(int paymentId);
        Task<List<TransactionModel>> GetTransactions();
        Task UpdateTransactionByPaymentId(int paymentId, dynamic updateModel);
    }
}