using DataAccessLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public class UserData : IUserData
    {
        private readonly ISqlDataAccess db;
        
        public UserData(ISqlDataAccess db)
        {
            this.db = db;
        }

        public Task<List<UserModel>> GetUsers()
        {
            string sql = @"select * from users";
            return db.LoadData<UserModel, dynamic>(sql, new { });
        }

        public Task AddUser(UserModel userModel)
        {
            string sql = @$"insert into users values ('{userModel.Email}', '{userModel.EncryptedPassword}', {(int)userModel.Role})";
            return db.SaveData(sql, userModel);
        }

        public Task<UserModel> AuthenticateUser(string email, string encryptedPassword)
        {
            string sql = $"select * from users where email = '{email}' and password = '{encryptedPassword}'";
            return db.LoadSingle<UserModel, dynamic>(sql, new { });
        }
    }
}
