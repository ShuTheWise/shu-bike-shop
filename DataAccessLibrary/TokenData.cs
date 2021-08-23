using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public class TokenData : ITokenData
    {
        private readonly ISqlDataAccess db;

        public TokenData(ISqlDataAccess db)
        {
            this.db = db;
        }

        public Task<List<string>> GetTokens(string username)
        {
            string sql = @"select token from tokens where username = @username order by token asc";
            return db.LoadData<string, dynamic>(sql, new { username });
        }

        public Task AddToken(string username, string token)
        {
            string sql = @"insert into tokens (username, token) values (@username, @token)";
            return db.SaveData<dynamic>(sql, new { token, username });
        }
    }
}
