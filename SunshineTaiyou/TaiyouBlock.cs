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

        public TaiyouBlock(string initiator, string string_parameters, string body_string)
        {
            Initiator = initiator;
            //Parameters = Utils.ParseParametersString(string_parameters);
        }


        public override string ToString()
        {
            string parms_string = "";

            if (Parameters != null)
            {
                for (int i = 0; i < Parameters.Length; i++)
                {
                    if (i == Parameters.Length - 1)
                    {
                        parms_string += Parameters[i].ToString();

                    }
                    else
                    {
                        parms_string += Parameters[i].ToString() + ", ";

                    }
                }
            }

            return $"TaiyouToken; Initiator: {Initiator}, Parameters: [{parms_string}]";
        }

    }
}
