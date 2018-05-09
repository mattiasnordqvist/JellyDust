
namespace JellyDust
{
    public class ConnectionUnitOfWork : IConnectionUnitOfWork
    {
        private readonly IDbConnectionFactory _connectionFactory;

        private readonly IDbTransactionFactory _transactionFactory;

        private IConnection _connectionSession;

        public ConnectionUnitOfWork(IDbConnectionFactory connectionFactory, IDbTransactionFactory transactionFactory)
        {
            _connectionFactory = connectionFactory;
            _transactionFactory = transactionFactory;
        }

        public IConnection Session => _connectionSession ?? (_connectionSession = new Connection(_connectionFactory, _transactionFactory));

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