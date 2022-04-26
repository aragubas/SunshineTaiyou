using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunshineTaiyou.TokenTypes
{
    public class MethodCalling : TaiyouToken
    {
        TaiyouSymbol Symbol;
        object[] Arguments;

        public MethodCalling(string name, string parameters_string)
        {
            Symbol = new TaiyouSymbol(name, SymbolContext.MethodReference);
            Arguments = Utils.ParseParametersString(ref parameters_string, true, SymbolContext.MethodParameters);
        }

        public override string ToString()
        {
            string parameters_string = "";

            if (Arguments != null)
            {
                for (int i = 0; i < Arguments.Length; i++)
                {
                    string value_string = $"{Arguments[i].GetType()}; '{Arguments[i].ToString()}'";

                    if (Arguments[i].GetType() == typeof(TaiyouSymbol))
                    {
                        value_string = $"TaiyouSymbol; Name: '{(Arguments[i] as TaiyouSymbol).Name}'";
                    }

                    if (i == Arguments.Length - 1)
                    {
                        parameters_string += value_string;

                    }
                    else
                    {
                        parameters_string += value_string + ", ";

                    }
                }
            }


            return $"{this.GetType().Name}Token; SymbolicName: {Symbol.Name}, Parameters: [{parameters_string}]";
        }
    }
}
