using System;
using System.Data;

namespace JellyDust.Connection
{
    public abstract class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly IDbConnectionStringResolver _connectionStringResolver;

        public DbConnectionFactory(IDbConnectionStringResolver connectionStringResolver)
        {
            _connectionStringResolver = connectionStringResolver;
        }

        public IDbConnection OpenNew()
        {
            var connection = CreateConnection();
            connection.ConnectionString = _connectionStringResolver.GetConnectionString();
            connection.Open();
            return connection;
        }

        protected abstract IDbConnection CreateConnection();
    }
}