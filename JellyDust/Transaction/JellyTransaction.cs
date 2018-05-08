using System.Data;

using JellyDust.Connection;

namespace JellyDust.Transaction
{
    public class JellyTransaction : IJellyTransaction
    {
        private readonly IDbTransactionFactory _databaseTransactionFactory;

        private readonly IJellyConnection _connectionSession;

        private IDbTransaction _databaseTransaction;

        public JellyTransaction(IDbTransactionFactory databaseTransactionFactory, IJellyConnection connectionSession)
        {
            _databaseTransactionFactory = databaseTransactionFactory;
            _connectionSession = connectionSession;
        }

        public IDbConnection Connection => _connectionSession.Connection;

        public IDbTransaction Transaction
        {
            get
            {
                if (_databaseTransaction == null)
                {
                    _databaseTransaction = _databaseTransactionFactory.OpenTransaction(Connection);
                    _connectionSession.SetCurrentTransaction(_databaseTransaction);
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
            _databaseTransaction?.Dispose();
            _connectionSession?.Dispose();
        }

        public void Commit()
        {
            _databaseTransaction?.Commit();
            _connectionSession.SetCurrentTransaction(null);
        }

        public void Rollback()
        {
            _databaseTransaction?.Rollback();
        }
    }
}