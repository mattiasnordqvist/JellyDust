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
                return _connection ?? (_connection = _connectionFactory.CreateOpenConnection());
            }
        }

        public bool IsDisposed { get; private set; }

        public void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            _connection?.Dispose();
            _connection = null;

            IsDisposed = true;
        }

        public void VerifyNotDisposed()
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
            VerifyNotDisposed();
            return _currentTransaction;
        }

        public void SetCurrentDbTransaction(IDbTransaction databaseTransaction)
        {
            if (databaseTransaction == null)
            {
                _currentTransaction = null;
            }
            else
            {
                VerifyNotDisposed();
                _currentTransaction = databaseTransaction;
            }
        }
    }
}