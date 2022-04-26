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
            Console.WriteLine($"TaiyouScript Compiler (tysc) v{Version[0]}.{Version[1]}.{Version[2]}-{BuildChannel}");
        }
        // Main entry point
        static void Main(string[] args)
        {
            PrintLogo();

            //
            // Reads and parses the main assembly
            //
            TaiyouAssembly mainAssembly = new TaiyouAssembly("./program/main.tiy");

            
        }
    }
}