using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunshineTaiyou
{
    internal enum SymbolContext
    {
        None,
        BlockParameters,
        Assignment,
        MethodReference,
        MethodParameters
    }

    internal class TaiyouSymbol
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
