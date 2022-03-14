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
        abstract class Token
        {
            public string[] Data;
        }

        class ImportToken : Token {  }
        class NamespaceToken : Token {  }


        class RoutineBlock
        {
            public List<string> Arguments = new();
            public List<string> Statements = new();
        }

        // Main entry point
        static void Main(string[] args)
        {   
            string SourceCode = File.ReadAllText("./program/main.tiy").Trim().ReplaceLineEndings();
            string ParsedSourceCode = "";
            List<Token> tokens = new();
            List<RoutineBlock> routineBlocks = new();

            // Removes white spaces and comment lines
            bool CommentTokenStart = false;
            bool BlockComment = false;
            char LastChar = ' ';
            bool MultipleWhiteSpace = false;
            bool LiteralBlock = false;
            for (int i = 0; i < SourceCode.Length; i++)
            {
                char CurrentChar = SourceCode[i];
                
                if (CurrentChar == '"')
                {
                    LiteralBlock = !LiteralBlock;
                }

                if (CurrentChar == '/') 
                { 
                    CommentTokenStart = true;
                    continue;
                }

                if (CurrentChar == '*') 
                { 
                    if (CommentTokenStart && !LiteralBlock)
                    {
                        BlockComment = !BlockComment;
                        continue;
                    }
                }

                MultipleWhiteSpace = CurrentChar == ' ' && LastChar == ' ' && !LiteralBlock;

                if (!BlockComment && !MultipleWhiteSpace)
                {
                    ParsedSourceCode += CurrentChar;
                    LastChar = CurrentChar;
                }

            }

            // Finds @at statements
            LastChar = ' ';
            LiteralBlock = false;
            string Piece = "";
            bool SpecialStatement = false;
            Token currentToken = null;
            string SecondPass = "";
            for (int i = 0; i < ParsedSourceCode.Length; i++)
            {
                char CurrentChar = ParsedSourceCode[i];

                if (CurrentChar == '@')
                {
                    SpecialStatement = true;
                    continue;
                }

                if (CurrentChar == '(' && SpecialStatement)
                {
                    LiteralBlock = true;

                    if (Piece == "namespace")
                    {
                        currentToken = new NamespaceToken();
                    }

                    else if (Piece == "import")
                    {
                        currentToken = new ImportToken();
                    }

                    Piece = "";
                    continue;
                }

                // Ends literal block
                if (CurrentChar == ')' && LiteralBlock && SpecialStatement)
                {
                    LiteralBlock = false;
                    SpecialStatement = false;

                    if (currentToken != null)
                    {
                        currentToken.Data = new String[] { Piece };
                    }

                    tokens.Add(currentToken);
                    currentToken = null;

                    Piece = "";
                    continue;
                }

                if (SpecialStatement)
                {
                    Piece += CurrentChar;
                    continue;
                }
                SecondPass += CurrentChar;
            }
            ParsedSourceCode = SecondPass.Trim();

            // Console.WriteLine(ParsedSourceCode);


            RoutineBlock routineBlock = null;
            Piece = "";
            bool RoutineBlockIdentify = false;
            bool RoutineBlock = false;
            for (int i = 0; i < ParsedSourceCode.Length; i++)
            {
                Char CurrentChar = ParsedSourceCode[i];

                if (CurrentChar == '$')
                {
                    RoutineBlockIdentify = true;
                    continue;
                }   

                if (CurrentChar == '(' && RoutineBlockIdentify)
                {
                    if (Piece == "function")
                    {
                        RoutineBlock = true;
                        RoutineBlockIdentify = false;

                        routineBlock = new RoutineBlock();

                    }
                    Piece = "";
                    continue;
                }

                if (CurrentChar == ')' && RoutineBlock && RoutineBlockIdentify)
                {
                    RoutineBlockIdentify = false;

                    string[] arguments = Piece.Remove(0, 1).Trim().Split(',');

                    foreach(String arg in arguments)
                    {
                        routineBlock.Arguments.Add(arg.Trim());
                    }

                    Console.WriteLine(arguments[1]);

                    routineBlocks.Add(routineBlock);
                    
                    Piece = "";
                    continue;
                }
 
                if (RoutineBlock && !RoutineBlockIdentify)
                {
                    Console.WriteLine(Piece);
                    
                }

                if (RoutineBlockIdentify || RoutineBlock)
                {
                    Piece += CurrentChar;
                }
            }


            // foreach(Token token in tokens)
            // {
            //     Console.WriteLine(token);
            //     Console.Write("  ");

            //     foreach(string data in token.Data)
            //     {
            //         Console.Write($"\'{data}\', ");
            //     }

            //     Console.Write("\n");
            // }


            // CompileScripts();

        }
 
        public static void CompileScripts()
        {
            DirectoryInfo files = new DirectoryInfo("program");
            string currentDir = Path.Combine("program", Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase));

            foreach(FileInfo info in files.GetFiles("*.tasm", SearchOption.AllDirectories))
            {
                string AssemblyName = Path.GetRelativePath(currentDir, info.FullName).Replace("../", "");

                Console.WriteLine($"-- Loading assembly \"{AssemblyName}\"...");

                
                Console.WriteLine("Compiling assembly...");


                Console.WriteLine("Done!");

            }




        }
    }
}
