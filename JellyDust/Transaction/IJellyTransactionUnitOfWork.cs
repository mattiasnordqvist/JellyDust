namespace JellyDust.Transaction
{
    public interface IJellyTransactionUnitOfWork
    {
        bool IsDisposed { get; }
        void Dispose();
        void Commit();
        void Rollback();
        IJellyTransaction Session { get; }
    }
}