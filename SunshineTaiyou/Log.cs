using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunshineTaiyou
{
    internal enum LogLevel
    { 
        Warning = 0,
        Info = 1,
    }

    internal static class Log
    {
        public static int LogLevel = 0;

        // Errors are always logged, no matter what
        public static void Error(string message, bool WriteOnly = false)
        {
            Console.ForegroundColor = ConsoleColor.Red;

            Console.Write($"Error");

            Console.ResetColor();

            if (!WriteOnly)
            {
                Console.WriteLine($": {message}");
                
            }else
            {
                Console.Write($": {message}");
                
            }

        }

        public static void Warning(string message, bool WriteOnly = false)
        {
            if (LogLevel < (int)SunshineTaiyou.LogLevel.Warning) { return; }

            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.Write($"Warning");
            
            Console.ResetColor();

            if (!WriteOnly)
            {
                Console.WriteLine($": {message}");

            }
            else
            {
                Console.Write($": {message}");

            }
        }

        public static void Info(string message, bool WriteOnly = false)
        {
            if (LogLevel < (int)SunshineTaiyou.LogLevel.Info) { return; }

            if (!WriteOnly)
            {
                Console.WriteLine($"{message}");

            }else
            {
                Console.Write($"{message}");
            }
        }
    }
}
