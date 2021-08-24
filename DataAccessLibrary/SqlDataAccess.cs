﻿using System;
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
        private readonly IConfiguration config;

        public string ConnectionStringName { get; set; } = "DefaultConnectionString";

        public SqlDataAccess(IConfiguration config)
        {
            this.config = config;
        }

        public async Task<List<T>> LoadData<T, U>(string sql, U parameters)
        {
            var connectionString = config[ConnectionStringName];
            using (IDbConnection connection = new NpgsqlConnection(connectionString))
            {
                var data = await connection.QueryAsync<T>(sql, parameters);
                return data.ToList();
            }
        }

        public async Task<List<T>> LoadData<T,U>(Type t, string sql, U parameters) where T : class
        {
            var connectionString = config[ConnectionStringName];
            using (IDbConnection connection = new NpgsqlConnection(connectionString))
            {
                var data = await connection.QueryAsync(t, sql, parameters);
                return data.Cast<T>().ToList();
            }
        }

        public async Task SaveData<T>(string sql, T parameters)
        {
            var connectionString = config[ConnectionStringName];
            using (IDbConnection connection = new NpgsqlConnection(connectionString))
            {
                await connection.ExecuteAsync(sql, parameters);
            }
        }

        public async Task<U> SaveData<T, U>(string sql, T parameters)
        {
            var connectionString = config[ConnectionStringName];
            using (IDbConnection connection = new NpgsqlConnection(connectionString))
            {
                var ret = await connection.QuerySingleOrDefaultAsync<U>(sql, parameters);
                return ret;
            }
        }

        public async Task<T> LoadSingle<T, U>(string sql, U parameters)
        {
            var connectionString = config[ConnectionStringName];
            using (IDbConnection connection = new NpgsqlConnection(connectionString))
            {
                var data = await connection.QuerySingleAsync<T>(sql, parameters);
                return data;
            }
        }

        public async Task<T> LoadSingleOrDefault<T, U>(string sql, U parameters)
        {
            var connectionString = config[ConnectionStringName];
            using (IDbConnection connection = new NpgsqlConnection(connectionString))
            {
                var data = await connection.QuerySingleOrDefaultAsync<T>(sql, parameters);
                return data;
            }
        }

        public async Task<T> LoadSingleOrDefault<T, U>(Type t, string sql, U parameters) where T : class
        {
            var connectionString = config[ConnectionStringName];
            using (IDbConnection connection = new NpgsqlConnection(connectionString))
            {
                var data = await connection.QuerySingleOrDefaultAsync(t, sql, parameters);
                return data as T;
            }
        }
    }
}
