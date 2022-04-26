using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunshineTaiyou
{
    public static class Parsers
    {
        public static TaiyouToken[] MethodParser(ref string input)
        {
            string ParsedSourceCode = input;
            List<TaiyouToken> Output = new List<TaiyouToken>();

            // Call signature convention
            // Log(Tstring)

            string current_reading = "";
            bool StringBlock = false;
            List<string> LastSymbol = new List<string>();
            char last_char = ' ';
            bool VariableDeclaration = false;
            bool VariableAssignment = false;
            bool MethodCalling = false;
            bool InnerBlockStatement = false;
            int InnerBlockStatementLevel = 0;
            bool InnerBlockStatementHeadDefined = false;
            string StatementBlockType = "";
            string StatementBlockParameters = "";
            string StatementBlockBody = "";
            string VariableType = "";
            string VariableName = "";
            string VariableBody = "";
            string MethodName = "";
            string MethodBody = "";
            VariableDefinition VariableDeclarationToken = null;
            TokenTypes.VariableAssignment VariableAssignmentToken = null;
            TokenTypes.MethodCalling MethodCallingToken = null;
            TaiyouBlock InnerBlock = null;

            for (int i = 0; i < input.Length; i++)
            {
                char current_char = input[i];

                if ((i == 0 && current_char == '"') || (current_char == '"' && input[i - 1] != '\\'))
                {
                    StringBlock = !StringBlock;
                }
                
                // Type definition symbol
                if (current_char == ':' && last_char == ':' && !StringBlock & !InnerBlockStatement)
                {
                    VariableType = current_reading.Remove(current_reading.Length - 1).Trim();
                    VariableDeclaration = true;

                    current_reading = "";
                    continue;
                }

                // Assign Symbol
                if (current_char == '=' && !StringBlock && !InnerBlockStatement)
                {
                    // Currently declaring a variable
                    if (VariableDeclaration)
                    {
                        VariableName = current_reading.Trim();

                        current_reading = "";
                        continue;

                    }
                    else
                    {
                        // Currently assigning a value to a variable
                        VariableName = current_reading.Trim();

                        VariableAssignment = true;
                        current_reading = "";
                        continue;
                    }
                }

                // Ending symbol
                if (current_char == ';' && !StringBlock && !InnerBlockStatement)
                {
                    // If currently declaring a variable
                    if (VariableDeclaration)
                    {
                        VariableBody = current_reading.Trim();

                        VariableDeclarationToken = new VariableDefinition(VariableType, VariableName, VariableBody);

                        Output.Add(VariableDeclarationToken);

                        current_reading = "";
                        VariableBody = "";
                        last_char = ' ';
                        VariableDeclaration = false;
                        VariableAssignmentToken = null;
                        continue;
                        
                    }
                    else if (VariableAssignment)
                    {
                        // If currently assigning to a variable 
                        VariableBody = current_reading.Trim();

                        VariableAssignmentToken = new TokenTypes.VariableAssignment(VariableName, VariableBody);

                        Output.Add(VariableAssignmentToken);

                        current_reading = "";
                        last_char = ' ';
                        VariableBody = "";
                        VariableAssignment = false;
                        VariableAssignmentToken = null;
                        continue;

                    }
                }

                // Argument list start symbol
                if (current_char == '(' && !StringBlock && !VariableDeclaration && !VariableAssignment && !InnerBlockStatement)
                {
                    current_reading = current_reading.Trim();

                    // If currently in a if block statement 
                    if (current_reading == "if" && InnerBlockStatementLevel == 0)
                    {
                        InnerBlockStatement = true;
                        StatementBlockType = "if";

                    }else
                    {
                        // If currently calling a method
                        MethodCalling = true;

                        MethodName = current_reading.Trim();
                    }

                    current_reading = "";
                    continue;
                }

                if (current_char == ')' && !StringBlock && !VariableDeclaration && !VariableAssignment)
                {
                    // If currently in a statement block
                    if (InnerBlockStatement)
                    {
                        if (!InnerBlockStatementHeadDefined && InnerBlockStatementLevel == 0)
                        {
                            StatementBlockParameters = current_reading.Trim();
                            InnerBlockStatementHeadDefined = true;

                        }
                        else
                        {
                            current_reading += current_char;
                            continue;
                        }
                    }
                    else
                    {
                        // Finished method calling
                        MethodCalling = false;

                        MethodBody = current_reading.Trim();

                        MethodCallingToken = new TokenTypes.MethodCalling(MethodName, MethodBody);

                        Output.Add(MethodCallingToken);

                        MethodCallingToken = null;
                    }

                    current_reading = "";
                    MethodBody = "";
                    MethodName = "";
                    last_char = ' ';
                    continue;
                }

                if (current_char == '{' && InnerBlockStatement && !StringBlock)
                {
                    InnerBlockStatementLevel++;

                    if (InnerBlockStatementLevel > 1)
                    {
                        current_reading += current_char;
                    }
                    continue;
                }

                if (current_char == '}' && InnerBlockStatement && !StringBlock)
                {
                    InnerBlockStatementLevel--;

                    if (InnerBlockStatementLevel == 0)
                    {
                        // Ends block statement
                        StatementBlockBody = current_reading.Trim();

                        InnerBlock = new TaiyouBlock(StatementBlockType, StatementBlockParameters, StatementBlockBody);

                        Output.Add(InnerBlock);

                        InnerBlockStatement = false;
                        InnerBlockStatementHeadDefined = false;
                        StatementBlockBody = "";
                        StatementBlockParameters = "";
                        StatementBlockType = "";
                        InnerBlock = null;
                        current_reading = "";
                        continue;
                    }


                }

                current_reading += current_char;
                last_char = current_char;
            }

            return Output.ToArray();
        }

        public static string ParserRemoveWhitespaces(ref string input)
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
        public static List<string> ParserStepOne(ref string[] input_lines)
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

        public static List<string> ParserRemoveInlineBlockComments(ref List<string> input_lines)
        {
            string entire_file = "";
            foreach (string line in input_lines)
            {
                entire_file += line + "\n";
            }
            entire_file = entire_file.Trim();

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

        public static void PrintList(List<string> input)
        {
            for (int i = 0; i < input.Count; i++)
            {
                Console.WriteLine($"'{input[i]}'");
            }
        }

        public static List<TaiyouToken> ParserGetAtDefinitions(ref List<string> input)
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
            TaiyouToken atToken = new AtDefinition();
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

                    atToken = new AtDefinition(AtName, AtBody);

                    Output.Add(atToken);

                    AtReadingBody = false;
                    AtDeclared = false;
                    AtName = "";
                    atToken = null;
                    continue;
                }

                if (AtDeclared)
                {
                    if (!AtReadingBody)
                    {
                        AtName += current_char;
                    }
                    else
                    {
                        AtBody += current_char;
                    }
                }

                last_char = current_char;
            }

            return Output;
        }

        public static AtDefinitionType ParseAtDefinitionType(ref string input)
        {
            switch (input.ToLower().Trim())
            {
                case "namespace":
                {
                    return AtDefinitionType.@namespace;
                }

                case "import":
                {
                    return AtDefinitionType.import;
                }

                case "option":
                {
                    return AtDefinitionType.option;
                }
            }
            
            return AtDefinitionType.Invalid;
        }

        public static List<TaiyouBlock> ParserGetTopLevelRoutineBlocks(ref List<string> input)
        {
            string ParsedSourceCode = "";
            List<TaiyouBlock> Output = new List<TaiyouBlock>();

            foreach (string line in input) { ParsedSourceCode += $"{line}\n"; }
            ParsedSourceCode = ParsedSourceCode.Trim();

            string BlockParameters = "";
            string BlockBody = "";
            char last_char = ' ';
            string current_reading = "";
            string routine_latch = "";
            bool routine_detect = false;
            bool RoutineBlock = false;
            bool Routine_ReadingParameters = false;
            bool Routine_ReadingBodyStarted = false;
            bool Routine_ReadingBody = false;
            int Routine_SubBlockLevel = 0;
            bool StringBlock = false;

            for (int i = 0; i < ParsedSourceCode.Length; i++)
            {
                char current_char = ParsedSourceCode[i];

                if (current_char == '"' && ParsedSourceCode[i - 1] != '\\')
                {
                    StringBlock = !StringBlock;
                }

                if (!RoutineBlock && current_char == 'r' && last_char == '.' && !StringBlock)
                {
                    routine_detect = true;
                }

                // Detects routine keyword
                if (!RoutineBlock && routine_detect && !StringBlock)
                {
                    bool Valid = false;

                    Valid = current_char == 'r' || current_char == 'o' || current_char == 'u' || current_char == 't' || current_char == 'i' || current_char == 'n' || current_char == 'e';

                    if (Valid)
                    {
                        routine_latch += current_char;

                    }
                    else
                    {
                        if (routine_latch == "routine")
                        {
                            RoutineBlock = true;

                            current_reading = "";
                            routine_detect = false;
                            routine_latch = "";
                            continue;
                        }

                        routine_latch = "";
                    }


                }

                if (RoutineBlock)
                {
                    if (current_char == '(' && !StringBlock && !Routine_ReadingBody)
                    {
                        Routine_ReadingParameters = true;
                        continue;
                    }

                    if (current_char == ')' && Routine_ReadingParameters && !StringBlock)
                    {
                        if (Routine_ReadingParameters)
                        {
                            BlockParameters = current_reading.Trim();
                            current_reading = "";

                            Routine_ReadingBody = true;
                            Routine_ReadingParameters = false;
                            continue;
                        }
                    }

                    // Prevent reset when reading nested blocks
                    if (current_char == '{' && Routine_ReadingBody && Routine_ReadingBodyStarted)
                    {
                        Routine_SubBlockLevel++;
                    }

                    if (current_char == '}' && Routine_SubBlockLevel > 0 && Routine_ReadingBody && Routine_ReadingBodyStarted)
                    {
                        Routine_SubBlockLevel--;
                        current_reading += current_char;
                        continue;
                    }

                    if (current_char == '{' && Routine_ReadingBody && Routine_SubBlockLevel == 0 && !StringBlock)
                    {
                        Routine_ReadingBodyStarted = true;
                        current_reading = "";
                        continue;
                    }


                    if (current_char == '}' && Routine_SubBlockLevel == 0 && !StringBlock)
                    {
                        BlockBody = current_reading.Trim();
                        Output.Add(new TaiyouBlock("routine", BlockParameters, BlockBody));

                        Routine_ReadingBody = false;
                        Routine_ReadingBodyStarted = false;
                        RoutineBlock = false;
                        BlockParameters = "";
                        current_reading = "";

                        continue;
                    }

                    current_reading += current_char;
                }

                last_char = current_char;
            }

            return Output;
        }
    }
}
