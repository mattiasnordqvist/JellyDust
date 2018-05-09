using System.Data;


namespace JellyDust
{
    public class Transaction : ITransaction
    {
        private readonly IDbTransactionFactory _databaseTransactionFactory;

        private readonly IConnection _connectionSession;

        private IDbTransaction _databaseTransaction;

        public Transaction(IDbTransactionFactory databaseTransactionFactory, IConnection connectionSession)
        {
            _databaseTransactionFactory = databaseTransactionFactory;
            _connectionSession = connectionSession;
        }

        public IDbConnection DbConnection => _connectionSession.DbConnection;

        public IDbTransaction DbTransaction
        {
            get
            {
                if (_databaseTransaction == null)
                {
                    _databaseTransaction = _databaseTransactionFactory.OpenTransaction(DbConnection);
                    _connectionSession.SetCurrentDbTransaction(_databaseTransaction);
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
            _connectionSession.SetCurrentDbTransaction(null);
        }

        public void Rollback()
        {
            _databaseTransaction?.Rollback();
        }
    }
}