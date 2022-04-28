﻿using SunshineTaiyou.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunshineTaiyou
{
    public class TaiyouAssembly
    {
        public string AssemblyName;
        string[] SourceCode;
        public string Namespace = null;
        public string[] Imports;
        public List<TaiyouToken> TopLevelTokens = new List<TaiyouToken>();
        public List<TaiyouBlock> TopLevelBlocks = new List<TaiyouBlock>();

        public TaiyouAssembly(string FilePath, string SourceFilesPath, ref TaiyouProject project)
        {
            SourceCode = File.ReadAllLines(FilePath);
            AssemblyName = Path.GetRelativePath(SourceFilesPath, FilePath).Replace("\\", ".").Replace("/", ".").Replace(".tiy", "");


            List<string> FirstStepParserOutput = Parsers.ParserStepOne(ref SourceCode);
            List<string> SecondStepParserOutput = Parsers.ParserRemoveInlineBlockComments(ref FirstStepParserOutput);

            TopLevelTokens.AddRange(Parsers.ParserGetAtDefinitions(ref SecondStepParserOutput));
            TopLevelBlocks.AddRange(Parsers.ParserGetTopLevelRoutineBlocks(ref SecondStepParserOutput));

            // Compile top level tokens
            CompileTopLevelTokens();

            if (!project.Namespaces.ContainsKey(Namespace))
            {
                project.Namespaces.Add(Namespace, new TaiyouNamespace());
            }

            project.Namespaces[Namespace].blocks.AddRange(TopLevelBlocks);
        }

        void CompileTopLevelTokens()
        {
            List<string> ParsedImports = new List<string>();
            foreach (TaiyouToken token in TopLevelTokens)
            {
                if (token is AtDefinition)
                {
                    AtDefinition atToken = (AtDefinition)token;

                    if (atToken.Type == AtDefinitionType.@namespace)
                    {
                        // SpellChecker - Assemblies should be in a single namespace
                        if (Namespace != null)
                        {
                            throw new TaiyouException("Assemblies should be in a single namespace");
                        }

                        // SpellChecker - Namespace should only contain 1 parameter and this parameter should be a string
                        if (atToken.Parameters.Count() != 1)
                        {
                            throw new TaiyouException("Namespace must have 1 parameter");
                        }
                        if (atToken.Parameters[0].GetType() != typeof(string))
                        {
                            throw new TaiyouException("Namespace parameter must be a string");
                        }

                        Namespace = (string)atToken.Parameters[0];
                        continue;
                    }

                    if (atToken.Type == AtDefinitionType.import)
                    {
                        if (atToken.Parameters.Count() != 1)
                        {
                            throw new TaiyouException("Import must have 1 parameter");
                        }
                        if (atToken.Parameters[0].GetType() != typeof(string))
                        {
                            throw new TaiyouException("Import parameter must be a string");
                        }

                        ParsedImports.Add((string)atToken.Parameters[0]);
                    }
                }

                Imports = ParsedImports.ToArray();
            }

        }
    }
}
