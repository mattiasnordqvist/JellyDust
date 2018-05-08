using System.Data;

namespace JellyDust.Connection
{
    public interface IDbConnectionFactory
    {
        IDbConnection OpenNew();
    }
}