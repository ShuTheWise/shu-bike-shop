using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public interface ISqlDataAccess
    {
        string ConnectionStringName { get; set; }

        Task<List<T>> LoadData<T, U>(string sql, U parameters);
        Task<List<T>> LoadData<T, U>(Type t, string sql, U parameters) where T : class;
        Task<T> LoadSingle<T, U>(string sql, U parameters);
        Task<T> LoadSingleOrDefault<T, U>(string sql, U parameters);
        Task<T> LoadSingleOrDefault<T,U>(Type t, string sql, U parameters) where T : class;
        Task SaveData<T>(string sql, T parameters);
        Task<U> SaveData<T, U>(string sql, T parameters);
    }
}