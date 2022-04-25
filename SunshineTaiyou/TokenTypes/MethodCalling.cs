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
    }
}
