using SunshineTaiyou.TokenTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunshineTaiyou.Runtime
{
    public class RuntimeMachine
    {
        TaiyouProject project;
        
        public RuntimeMachine(TaiyouProject project)
        {
            this.project = project;
        }

        public void Run()
        {
            bool Execution = true;
            string CurrentRoutinePath = project.EntryPoint;

            Dictionary<TaiyouSymbol, object> StackMemory = new Dictionary<TaiyouSymbol, object>();

            TaiyouBlock currentBlock = null;
            int StackPointer = 0;

            while (Execution)
            {
                if (currentBlock == null)
                {
                    string[] routinePath = CurrentRoutinePath.Split(':');
                    currentBlock = project.Namespaces[routinePath[0]].blocks[routinePath[1]];
                }

                if (StackPointer > currentBlock.InnerTokens.Length)
                {
                    Execution = false;
                    continue;
                }

                TaiyouToken currentToken = currentBlock.InnerTokens[StackPointer];

                if (currentToken.GetType() == typeof(VariableDefinition))
                {
                    VariableDefinition definition = (VariableDefinition)currentToken;
 
                    switch (definition.VariableType.ToLower())
                    {
                        case "string":
                        {
                            StackMemory.Add(definition.Name, Convert.ToString(definition.Value));
                            break;
                        }

                    }
                    

                }

                StackPointer++;
            }

        }

    }
}
