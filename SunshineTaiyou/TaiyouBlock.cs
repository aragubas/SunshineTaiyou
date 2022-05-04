using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunshineTaiyou
{
    public enum TaiyouBlockType
    { 
        Invalid,
        Routine,
        Conditional
    }

    public class TaiyouBlock : TaiyouToken
    {
        public TaiyouBlockType Type;
        public TaiyouToken[] InnerTokens;

        public TaiyouBlock(string initiator, string string_parameters, string body_string)
        {
            Type = ParseBlockType(ref initiator);
            Parameters = Utils.ParseParametersString(ref string_parameters, true, SymbolContext.BlockParameters);
            InnerTokens = Parsers.MethodParser(ref body_string);
        }

        public static TaiyouBlockType ParseBlockType(ref string input)
        {
            switch (input.Trim().ToLower())
            {
                case "routine":
                    return TaiyouBlockType.Routine;

                case "if":
                    return TaiyouBlockType.Conditional;
            }

            return TaiyouBlockType.Invalid;
        }

        public override string ToString()
        {
            string parms_string = "";
            string inner_token_string = "";

            if (Parameters != null)
            {
                for (int i = 0; i < Parameters.Length; i++)
                {
                    string value_string = $"{Parameters[i].GetType()}; '{Parameters[i].ToString()}'";

                    if (Parameters[i].GetType() == typeof (TaiyouSymbol))
                    {
                        value_string = $"TaiyouSymbol; Name: '{(Parameters[i] as TaiyouSymbol).Name}'";
                    }

                    if (i == Parameters.Length - 1)
                    {
                        parms_string += value_string;

                    }
                    else
                    {
                        parms_string += value_string + ", ";

                    }
                }
            }

            if (InnerTokens != null)
            {
                for (int i = 0; i < InnerTokens.Length; i++)
                {
                    inner_token_string += $" -{InnerTokens[i].ToString()}\n";
                }
            }

            return $"TaiyouBlock; Type: {Type}, Parameters: [{parms_string}], InnerTokens: [{inner_token_string}]";
        }

    }
}
