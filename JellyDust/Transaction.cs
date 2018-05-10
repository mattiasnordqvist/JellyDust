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
            _databaseTransaction?.Dispose();
            _connection?.Dispose();
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
    }
}