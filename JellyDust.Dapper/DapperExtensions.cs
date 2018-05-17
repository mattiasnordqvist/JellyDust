﻿using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace JellyDust.Dapper
{
    public static class DapperExtensions
    {
        public static async Task<int> ExecuteAsync(this ITransaction @this, string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null) =>
            await @this.DbTransaction.Connection.ExecuteAsync(sql, param, @this.DbTransaction, commandTimeout, commandType);

        public static async Task<IEnumerable<T>> QueryAsync<T>(this ITransaction @this, string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null) =>
            await @this.DbTransaction.Connection.QueryAsync<T>(sql, param, @this.DbTransaction, commandTimeout, commandType);

        public static async Task<T> QueryFirstAsync<T>(this ITransaction @this, string sql, object param = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null) =>
            await @this.DbTransaction.Connection.QueryFirstAsync<T>(sql, param, @this.DbTransaction, commandTimeout, commandType);

        public static async Task<SqlMapper.GridReader> QueryMultipleAsync(this ITransaction @this, string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null) =>
            await @this.DbTransaction.Connection.QueryMultipleAsync(sql, param, @this.DbTransaction, commandTimeout, commandType);

        public static async Task<T> ExecuteScalarAsync<T>(this ITransaction @this, string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null) =>
            await @this.DbConnection.ExecuteScalarAsync<T>(sql, param, @this.DbTransaction, commandTimeout, commandType);

        public static async Task<int> ExecuteAsync(this IDbTransaction @this, string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null) =>
            await @this.Connection.ExecuteAsync(sql, param, @this, commandTimeout, commandType);

        public static async Task<T> ExecuteScalarAsync<T>(this IDbTransaction @this, string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null) =>
            await @this.Connection.ExecuteScalarAsync<T>(sql, param, @this, commandTimeout, commandType);

        public static async Task<IEnumerable<T>> QueryAsync<T>(this IDbTransaction @this, string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null) =>
            await @this.Connection.QueryAsync<T>(sql, param, @this, commandTimeout, commandType);

        public static async Task<T> QueryFirstAsync<T>(this IDbTransaction @this, string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null) =>
            await @this.Connection.QueryFirstAsync<T>(sql, param, @this, commandTimeout, commandType);

        public static async Task<int> ExecuteAsync(this IConnection @this, string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null) =>
            await @this.DbConnection.ExecuteAsync(sql, param, @this.GetCurrentDbTransaction(), commandTimeout, commandType);

        public static async Task<SqlMapper.GridReader> QueryMultipleAsync(this IConnection @this, string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null) =>
            await @this.DbConnection.QueryMultipleAsync(sql, param, @this.GetCurrentDbTransaction(), commandTimeout, commandType);

        public static async Task<IEnumerable<T>> QueryAsync<T>(this IConnection @this, string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null) =>
            await @this.DbConnection.QueryAsync<T>(sql, param, @this.GetCurrentDbTransaction(), commandTimeout, commandType);

        public static async Task<T> QueryFirstAsync<T>(this IConnection @this, string sql, object param = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null) =>
            await @this.DbConnection.QueryFirstAsync<T>(sql, param, @this.GetCurrentDbTransaction(), commandTimeout, commandType);
    }
}