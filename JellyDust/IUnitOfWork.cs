using System;

namespace JellyDust
{
    public interface IUnitOfWork : IDisposable
    {
        bool IsDisposed { get; }
        void Commit();
        void Rollback();
        ITransaction Transaction { get; }
        IConnection Connection { get; }
    }
}