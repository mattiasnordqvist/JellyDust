using System;
using System.Data;

namespace JellyDust
{
    public class Connection : IConnection
    {
        private readonly IDbConnectionFactory _connectionFactory;

        private readonly IDbTransactionFactory _transactionFactory;

        private IDbConnection _connection;

        private IDbTransaction _currentTransaction;

        public Connection(IDbConnectionFactory connectionFactory, IDbTransactionFactory transactionFactory)
        {
            _connectionFactory = connectionFactory;
            _transactionFactory = transactionFactory;
        }

        public IDbConnection DbConnection => _connection ?? 
            (_connection = _connectionFactory.OpenNew());

        public void Dispose()
        {
            if (_connection != null)
            {
                _connection.Dispose();
                _connection = null;
            }
        }

        public IDbTransaction GetCurrentDbTransaction()
        {
            return _currentTransaction;
        }

        public void SetCurrentDbTransaction(IDbTransaction databaseTransaction)
        {
            _currentTransaction = databaseTransaction;
        }

        public void RunInTransaction(Action<IDbTransaction> action)
        {
            using (var transaction = _transactionFactory.OpenTransaction(DbConnection))
            {
                action(transaction);
                transaction.Commit();
            }
        }

        public T RunInTransaction<T>(Func<IDbTransaction, T> action)
        {
            using (var transaction = _transactionFactory.OpenTransaction(DbConnection))
            {
                var result = action(transaction);
                transaction.Commit();
                return result;
            }
        }
    }
}