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

namespace SunshineTaiyou
{
    class Program
    {
        /// <summary> Removes empty lines and inline comments </summary>
        static List<string> ParserStepOne(ref string[] input_lines)
        {
            List<string> ParserOutput = new List<string>();

            for (int i = 0; i < input_lines.Length; i++)
            {
                string line = input_lines[i].Trim();
                if (line.Length < 1) { continue; }

                // First level : Line Starters
                if (line.StartsWith("//")) { continue; }
                
                ParserOutput.Add(line);
            }
            
            return ParserOutput;
        }

        static List<string> ParsetStepTwo(ref List<string> input_lines)
        {
            string entire_file = "";
            List<string> Output = new List<string>();

            foreach (string line in input_lines)
            {
                entire_file += line + "\n";
            }
            entire_file = entire_file.Trim();

            //
            //  Removes all string literals
            // 
            bool StringBlock = false;
            bool BigComment = false;
            string output = "";
            char last_char = ' ';
            for (int i = 0; i < entire_file.Length; i++)
            {
                char current_char = entire_file[i];

                if (current_char == '"' && entire_file[i - 1] != '\\')
                {
                    StringBlock = !StringBlock;
                }

                if (!StringBlock)
                {
                    // Comment start
                    if (current_char == '*' && last_char == '/')
                    {
                        BigComment = true;
                        output = output.Remove(output.Length - 1, 1);
                    }

                    if (current_char == '/' && last_char == '*')
                    {
                        BigComment = false;
                        continue;
                    }
                }

                last_char = current_char;
                if (BigComment)
                {
                    continue;
                }

                output += current_char; 
            }

            Console.WriteLine(output);
            return Output;
        }

        static void PrintList(List<string> input)
        {
            for (int i = 0; i < input.Count; i++)
            {
                Console.WriteLine(input[i]);
            }
        }

        // Main entry point
        static void Main(string[] args)
        {
            string[] source_code = File.ReadAllLines("./program/main.tiy");
            List<string> FirstStepParser = ParserStepOne(ref source_code);
            List<string> SecondStepParser = ParsetStepTwo(ref FirstStepParser);

            //PrintList(SecondStepParser);
        }
    }
}