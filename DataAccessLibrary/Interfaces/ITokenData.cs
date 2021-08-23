using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public interface ITokenData
    {
        Task AddToken(string username, string token);
        Task<List<string>> GetTokens(string username);
    }
}