using System;
using System.Data;

using JellyDust.Transaction;

namespace JellyDust.Connection
{
    public class JellyConnection : IJellyConnection
    {
        private readonly IDbConnectionFactory _connectionFactory;

        private readonly IDbTransactionFactory _transactionFactory;

        private IDbConnection _connection;

        private IDbTransaction _currentTransaction;

        public JellyConnection(IDbConnectionFactory connectionFactory, IDbTransactionFactory transactionFactory)
        {
            _connectionFactory = connectionFactory;
            _transactionFactory = transactionFactory;
        }

        public IDbConnection Connection => _connection ?? 
            (_connection = _connectionFactory.OpenNew());

        public void Dispose()
        {
            if (_connection != null)
            {
                _connection.Dispose();
                _connection = null;
            }
        }

        public IDbTransaction GetCurrentTransaction()
        {
            return _currentTransaction;
        }

        public void SetCurrentTransaction(IDbTransaction databaseTransaction)
        {
            _currentTransaction = databaseTransaction;
        }

        public void RunInTransaction(Action<IDbTransaction> action)
        {
            using (var transaction = _transactionFactory.OpenTransaction(Connection))
            {
                action(transaction);
                transaction.Commit();
            }
        }

        public T RunInTransaction<T>(Func<IDbTransaction, T> action)
        {
            using (var transaction = _transactionFactory.OpenTransaction(Connection))
            {
                var result = action(transaction);
                transaction.Commit();
                return result;
            }
        }
    }
}