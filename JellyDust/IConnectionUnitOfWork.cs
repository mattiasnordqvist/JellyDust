namespace JellyDust
{
    public interface IConnectionUnitOfWork
    {
        bool IsDisposed { get; }
        void Dispose();
        IConnection Session { get; }
    }
}