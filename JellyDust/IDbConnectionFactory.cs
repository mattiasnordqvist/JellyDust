using System.Data;

namespace JellyDust
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateOpenConnection();
    }
}