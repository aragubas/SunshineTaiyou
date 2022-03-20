using System.Collections.Generic;
using SunshineTaiyou.Instruction;

namespace SunshineTaiyou
{
    class TaiyouContext
    {
        public Dictionary<string, TaiyouNamespace> TaiyouNamespaces = new();
    }

    class TaiyouNamespace
    {
        public string NamespaceName;
        public Dictionary<string, Routine> Routines = new();
        public TaiyouContext Context;

        public TaiyouNamespace(string namespaceName, TaiyouContext context)
        { 
            NamespaceName = namespaceName;
            Context = context;
        }        
    }
}