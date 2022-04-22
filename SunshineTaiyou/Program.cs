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
        static string ParserRemoveWhitespaces(ref string input)
        {
            string[] lines = input.Split('\n');
            string output = "";
            for (int i = 0; i < lines.Length; i++)
            {
                string current_line = lines[i];
                if (current_line == "" || current_line == " ") { continue; }
                output += current_line + "\n";
            }
            
            return output.Trim();
        }

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

        static List<string> ParserStepTwo(ref List<string> input_lines)
        {
            string entire_file = "";
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

            return new List<string>(ParserRemoveWhitespaces(ref output).Split('\n'));
        }

        static void PrintList(List<string> input)
        {
            for (int i = 0; i < input.Count; i++)
            {
                Console.WriteLine($"'{input[i]}'");
            }
        }

        static List<TaiyouToken> ParserGetAtDefinitions(ref List<string> input)
        {
            string ParsedSourceCode = "";
            List<TaiyouToken> Output = new List<TaiyouToken>();

            foreach (string line in input) { ParsedSourceCode += $"{line}\n"; }
            ParsedSourceCode = ParsedSourceCode.Trim();

            bool AtDeclared = false;
            bool AtReadingBody = false;
            bool StringBlock = false;
            string AtName = "";
            string AtBody = "";
            TaiyouToken atToken = new TaiyouToken();
            char last_char = ' ';

            for (int i = 0; i < ParsedSourceCode.Length; i++)
            {
                char current_char = ParsedSourceCode[i];

                if (current_char == '"' && ParsedSourceCode[i - 1] != '\\')
                {
                    StringBlock = !StringBlock;
                }

                if (current_char == '@' && !AtDeclared)
                {
                    AtDeclared = true;

                    AtName = "";
                    AtBody = "";
                    continue;
                }

                // Start reading body, stop parsing name
                if (current_char == '(' && !AtReadingBody && AtDeclared && !StringBlock)
                {
                    AtName = AtName.Trim();
                    AtReadingBody = true;
                    continue;
                }

                // Stops reading body, create taiyou token object
                if (current_char == ')' && AtReadingBody && AtDeclared && !StringBlock)
                {
                    AtBody = AtBody.Trim();

                    atToken = new TaiyouToken(AtName, AtBody, TaiyouTokenType.AtDefinition);

                    Output.Add(atToken);

                    AtReadingBody = false;
                    AtDeclared = false;
                    AtName = "";
                    continue;
                }

                if (AtDeclared)
                {
                    if (!AtReadingBody)
                    {
                        AtName += current_char;
                    }else
                    {
                        AtBody += current_char;
                    }
                }

                last_char = current_char;
            }

            return Output;
        }

        // Main entry point
        static void Main(string[] args)
        {
            //
            // Parser Step1 : General code cleanup and remove of comment lines
            //
            string[] source_code = File.ReadAllLines("./program/main.tiy");
            List<string> FirstStepParserOutput = ParserStepOne(ref source_code);
            List<string> SecondStepParserOutput = ParserStepTwo(ref FirstStepParserOutput);

            List<TaiyouToken> tokens = new List<TaiyouToken>();

            foreach(TaiyouToken token in ParserGetAtDefinitions(ref SecondStepParserOutput))
            {
                //Console.WriteLine(token.ToString());
                tokens.Add(token);
            }

           
            
            //PrintList(SecondStepParserOutput);
        }
    }
}