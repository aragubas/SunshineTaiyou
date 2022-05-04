using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunshineTaiyou
{
    public class TaiyouProject
    {
        public List<TaiyouAssembly> Assemblies = new();
        public Dictionary<string, TaiyouNamespace> Namespaces = new();
        public string EntryPoint = "Main:program";

        public TaiyouProject()
        {
            
        }
    }
}
