using System.Diagnostics;

namespace Core.Utils;

public static class Logger
{
    public static void Log(string msg)
    {
        Debug.WriteLine($"[{DateTime.Now:HH:mm:ss}] {msg}");
    }
}
