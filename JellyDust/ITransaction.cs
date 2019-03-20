using System;
using System.Data;

namespace JellyDust
{
    public interface ITransaction  : IDisposable
    {
        IDbTransaction DbTransaction { get; }
        IDbConnection DbConnection { get; }
        void Commit();
        void Rollback();
        void VerifyNotDisposed();
    }
}