using DataAccessLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public interface ITransactionsData
    {
        Task<TransactionModel> AddTransaction(TransactionCreateModel model);
        Task<List<TransactionModel>> GetTransactions();
    }
}