using System.Data;

namespace JellyDust
{
    public interface IConnection
    {
        IDbConnection DbConnection { get; }

        void Dispose();

        void SetCurrentDbTransaction(IDbTransaction databaseTransaction);

        IDbTransaction GetCurrentDbTransaction();
        void VerifyNotDisposed();
    }
}
