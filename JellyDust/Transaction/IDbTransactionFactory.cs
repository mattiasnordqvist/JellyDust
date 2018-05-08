using System.Data;

namespace JellyDust.Transaction
{
    public interface IDbTransactionFactory
    {
        IDbTransaction OpenTransaction(IDbConnection connection);
    }
}