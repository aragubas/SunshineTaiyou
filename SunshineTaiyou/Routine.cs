using System.Collections.Generic;
using SunshineTaiyou.Instruction;

namespace SunshineTaiyou
{
    class Routine
    {
        public string Name;
        public List<TaiyouInstruction> Instructions = new();
        public TaiyouNamespace ParentNamespace;

        public Routine(string name, TaiyouNamespace parentNamespace)
        { 
            Name = name; 
            ParentNamespace = parentNamespace;
        }

        public void run()
        {
            foreach (TaiyouInstruction instruction in Instructions)
            {
                instruction.run();
            }
        }
    }
}