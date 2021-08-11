using DataAccessLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public class UserData : IUserData
    {
        private readonly ISqlDataAccess _db;

        public UserData(ISqlDataAccess db)
        {
            _db = db;
        }

        public Task<List<UserModel>> GetUsers()
        {
            string sql = @"select * from users";
            return _db.LoadData<UserModel, dynamic>(sql, new { });
        }

        public Task AddUser(UserModel userModel)
        {
            string sql = @$"insert into users values ('{userModel.Email}', '{userModel.EncryptedPassword}', {(int)userModel.Role})";
            return _db.SaveData(sql, userModel);
        }

        public Task<UserModel> AuthenticateUser(string email, string encryptedPassword)
        {
            string sql = $"select * from users where email = '{email}' and password = '{encryptedPassword}'";
            return _db.LoadSingle<UserModel, dynamic>(sql, new { });
        }
    }
}
