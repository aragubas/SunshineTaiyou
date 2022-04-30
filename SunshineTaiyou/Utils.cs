using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunshineTaiyou
{
    internal static class Utils
    {
        public static bool IsOnlyDigits(ref string input)
        {
            foreach (char c in input)
            {
                if (!char.IsNumber(c))
                    return false;
            }
            return true;
        }

        public static object[] ParseParametersString(ref string input, bool HandleUnknowAsSymbol=false, SymbolContext UnknownSymbolContext=SymbolContext.None)
        {
            // Step 1 - Convert parameter list to string list
            List<string> str_parameters = new List<string>();

            bool StringBlock = false;
            string Output = "";

            for (int i = 0; i < input.Length; i++)
            {
                char current_char = input[i];

                if ((i == 0 && current_char == '"') || (current_char == '"' && input[i - 1] != '\\'))
                {
                    StringBlock = !StringBlock;
                }

                // Forcibly Add last character
                if (i == input.Length - 1)
                {
                    Output += current_char;
                }

                if (!StringBlock)
                {
                    if (current_char == ',' || i == input.Length - 1)
                    {
                        Output = Output.Trim();

                        str_parameters.Add(Output);

                        Output = "";
                        continue;
                    }
                }

                Output += current_char;
            }

            // Parse parsed parameters
            List<object> parameters = new List<object>();

            foreach (string parameter in str_parameters)
            {
                object value = null;

                try
                {
                    // String Parameter
                    if (parameter.StartsWith('"') && parameter.EndsWith('"'))
                    {
                        value = parameter.Substring(1, parameter.Length - 2);

                    }

                    // Boolean Parameter
                    else if (parameter == "true" || parameter == "false")
                    {
                        value = bool.Parse(parameter);
                        
                    }

                    // Double Parameter
                    else if (parameter.Contains('.'))
                    {
                        value = double.Parse(parameter);
                        
                    }

                    // Integer Parameter
                    else
                    {
                        value = int.Parse(parameter);

                    }

                }
                catch (FormatException)
                {
                    if (HandleUnknowAsSymbol)
                    {
                        value = new TaiyouSymbol(parameter, UnknownSymbolContext);
                    }
                    else
                    {
                        // Parameter invalid 
                        throw new FormatException($"No suitable type found for parameter: '{parameter}'");
                    }
                }

                parameters.Add(value);
            }

            return parameters.ToArray();

        }
    }
}
