namespace JellyDust.Connection
{
    public interface IJellyConnectionUnitOfWork
    {
        bool IsDisposed { get; }
        void Dispose();
        IJellyConnection Session { get; }
    }
}