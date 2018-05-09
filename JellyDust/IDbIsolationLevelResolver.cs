using System.Data;

namespace JellyDust
{
    public interface IDbIsolationLevelResolver
    {
        IsolationLevel GetIsolationLevel();
    }
}