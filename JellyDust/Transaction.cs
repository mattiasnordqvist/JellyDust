using System;
using System.Data;


namespace JellyDust
{
    public class Transaction : ITransaction
    {
        private readonly IDbTransactionFactory _databaseTransactionFactory;

        private readonly IConnection _connection;

        private IDbTransaction _databaseTransaction;

        public Transaction(IDbTransactionFactory databaseTransactionFactory, IConnection connection)
        {
            _databaseTransactionFactory = databaseTransactionFactory;
            _connection = connection;
        }

        public IDbConnection DbConnection => _connection.DbConnection;

        public IDbTransaction DbTransaction
        {
            get
            {
                VerifyNotDisposed();
                if (_databaseTransaction == null)
                {
                    _databaseTransaction = _databaseTransactionFactory.OpenTransaction(DbConnection);
                    _connection.SetCurrentDbTransaction(_databaseTransaction);
                }

                return _databaseTransaction;
            }
        }

        public bool HasTransaction()
        {
            return _databaseTransaction != null;
        }

        public void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            _connection?.SetCurrentDbTransaction(null);
            _databaseTransaction?.Dispose();

            IsDisposed = true;
        }

        public void Commit()
        {
            _databaseTransaction?.Commit();
            _connection.SetCurrentDbTransaction(null);
        }

        public void Rollback()
        {
            _databaseTransaction?.Rollback();
        }

        public void VerifyNotDisposed()
        {
            if (IsDisposed)
            {
                throw new InvalidOperationException("The Transaction has been disposed and can no longer be used");
            }
        }

        public bool IsDisposed { get; private set; }
    }
}