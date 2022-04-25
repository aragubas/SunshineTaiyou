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
            bool VariableDeclaration_ReadingBody = false;
            string VariableType = "";
            string VariableName = "";
            string VariableBody = "";
            TaiyouToken VariableDeclarationToken = null;

            for (int i = 0; i < input.Length; i++)
            {
                char current_char = input[i];

                if ((i == 0 && current_char == '"') || (current_char == '"' && input[i - 1] != '\\'))
                {
                    StringBlock = !StringBlock;
                }
                
                if (current_char == ':' && last_char == ':' && !StringBlock)
                {
                    VariableType = current_reading.Remove(current_reading.Length - 1);
                    VariableDeclaration = true;

                    current_reading = "";
                    continue;
                }

                if (current_char == '=' && !StringBlock)
                {
                    VariableName = current_reading.TrimEnd();
                    
                    current_reading = "";
                    continue;
                }

                if (current_char == ';' && !StringBlock)
                {
                    VariableBody = current_reading.Trim();


                    current_reading = "";
                    last_char = ' ';
                    continue;
                }


                current_reading += current_char;
                last_char = current_char;
            }

            return Output.ToArray();
        }

    }
}
