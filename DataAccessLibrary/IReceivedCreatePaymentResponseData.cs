using DataAccessLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public interface IReceivedCreatePaymentResponseData
    {
        Task<int> AddResponse(string json);
        Task<List<OrderModel>> GetResponses();
    }
}