using System;
using System.Data;

namespace JellyDust
{
    public class Connection : IConnection
    {
        private readonly IDbConnectionFactory _connectionFactory;

        private IDbConnection _connection;

        private IDbTransaction _currentTransaction;

        public Connection(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public IDbConnection DbConnection
        {
            get
            {
                VerifyNotDisposed();
                return _connection ?? (_connection = _connectionFactory.OpenNew());
            }
        }

        public bool IsDisposed { get; private set; }

        public void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            if (HasConnection())
            {
                _connection.Dispose();
                _connection = null;
            }

            IsDisposed = true;
        }

        private void VerifyNotDisposed()
        {
            if (IsDisposed)
            {
                throw new InvalidOperationException("The Connection has been disposed and can no longer be used");
            }
        }

        public bool HasConnection()
        {
            return _connection != null;
        }

        public IDbTransaction GetCurrentDbTransaction()
        {
            return _currentTransaction;
        }

        public void SetCurrentDbTransaction(IDbTransaction databaseTransaction)
        {
            _currentTransaction = databaseTransaction;
        }
    }
}