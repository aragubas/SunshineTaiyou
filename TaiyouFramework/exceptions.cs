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

using System;

namespace TaiyouFramework.Exception
{
    public enum InvalidTokenExceptionTokenType
    {
        Super, Special, Instruction
    }
    
    [Serializable]
    public class InvalidTokenException : System.Exception
    {   
        public InvalidTokenException(InvalidTokenExceptionTokenType type, string InvalidToken) : base($"Token '{InvalidToken}' is invalid or doesn't exist for type '{type.ToString()}'.") { }   
    
    }

    [Serializable]    
    public class CompilationError : System.Exception
    {
        public CompilationError(string Message, SourceAssembly source) : base($"Error while compiling assembly \"{source.AssemblyName}\".\n{Message}") { }
    }


}