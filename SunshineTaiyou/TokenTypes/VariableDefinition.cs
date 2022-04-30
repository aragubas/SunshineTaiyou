using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunshineTaiyou
{
    internal class VariableDefinition : TaiyouToken
    {
        public string VariableType = "";
        public object Value;
        
        public VariableDefinition() { }

        public VariableDefinition(string type, string name, string value)
        {
            Name = name;
            VariableType = type;
            Value = Utils.ParseParametersString(ref value)[0];
        }

        public override string ToString()
        {
            return $"{this.GetType().Name}Token; Name: {Name}, Type: {VariableType}, Value: '{Value}'";
        }

    }
}
