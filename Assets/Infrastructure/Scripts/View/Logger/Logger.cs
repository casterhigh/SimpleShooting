using UnityEngine;
using ILogger = Infrastructure.View.Logger.Interface.ILogger;

public class Logger : ILogger
{
    public void Log(object message)
    {
        Log(message, Color.white);
    }

    public void Log(object message, Color color)
    {
        UnityEngine.Debug.Log(Colorize(message, color));
    }

    public void LogWarning(object message, Color color)
    {
        UnityEngine.Debug.LogWarning(Colorize(message, color));
    }

    public void LogError(object message, Color color)
    {
        UnityEngine.Debug.LogError(Colorize(message, color));
    }

    string Colorize(object message, Color color)
    {
        if (color == default)
        {
            color = Color.white;
        }

        string hexColor = ColorUtility.ToHtmlStringRGB(color);
        return $"<color=#{hexColor}>{message}</color>";
    }
}
