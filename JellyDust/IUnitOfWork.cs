namespace JellyDust
{
    public interface IUnitOfWork
    {
        bool IsDisposed { get; }
        void Dispose();
        void Commit();
        void Rollback();
        ITransaction Session { get; }
        IConnection Connection { get; }
    }
}