using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunshineTaiyou.Log
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
        public static void Error(ref string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;

            Console.Write($"Error");

            Console.ResetColor();

            Console.Write($": {message}{Environment.NewLine}");
        }

        public static void Warning(ref string message)
        {
            if (LogLevel < (int)SunshineTaiyou.Log.LogLevel.Warning) { return; }

            Console.ForegroundColor = ConsoleColor.DarkYellow;

            Console.Write($"Warning");
            Console.ResetColor();

            Console.Write($": {message}{Environment.NewLine}");
        }

        public static void Info(ref string message)
        {
            if (LogLevel < (int)SunshineTaiyou.Log.LogLevel.Info) { return; }

            Console.WriteLine($"Info: {message}");
        }
    }
}
