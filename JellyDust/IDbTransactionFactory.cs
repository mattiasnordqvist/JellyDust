using System.Data;

namespace JellyDust
{
    public interface IDbTransactionFactory
    {
        IDbTransaction OpenTransaction(IDbConnection connection);
    }
}