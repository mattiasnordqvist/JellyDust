using System;


namespace JellyDust
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbTransactionFactory _transactionFactory;

        private readonly IConnection _connection;

        private ITransaction _session;

        public UnitOfWork(IDbTransactionFactory transactionFactory, IConnection connection)
        {
            _transactionFactory = transactionFactory;
            _connection = connection;
        }

        public bool IsDisposed { get; private set; }

        public ITransaction Session
        {
            get
            {
                VerifyNotDisposed();
                return _session ?? (_session = new Transaction(_transactionFactory, _connection));
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