using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunshineTaiyou
{
    public class TaiyouBlock
    {
        public string Initiator;
        public object[] Parameters;
        public TaiyouToken[] InnerTokens;

        public TaiyouBlock(string initiator, object[] parameters, TaiyouToken[] innerTokens)
        {
            Initiator = initiator;
            Parameters = parameters;
            InnerTokens = innerTokens;
        }

        public TaiyouBlock(string initiator, string string_parameters, string body_string)
        {
            Initiator = initiator;
            Parameters = Utils.ParseParametersString(ref string_parameters, true, SymbolContext.BlockParameters);
            InnerTokens = Parsers.MethodParser(ref body_string);
        }

        public override string ToString()
        {
            string parms_string = "";

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

            return $"TaiyouBlock; Initiator: {Initiator}, Parameters: [{parms_string}]";
        }

    }
}
