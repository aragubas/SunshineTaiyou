using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunshineTaiyou.TokenTypes
{
    internal class VariableAssignment : TaiyouToken
    {
        public new TaiyouSymbol Name;
        public object Value;

        public VariableAssignment(string name, string value)
        {
            Name = new TaiyouSymbol(name, SymbolContext.Assignment);
            Value = Utils.ParseParametersString(ref value, true, SymbolContext.Assignment)[0];
        }

        public override string ToString()
        {
            return $"{this.GetType().Name}Token; SymbolicName: {Name.Name}, Value: '{Value}'";
        }
    }
}
