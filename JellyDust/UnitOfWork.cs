using System;


namespace JellyDust
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbTransactionFactory _transactionFactory;
        private readonly IDbConnectionFactory _connectionFactory;

        private IConnection _connection;
        private ITransaction _session;

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
                return _connection ?? (_connection = new Connection(_connectionFactory));
            }
        }

        public ITransaction Session
        {
            get
            {
                VerifyNotDisposed();
                return _session ?? (_session = new Transaction(_transactionFactory, Connection));
            }
        }

        public void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }
            
            if (HasSession())
            {
                _session.Dispose();
                _session = null;
            }

            _connection?.Dispose();
            IsDisposed = true;
        }

        public void Commit()
        {
            VerifyNotDisposed();

            if (HasSession())
            {
                _session.Commit();
            }

            Dispose();
        }

        public void Rollback()
        {
            VerifyNotDisposed();

            if (HasSession())
            {
                _session.Rollback();
            }

            Dispose();
        }

        private void VerifyNotDisposed()
        {
            if (IsDisposed)
            {
                throw new InvalidOperationException("The Transaction Unit of Work has been disposed and can no longer be used");
            }
        }
     
        private bool HasSession()
        {
            return _session != null;
        }
    }
}