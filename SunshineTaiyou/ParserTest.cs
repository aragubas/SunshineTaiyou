using System;
using System.Collections.Generic;
using System.IO;

namespace SunshineTaiyou
{
    class Parser
    {
        static string ParserRemoveCommentLines(string input)
        {
            bool CommentLineStart = false;
            bool CommentLine = false;
            bool StringSequenceEscape = false;
            string output = "";

            for(int i = 0; i < input.Length; i++)
            {
                char CurrentChar = input[i];
 
                if (CurrentChar == '"' && !CommentLine)
                {
                    StringSequenceEscape = !StringSequenceEscape;         
                }

                if (CurrentChar == '/' && !StringSequenceEscape)
                {
                    CommentLineStart = true;
                    continue;
                }

                if (CurrentChar == '*' && CommentLineStart && !StringSequenceEscape)
                {
                    if (CommentLine)
                    {
                        CommentLine = false;
                        CommentLineStart = false;
                        
                    }else
                    {
                        CommentLine = true;
                    }

                    continue;
                }


                // Validate current char
                if (!CommentLineStart && !CommentLine || StringSequenceEscape)
                {
                    output += CurrentChar;
                }
            }

            return output;
        }

        class AtDirective
        {
            public string directiveName;
            public string arguments;

            public AtDirective(string directiveName, string arguments)
            {
                this.directiveName = directiveName;
                this.arguments = arguments;
            }
        }

        static List<AtDirective> ParseAtDirective(string input)
        {
            List<AtDirective> output = new List<AtDirective>();

            bool readingArguments = false;
            bool directiveTypeSet = false;
            string currentArgument = "";
            string directiveName = "";

            for(int i = 0; i < input.Length; i++)
            {
                char currentChar = input[i];

                // Initiator character
                if (currentChar == '@')
                {
                    readingArguments = true;
                    continue;
                }

                if (currentChar == '(' && !directiveTypeSet && readingArguments)
                {
                    directiveName = currentArgument;
                    directiveTypeSet = true;
 
                    currentArgument = "";
                    continue;
                }

                if (currentChar == ')' && directiveTypeSet)
                {
                    output.Add(new AtDirective(directiveName, currentArgument));
                    
                    readingArguments = false;
                    directiveTypeSet = false;
                    directiveName = "";

                    continue;
                }

                if (readingArguments)
                {
                    currentArgument += currentChar;
                }
            }
  
            return output;
        }

        static string ParserRemoveEmptySpaces(string input)
        {
            string output = "";


            bool spaceRepeated = false;
            bool stringLiteral = false;
            for(int i = 0; i < input.Length; i++)
            {
                char currentChar = input[i];
                
                if (currentChar == '"')
                { 
                    stringLiteral = !stringLiteral;
                }


                spaceRepeated = currentChar == ' ' && !stringLiteral;

                if (!spaceRepeated)
                {
                    output += currentChar;
                }
            }

            return output;
        }
 
        public static void Parse()
        {
            string SourceCode = File.ReadAllText("./program/main.tiy").ReplaceLineEndings().Replace(Environment.NewLine, "").Trim();
            string output = "";

            // Step 1 - Removes all comment line
            output = ParserRemoveCommentLines(SourceCode);

            // Step 2 - Removes empty spaces
            output = ParserRemoveEmptySpaces(output);

            // Console.WriteLine($"[{output}]");


            // Step 3 - Tokenize At directives
            List<AtDirective> atDirectives = ParseAtDirective(output);

            foreach(AtDirective directive in atDirectives)
            {
                Console.WriteLine($"[{directive.directiveName}]\n  |[{directive.arguments}]\n");
            }


            


            Console.WriteLine(output);
        }

    }
}