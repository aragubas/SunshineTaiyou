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
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using SunshineTaiyou.Exceptions;

namespace SunshineTaiyou
{
    class Program
    {
        static readonly int[] Version = new int[] { 1, 0, 0 };
        static readonly string BuildChannel = "dev";

        static void PrintLogo()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"TaiyouScript Compiler (tysc) v{Version[0]}.{Version[1]}.{Version[2]}-{BuildChannel}");
            Console.ResetColor();
        }

        // Main entry point
        static int Main(string[] args)
        {
            bool NoLogo = false;
            bool CustomSourceFolder_Latch = false;
            bool CustomLogLevel_Latch = false;
            int LogLevel = 1;
            bool CustomOutputFolder_Latch = false;
            string SourceFolder = "./program/";
            string OutputFolder = "./output/";

            foreach (string arg in args)
            {
                // Next argument should be specified custom source folder
                if (CustomSourceFolder_Latch)
                {
                    SourceFolder = arg;
                    CustomSourceFolder_Latch = false;
                    continue;
                }
                
                if (CustomOutputFolder_Latch)
                {
                    OutputFolder = arg;
                    CustomOutputFolder_Latch = false;

                    continue;
                }

                if (CustomLogLevel_Latch)
                {
                    try
                    {
                        LogLevel = int.Parse(arg);
                        CustomLogLevel_Latch = false;
                        continue;
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

                if (arg == "-output")
                {
                    CustomOutputFolder_Latch = true;
                }

                if (arg == "-log")
                {
                    CustomLogLevel_Latch = true;
                }

            }

            if (!NoLogo) { PrintLogo(); }
            Log.LogLevel = LogLevel;

            // Check if source directory exists
            if (!Directory.Exists(SourceFolder))
            {
                throw new TaiyouException("Could not find source directory.");
            }

            string[] sourceFiles = Directory.GetFileSystemEntries(SourceFolder, "*.tiy", SearchOption.AllDirectories);

            if (sourceFiles.Length == 0)
            {
                Log.Warning("No source files found.");
                return 0;

            }
            else
            {
                // Make sure the output directory exists
                Directory.CreateDirectory(OutputFolder);
            }

            List<TaiyouAssembly> assemblies = new List<TaiyouAssembly>();
            TaiyouProject taiyouProject = new TaiyouProject();
            
            foreach (string sourceFileEntry in sourceFiles)
            {
                string sourceFile = sourceFileEntry.Replace("\\", "/");

                Stopwatch stopwatch = new Stopwatch();
                if (Log.LogLevel >= (int)SunshineTaiyou.LogLevel.Info)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"Compiling '{sourceFile}'...");
                    Console.ResetColor();
                    stopwatch.Start();
                }
                
                // Compiles the source file
                TaiyouAssembly assembly = new TaiyouAssembly(sourceFile, Path.GetFullPath(SourceFolder), ref taiyouProject);

                assemblies.Add(assembly);

                if (Log.LogLevel >= (int)SunshineTaiyou.LogLevel.Info)
                {
                    string ElapsedTimeString = "";

                    if (stopwatch.ElapsedMilliseconds > 1000) { ElapsedTimeString = $"{stopwatch.Elapsed.TotalSeconds}s."; }
                    else { ElapsedTimeString = $"{stopwatch.ElapsedMilliseconds}ms."; }

                    Console.ForegroundColor = ConsoleColor.Green;
                    Log.Info($"Done! in {ElapsedTimeString}");
                    Console.ResetColor();
                    
                }
            }

            

            return 0;
        }
    }
}