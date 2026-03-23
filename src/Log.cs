using System;

namespace LimitTeamChange;

public static class Log
{
    public static void Info(string message)
    {
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine($"[LimitTeamChange] {message}");
        Console.ResetColor();
    }

    public static void Error(string message)
    {
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine($"[LimitTeamChange] {message}");
        Console.ResetColor();
    }
}
