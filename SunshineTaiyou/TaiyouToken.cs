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

        }

        public override string ToString()
        {
            string parms_string = "";

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

            return $"TaiyouToken; Initiator: {Initiator}, Parameters: [{parms_string}], Type: {Type}";
        }
    }
}