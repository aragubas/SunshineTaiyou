using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunshineTaiyou
{
    public enum LogLevel
    { 
        Warning = 0,
        Info = 1,
    }

    public static class Log
    {
        public static int LogLevel = 0;

        // Errors are always logged, no matter what
        public static void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;

            Console.Write($"Error");

            Console.ResetColor();

            Console.Write($": {message}{Environment.NewLine}");
        }

        public static void Warning(string message)
        {
            if (LogLevel < (int)SunshineTaiyou.LogLevel.Warning) { return; }

            Console.ForegroundColor = ConsoleColor.DarkYellow;

            Console.Write($"Warning");
            Console.ResetColor();

            Console.Write($": {message}{Environment.NewLine}");
        }

        public static void Info(string message)
        {
            if (LogLevel < (int)SunshineTaiyou.LogLevel.Info) { return; }

            Console.WriteLine($"Info: {message}");
        }
    }
}
