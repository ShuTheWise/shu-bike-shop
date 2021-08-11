using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace DataAccessLibrary
{
    public class SqlDataAccess : ISqlDataAccess
    {
        private readonly IConfiguration _config;

        public string ConnectionStringName { get; set; } = "DefaultConnectionString";

        public SqlDataAccess(IConfiguration config)
        {
            var databaseURL = config["DATABASE_URL"];

            _config = config;
            _config[ConnectionStringName] = databaseURL;

            var uri = new Uri(databaseURL);
            var username = uri.UserInfo.Split(':')[0];
            var password = uri.UserInfo.Split(':')[1];

            var connectionString =
             "; Database=" + uri.AbsolutePath.Substring(1) +
             "; Username=" + username +
             "; Password=" + password +
             "; Port=" + uri.Port +
             "; SSL Mode=Require; Trust Server Certificate=true;";

            var builder = new NpgsqlConnectionStringBuilder(connectionString) { Host = uri.Host };

            config[ConnectionStringName] = builder.ToString();
        }

        public async Task<List<T>> LoadData<T, U>(string sql, U parameters)
        {
            var connectionString = _config[ConnectionStringName];
            using (IDbConnection connection = new NpgsqlConnection(connectionString))
            {
                var data = await connection.QueryAsync<T>(sql, parameters);
                return data.ToList();
            }
        }

        public async Task SaveData<T>(string sql, T parameters)
        {
            var connectionString = _config[ConnectionStringName];
            using (IDbConnection connection = new NpgsqlConnection(connectionString))
            {
                await connection.ExecuteAsync(sql, parameters);
            }
        }

        public async Task<U> SaveData<T, U>(string sql, T parameters)
        {
            var connectionString = _config[ConnectionStringName];
            using (IDbConnection connection = new NpgsqlConnection(connectionString))
            {
                var ret = await connection.QuerySingleOrDefaultAsync<U>(sql, parameters);
                return ret;
            }
        }

        public async Task<T> LoadSingle<T, U>(string sql, U parameters)
        {
            var connectionString = _config[ConnectionStringName];
            using (IDbConnection connection = new NpgsqlConnection(connectionString))
            {
                var data = await connection.QuerySingleAsync<T>(sql, parameters);
                return data;
            }
        }
    }
}
