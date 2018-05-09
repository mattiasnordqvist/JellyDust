using System;


namespace JellyDust
{
    public class TransactionUnitOfWork : ITransactionUnitOfWork
    {
        private readonly IDbTransactionFactory _transactionFactory;

        private readonly IConnectionUnitOfWork _connectionUnitOfWork;

        private ITransaction _session;

        public TransactionUnitOfWork(IDbTransactionFactory transactionFactory, IConnectionUnitOfWork connectionUnitOfWork)
        {
            _transactionFactory = transactionFactory;
            _connectionUnitOfWork = connectionUnitOfWork;
        }

        public bool IsDisposed { get; private set; }

        public ITransaction Session => _session ?? (_session = new Transaction(_transactionFactory, _connectionUnitOfWork.Session));
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

            _connectionUnitOfWork?.Dispose();
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