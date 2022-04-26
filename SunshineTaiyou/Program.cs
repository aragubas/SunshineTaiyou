/*
    Copyright 2022 Aragubas

    Licensed under the Apache License, Version 2.0 (the "License");
    you may not use this file except in compliance with the License.
    You may obtain a copy of the License at

        http://www.apache.org/licenses/LICENSE-2.0

    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS,
    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    See the License for the specific language governing permissions and
    limitations under the License.
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace SunshineTaiyou
{
    class Program
    {
        static readonly int[] Version = new int[] { 1, 0, 0 };
        static readonly string BuildChannel = "dev";

        static void PrintLogo()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("TaiyouScript Compiler (tysc) ");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"v{Version[0]}.{Version[1]}.{Version[2]}-{BuildChannel} ");
            Console.ResetColor();
            Console.Write($"on {Environment.OSVersion.Platform}\n");
        }

        // Main entry point
        static int Main(string[] args)
        {
            bool NoLogo = false;
            bool CustomSourceFolder_Latch = false;
            string CustomSourceFolder = "";
            bool CustomLogLevel_Latch = false;
            int LogLevel = -1;

            foreach (string arg in args)
            {
                // Next argument should be specified custom source folder
                if (CustomSourceFolder_Latch)
                {
                    CustomSourceFolder = arg;
                    CustomSourceFolder_Latch = false;
                    continue;
                }

                if (CustomLogLevel_Latch)
                {
                    try
                    {
                        LogLevel = int.Parse(arg);
                        CustomLogLevel_Latch = false;
                    }
                    catch
                    {
                        Log.Error("Invalid log level specified.");
                        return -1;
                    }
                }

                if (arg == "-nologo")
                {
                    NoLogo = true;
                }

                if (arg == "-source")
                {
                    CustomSourceFolder_Latch = true;
                }

                if (arg == "-log")
                {
                    CustomLogLevel_Latch = true;
                }

            }

            if (!NoLogo) { PrintLogo(); }
            Log.LogLevel = LogLevel;

            //
            // Reads and parses the main assembly
            //
            TaiyouAssembly mainAssembly = new TaiyouAssembly("./program/main.tiy");

            return 0;
        }
    }
}