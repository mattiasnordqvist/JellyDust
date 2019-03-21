using System;


namespace JellyDust
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbTransactionFactory _transactionFactory;
        private readonly IDbConnectionFactory _connectionFactory;

        private IConnection _connection;
        private ITransaction _transaction;

        public UnitOfWork(IDbTransactionFactory transactionFactory, IDbConnectionFactory connectionFactory)
        {
            _transactionFactory = transactionFactory;
            _connectionFactory = connectionFactory;
        }

        public bool IsDisposed { get; private set; }

        public IConnection Connection
        {
            get
            {
                VerifyNotDisposed();
                if (_connection != null)
                {
                    _connection.VerifyNotDisposed();
                }

                return _connection ?? (_connection = new Connection(_connectionFactory));
            }
        }

        public ITransaction Transaction
        {
            get
            {
                VerifyNotDisposed();
                _transaction?.VerifyNotDisposed();
                return _transaction ?? (_transaction = new Transaction(_transactionFactory, Connection));
            }
        }

        public void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            _transaction?.Dispose();
            _transaction = null;
            _connection?.Dispose();
            IsDisposed = true;
        }

        public void Commit()
        {
            VerifyNotDisposed();
            _transaction?.Commit();
        }

        public void Rollback()
        {
            VerifyNotDisposed();
            _transaction?.Rollback();
        }

        private void VerifyNotDisposed()
        {
            if (IsDisposed)
            {
                throw new InvalidOperationException("The Unit of Work has been disposed and can no longer be used");
            }
        }

        public bool HasTransaction()
        {
            return _transaction != null;
        }

        public bool HasConnection()
        {
            return _connection != null;
        }
    }
}