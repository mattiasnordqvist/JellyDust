using System.Data;

namespace JellyDust.Connection
{
    public interface IJellyConnection
    {
        IDbConnection Connection { get; }

        void Dispose();

        void SetCurrentTransaction(IDbTransaction databaseTransaction);

        IDbTransaction GetCurrentTransaction();
    }
}
