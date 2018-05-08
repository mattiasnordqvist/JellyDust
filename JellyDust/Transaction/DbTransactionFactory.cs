using System.Data;

using JellyDust.Connection;

namespace JellyDust.Transaction
{
    public class DbTransactionFactory : IDbTransactionFactory
    {
        private readonly IDbIsolationLevelResolver _isolationLevelResolver;

        public DbTransactionFactory(IDbIsolationLevelResolver isolationLevelResolver)
        {
            _isolationLevelResolver = isolationLevelResolver;
        }

        public IDbTransaction OpenTransaction(IDbConnection connection)
        {
            return connection.BeginTransaction(_isolationLevelResolver.GetIsolationLevel());
        }
    }
}