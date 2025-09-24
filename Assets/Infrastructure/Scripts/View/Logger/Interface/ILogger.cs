using UnityEngine;


namespace Infrastructure.View.Logger.Interface
{
    public interface ILogger
    {
        void Log(object message);

        void Log(object message, Color color);

        void LogWarning(object message, Color color);

        void LogError(object message, Color color);
    }
}
