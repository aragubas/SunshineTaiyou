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
using System.Text.RegularExpressions;
using TaiyouFramework.Exception;
using System;


namespace TaiyouFramework
{
    // Class for holding Taiyou Assembly
    public class TaiyouAssembly : TaiyouObj
    {
        public SourceAssembly source;
        public bool IsCompiled = false;
        public List<TaiyouBlock> AvailableBlocks = new List<TaiyouBlock>();
        public List<TaiyouLine> SuperLines = new List<TaiyouLine>();

        ///<summary>A taiyou assembly with taiyou code</summary>
        ///<param name="pContext">executing TaiyouContext</param>
        ///<param name="pSource">SourceAssembly containing assembly information</param>
        public TaiyouAssembly(TaiyouContext pContext, SourceAssembly pSource)
        {
            context = pContext; source = pSource;

        }
 
        /// <summary>Compile code inside this assembly, after compiling this assembly will be added to <c>Taiyou Context</c></summary>
        public void Compile()
        {
            if (IsCompiled) { Console.WriteLine($"Warning::Assembly: Cannot compile assembly \"{source.AssemblyName}\", already compiled."); return; }
            string[] CodeSplit = source.SourceCode.Split(new char[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
            bool CommentLineBlock = false;
 
            bool BlockStart = false;
            List<string> BlockArguments = new List<string>();
            List<TaiyouLine> BlockLines = new List<TaiyouLine>();

            // Breaks code into lines
            foreach (string line in CodeSplit)
            {
                // Handle comment blocks comments
                if (line.StartsWith("//")) { continue; }
                if (line.StartsWith("/*")) { CommentLineBlock = true; continue; }
                if (line.StartsWith("/")) { CommentLineBlock = false; continue; }
                if (CommentLineBlock) { continue; }
                
                // Gell all tokens
                List<string> tokensList = ReadStringTokens(line.Trim());

                // Get denominator type and list arguments
                List<string> ArgumentList = new List<string>();
                TaiyouLine newLine = new TaiyouLine();
                bool IsSuperTokenLine = false;

                #region Set denominator type and get arguments
                for(int i = 0; i < tokensList.Count; i++)
                {
                    string piece = tokensList[i];

                    if (i == 0) // Denominator if first index 
                    {
                        // "Super" token
                        if (piece.StartsWith("@"))
                        {
                            IsSuperTokenLine = true;
                            newLine.denominator = Tokens.Denominator.Super;                  

                            // Add denominator type
                            ArgumentList.Add(piece.Remove(0, 1));

                        }
                        
                        // "Block" token
                        else if (piece.StartsWith("$"))
                        {
                            newLine.denominator = Tokens.Denominator.BlockStart;
                            if (piece == "$end") { newLine.denominator = Tokens.Denominator.SpecialBlockEnd; }
                            // Add block type
                            ArgumentList.Add(piece.Remove(0, 1));

                        }
                        // If line doesn't start with "Super" or "Block" token its a instruction
                        else { newLine.denominator = Tokens.Denominator.Instruction; newLine.InstructionName = piece; }

                    }
                    else{ // If not reading first token, add arguments
                        
                        // Instruction Arguments
                        ArgumentList.Add(piece);
                    }

                } // After getting denominator type and listing arguments
                #endregion
                // Set properties of "new line" object
                newLine.parentAssembly = this;
                newLine.Arguments = new string[ArgumentList.Count];
                ArgumentList.CopyTo(0, newLine.Arguments, 0, ArgumentList.Count);

                // Check for Block Start
                #region Block Handling
                if (newLine.denominator == Tokens.Denominator.BlockStart) 
                { 
                    if (!BlockStart)
                    { 
                        BlockArguments = ArgumentList; 
                        BlockStart = true; 
                        continue;

                    } 
                    else 
                    { 
                        throw new CompilationError("Cannot declare routine inside another; One routine needs to end before declaring another one.", source); 
                    } 
                }

                // Check for special block end
                if (newLine.denominator == Tokens.Denominator.SpecialBlockEnd)
                {
                    if (BlockStart)
                    {
                        AccessModifier access;

                        // Parse access modifier
                        switch(BlockArguments[1])
                        {
                            case "private": access = AccessModifier.Private; break;
                            case "public": access = AccessModifier.Public; break;
                            default: throw new CompilationError($"Access modifier \"{BlockArguments[1]}\" is invalid.", source);
                        }

                        TaiyouBlock newBlock = new TaiyouBlock((string)BlockArguments[2], access, BlockLines);
                        AvailableBlocks.Add(newBlock);

                        BlockStart = false;
                        continue;

                    }else { throw new CompilationError($"Cannot end block because no block has been started.", source); }
                }
                #endregion
 
                // Add line to block lines list
                if (BlockStart || IsSuperTokenLine)
                {
                    if (BlockStart)
                    {
                        BlockLines.Add(newLine);

                    }else if (IsSuperTokenLine)
                    {
                        SuperLines.Add(newLine);
                    }
                    continue;
                }



                

            }

            context.AddLoadedAssembly(source, this);
            IsCompiled = true;
        }

        ///<summary>Splits string into tokens</summary>
        ///<param name="AnalysisString">Analysis <c>string<c/></param>
        private List<string> ReadStringTokens(string AnalysisString)
        {
            bool StringLiteralStarted = false;
            bool IgnoreToken = false;
            bool CommentToken = false;
            int CommentLevel = 0;
            string CurrentResult = "";
            List<string> FinalResult = new List<string>();
            
            // Add final result list
            void EndResult() { FinalResult.Add(CurrentResult); CurrentResult = ""; }

            foreach (char Char in AnalysisString)
            {
                // ====================
                // Ignore token, token 
                // Has the highest priority to allow ignoring comment lines and other tokens
                // Example: "\/*  \/" == "/* /"
                if (Char == '\\')
                {
                    IgnoreToken = true;
                    continue;
                }

                // ===============================================
                // Comments line have priority below ignore token
                // to allow ignoring comment line
                #region Handle inline comment
                // Comment line '/' char 
                if (Char == '/') 
                { 
                    if (CommentLevel == 2) 
                    {  
                        CommentToken = false; 
                        CommentLevel = 0;
                        continue;
                    } 
                    CommentLevel++; 
                    CommentToken = true; 
                    continue; 
                }
                
                // Comment line '*' char
                if (Char == '*') 
                { 
                    if (CommentLevel == 1) { CommentLevel++; continue; }
                }
                
                // Jumps to next char if CommentToken is enabled
                if (CommentToken)
                {
                    continue;
                }
                #endregion
 
                // =======================
                // String enclosure token
                if (Char == '\"' && !IgnoreToken)
                {
                    if (!StringLiteralStarted) { StringLiteralStarted = true; continue; }

                    StringLiteralStarted = false;
                    EndResult();                    
                    continue;

                }

                // ======================
                // Space enclosure token
                if (Char == ' ' && !StringLiteralStarted)
                {
                    if (CurrentResult != " " && CurrentResult.Length >= 1) 
                    { 
                        // Check if Current Result is a number (no one can read regex so i have to comment)
                        if (Regex.IsMatch(CurrentResult, @"^\d+$") || Regex.IsMatch(CurrentResult, @"^[1-9]\d*(,\d+)?$"))
                        {
                            if (CurrentResult.GetType() == typeof(int)) { EndResult(); }
                            if (CurrentResult.GetType() == typeof(float)) { EndResult(); }
                        }
                        else // Leave as null (to be checked at compile time)
                        {
                            EndResult();
                        }
                        
                    }
                    continue;
                }

                // ========================
                // Add character to result
                CurrentResult += Char;
                if (IgnoreToken) { IgnoreToken = false; continue; }

            }

            // =============================
            // Add result with single piece
            if (CurrentResult.Length >= 1 && CurrentResult != " ") { EndResult(); }



            return FinalResult;

        }


    }

    // Class for holding assembly source code information
    public class SourceAssembly : TaiyouObj
    {
        public string SourceCode = "";
        public string AssemblyName = "";

        public override string ToString()
        {
            return AssemblyName;
        }

        public SourceAssembly(string pSourceCode, string pAssemblyName) { SourceCode = pSourceCode; AssemblyName = pAssemblyName; }
    }

}