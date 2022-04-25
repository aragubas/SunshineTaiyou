using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunshineTaiyou.TokenTypes
{
    public class VariableAssignment : TaiyouToken
    {
        new TaiyouSymbol Name;
        object Value;

        public VariableAssignment(string name, string value)
        {
            Name = new TaiyouSymbol(name, SymbolContext.Assignment);
            Value = Utils.ParseParametersString(ref value, true, SymbolContext.Assignment)[0];
        }
    }
}
