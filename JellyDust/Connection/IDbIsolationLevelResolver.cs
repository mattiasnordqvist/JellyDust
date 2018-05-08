using System.Data;

namespace JellyDust.Connection
{
    public interface IDbIsolationLevelResolver
    {
        IsolationLevel GetIsolationLevel();
    }
}