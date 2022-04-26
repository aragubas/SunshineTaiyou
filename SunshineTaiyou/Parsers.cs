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

            for (int i = 0; i < input.Length; i++)
            {
                char current_char = input[i];

                if ((i == 0 && current_char == '"') || (current_char == '"' && input[i - 1] != '\\'))
                {
                    StringBlock = !StringBlock;
                }
                
                // Type definition symbol
                if (current_char == ':' && last_char == ':' && !StringBlock)
                {
                    VariableType = current_reading.Remove(current_reading.Length - 1).Trim();
                    VariableDeclaration = true;

                    current_reading = "";
                    continue;
                }

                // Assign Symbol
                if (current_char == '=' && !StringBlock && (!InnerBlockStatement || InnerBlockStatement && InnerBlockStatementHeadDefined))
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
                if (current_char == ';' && !StringBlock)
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
                        continue;

                    }
                }

                // Argument list start symbol
                if (current_char == '(' && !StringBlock && !VariableDeclaration && !VariableAssignment)
                {
                    current_reading = current_reading.Trim();

                    // If currently in a if block statement
                    if (current_reading == "if")
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
                        if (!InnerBlockStatementHeadDefined)
                        {
                            StatementBlockParameters = current_reading.Trim();
                            InnerBlockStatementHeadDefined = true;

                        }else
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
                    }

                    current_reading = "";
                    MethodBody = "";
                    MethodName = "";
                    last_char = ' ';
                    continue;
                }

                if (current_char == '{' && InnerBlockStatement && !StringBlock)
                {
                    continue;
                }

                if (current_char == '}' && InnerBlockStatement && !StringBlock)
                {
                    StatementBlockBody = current_reading.Trim();


                }

                current_reading += current_char;
                last_char = current_char;
            }

            return Output.ToArray();
        }

    }
}
