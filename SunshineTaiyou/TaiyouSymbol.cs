using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunshineTaiyou
{
    public enum SymbolContext
    {
        None,
        BlockParameters,
        Assignment,
        MethodReference,
        MethodParameters
    }

    public class TaiyouSymbol
    {
        public string Name;
        public SymbolContext Context;

        public TaiyouSymbol(string name, SymbolContext context)
        {
            Name = name;
            Context = context;
        }
    }
}
