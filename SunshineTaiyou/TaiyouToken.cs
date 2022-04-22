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

namespace SunshineTaiyou
{
    enum TaiyouTokenType
    {
        AtDefinition,
        FunctionDefinition,
        Instruction
    }

    class TaiyouToken
    {
        public string Initiator;
        public object[] Parameters;
        public TaiyouTokenType Type;

        public TaiyouToken(string initiator, object[] parameters, TaiyouTokenType type)
        {
            Initiator = initiator;
            Parameters = parameters;
            Type = type;
        }

        public TaiyouToken(string initiator, string parameters_to_parse, TaiyouTokenType type)
        {
            Initiator = initiator;
            ParseParameters(parameters_to_parse);
            Type = type;
        }

        public TaiyouToken() { }

        public void ParseParameters(string string_parameters)
        {
            // Step 1 - Convert parameter list to string list
            List<string> str_parameters = new List<string>();

            bool StringBlock = false;
            string Output = "";

            for (int i = 0; i < string_parameters.Length; i++)
            {
                char current_char = string_parameters[i];
                
                if (current_char == '"' && (string_parameters.Length > 1 && string_parameters[i - 1] != '\\'))
                {
                    StringBlock = !StringBlock;
                }

                if (!StringBlock)
                {
                    if (current_char == ',')
                    {
                        str_parameters.Add(Output);

                        Console.WriteLine("Added parameter: " + Output);
                        Output = "";
                        
                    }
                }

                Output += current_char;
            }


        }

        public override string ToString()
        {
            string parms_string = "";

            if (Parameters != null)
            {
                for(int i = 0; i < Parameters.Length; i++)
                {
                    if (i == Parameters.Length - 1)
                    {
                        parms_string += Parameters[i].ToString();
                    }else
                    {
                        parms_string += Parameters[i].ToString() + ", ";
                    }
                }
            }

            return $"TaiyouToken; Initiator: {Initiator}, Parameters: [{parms_string}], Type: {Type}";
        }
    }
}