using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunshineTaiyou
{
    public class TaiyouBlock
    {
        public string Initiator;
        public object[] Parameters;
        public TaiyouToken[] InnerTokens;

        public TaiyouBlock(string initiator, object[] parameters, TaiyouToken[] innerTokens)
        {
            Initiator = initiator;
            Parameters = parameters;
            InnerTokens = innerTokens;
        }

        public TaiyouBlock(string initiator, string string_parameters, TaiyouToken[] innerTokens)
        {
            Initiator = initiator;
            Parameters = Utils.ParseParametersString(string_parameters);
            InnerTokens = innerTokens;
        }
    }
}
