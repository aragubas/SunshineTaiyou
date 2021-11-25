/*
    Copyright 2021 Aragubas

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

using System.Collections.Generic;
using System;

namespace TaiyouFramework
{
    
    public class TaiyouContext
    {
        // Namespace handling
        #region Namespace Manipulation

        public Dictionary<string, Namespace> AvailableNamespaces = new Dictionary<string, Namespace>();

        public void AddNamespace(string NamespaceName)
        {
            // Return if namespace already exists
            if (AvailableNamespaces.ContainsKey(NamespaceName)) { return; }
            // Add namespace if it doesn't exist
            AvailableNamespaces.Add(NamespaceName, new Namespace(this, NamespaceName));
        }
        #endregion


        // Loaded assemblies handling
        #region Loaded Assemblies
        public Dictionary<SourceAssembly, TaiyouAssembly> LoadedAssemblies = new Dictionary<SourceAssembly, TaiyouAssembly>();

        public void AddLoadedAssembly(SourceAssembly AssemblyInformation, TaiyouAssembly assembly)
        {
            if (LoadedAssemblies.ContainsKey(AssemblyInformation)) { return; }
            LoadedAssemblies.Add(AssemblyInformation, assembly);
        }

        #endregion



        public TaiyouContext(string pContextName) { ContextName = pContextName; }
 
        public string ContextName;

        public override string ToString()
        {
            return ContextName;
        }


    }
}