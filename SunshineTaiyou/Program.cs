/*
    Copyright 2021 Aragubas

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
using System.IO;
using System.Reflection;
using TaiyouFramework;

namespace SunshineTaiyou
{
    class Program
    {
        public static TaiyouContext mainContext = new TaiyouContext("Main Context");

        // Main entry point
        static void Main(string[] args)
        {
            CompileScripts();

        }
 
        public static void CompileScripts()
        {
            // Create "Core" namespace
            mainContext.AddNamespace("Core");

            DirectoryInfo files = new DirectoryInfo("program");
            string currentDir = Path.Combine("program", Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase));

            foreach(FileInfo info in files.GetFiles("*.tasm", SearchOption.AllDirectories))
            {
                string AssemblyName = Path.GetRelativePath(currentDir, info.FullName).Replace("../", "");

                Console.WriteLine($"-- Loading assembly \"{AssemblyName}\"...");

                TaiyouAssembly newAssembly = new TaiyouAssembly(mainContext, new SourceAssembly(File.ReadAllText(info.FullName), AssemblyName));
                
                Console.WriteLine("Compiling assembly...");
                newAssembly.Compile();
                Console.WriteLine("Done!");

            }




        }
    }
}
