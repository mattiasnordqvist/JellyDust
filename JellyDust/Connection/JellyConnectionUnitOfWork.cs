using JellyDust.Transaction;

namespace JellyDust.Connection
{
    public class JellyConnectionUnitOfWork : IJellyConnectionUnitOfWork
    {
        private readonly IDbConnectionFactory _connectionFactory;

        private readonly IDbTransactionFactory _transactionFactory;

        private IJellyConnection _connectionSession;

        public JellyConnectionUnitOfWork(IDbConnectionFactory connectionFactory, IDbTransactionFactory transactionFactory)
        {
            _connectionFactory = connectionFactory;
            _transactionFactory = transactionFactory;
        }

        public IJellyConnection Session => _connectionSession ?? (_connectionSession = new JellyConnection(_connectionFactory, _transactionFactory));

        public bool IsDisposed { get; private set; }

        public void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            if (HasSession())
            {
                _connectionSession.Dispose();
                _connectionSession = null;
            }

            IsDisposed = true;
        }

        private bool HasSession()
        {
            return _connectionSession != null;
        }
    }
}