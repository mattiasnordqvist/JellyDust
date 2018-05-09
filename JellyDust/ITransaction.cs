using System.Data;

namespace JellyDust
{
    public interface ITransaction 
    {
        IDbTransaction DbTransaction { get; }
        IDbConnection DbConnection { get; }

        void Dispose();

        void Commit();

        void Rollback();
    }
}