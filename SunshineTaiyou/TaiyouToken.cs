/*
    Copyright 2022 Aragubas

    Licensed under the Apache License, Version 2.0 (the "License");
    you may not use this file except in compliance with the License.
    You may obtain a copy of the License at

        http://www.apache.org/licenses/LICENSE-2.0

    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS,
    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    See the License for the specific language governing permissions and
    limitations under the License.
*/

using System;
using System.Collections.Generic;

namespace SunshineTaiyou
{
    public abstract class TaiyouToken
    {
        public string Name;
        public object[] Parameters;

        public TaiyouToken(string initiator, object[] parameters)
        {
            Name = initiator;
            Parameters = parameters;
        }

        public TaiyouToken(string initiator, string parameters_string)
        {
            Name = initiator;
            Parameters = Utils.ParseParametersString(ref parameters_string);
        }

        public TaiyouToken() { }

        public override string ToString()
        {
            string parms_string = "";

            if (Parameters != null)
            {
                for(int i = 0; i < Parameters.Length; i++)
                {
                    if (i == Parameters.Length - 1)
                    {
                        parms_string += $"{Parameters[i].GetType()}; '{Parameters[i].ToString()}'";

                    }else
                    {
                        parms_string += $"{Parameters[i].GetType()}; '{Parameters[i].ToString()}', ";

                    }
                }
            }

            return $"{this.GetType().Name}Token; Name: {Name}, Parameters: [{parms_string}]";
        }

    }
}