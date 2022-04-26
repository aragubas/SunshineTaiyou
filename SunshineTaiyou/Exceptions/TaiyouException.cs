using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SunshineTaiyou.Exceptions
{
    public class TaiyouException : Exception
    {
        public TaiyouException(string message) : base(message)
        {
            Log.Error(message);
        }
    }
}
