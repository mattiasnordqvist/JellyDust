using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace JellyDust.Dapper
{
    public static class DapperGetExtensions
    {
        public static async Task<T> GetAsync<T>(this ITransaction @this, string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            var results = await @this.QueryAsync<T>(sql, param, commandTimeout, commandType);
            if (results.Count() == 0)
            {
                return default(T);
            }
            else
            {
                return results.Single();
            }
        }

        public static async Task<T> GetAsync<T>(this IConnection @this, string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null) {
            var results = await @this.QueryAsync<T>(sql, param, commandTimeout, commandType);
            if (results.Count() == 0)
            {
                return default(T);
            }
            else
            {
                return results.Single();
            }
        }
    }
}