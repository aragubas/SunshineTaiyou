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
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using SunshineTaiyou.Instruction;

namespace SunshineTaiyou
{
    class Program
    {
        class Runtime
        {
            TaiyouContext context;

            public void Run()
            {
                context = new TaiyouContext();

                TaiyouNamespace mainNamespace = new TaiyouNamespace("main", context);
                
                // Creates program routine with WriteLine instruction
                Routine mainRoutine = new Routine("program", mainNamespace);
                mainRoutine.Instructions.Add(new Taiyou_WriteLine(new object[] { "Hello" }, mainRoutine));
                mainRoutine.Instructions.Add(new Taiyou_CallRoutine(new object[] { "second" }, mainRoutine));
                

                // Creates program routine with WriteLine instruction
                Routine secondRoutine = new Routine("second", mainNamespace);
                secondRoutine.Instructions.Add(new Taiyou_Write(new object[] { "World" }, secondRoutine));


                mainNamespace.Routines.Add("program", mainRoutine);
                mainNamespace.Routines.Add("second", secondRoutine);

                // Create the main namespace
                context.TaiyouNamespaces.Add("main", mainNamespace);
 


                // Runs the program routine in main namespace
                context.TaiyouNamespaces["main"].Routines["program"].run();
                
            }
        }

        // Main entry point
        static void Main(string[] args)
        {   
            // Runtime taiyouRuntime = new Runtime();
            // taiyouRuntime.Run();

            Parser.Parse();

            Console.WriteLine("\n\nPress any key to exit.");
            Console.ReadKey();
        }
    }
}