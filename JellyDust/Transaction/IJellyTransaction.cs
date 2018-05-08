using System.Data;

namespace JellyDust.Transaction
{
    public interface IJellyTransaction 
    {
        IDbTransaction Transaction { get; }
        IDbConnection Connection { get; }

        void Dispose();

        void Commit();

        void Rollback();
    }
}