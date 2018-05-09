namespace JellyDust
{
    public interface ITransactionUnitOfWork
    {
        bool IsDisposed { get; }
        void Dispose();
        void Commit();
        void Rollback();
        ITransaction Session { get; }
    }
}