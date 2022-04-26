using SunshineTaiyou.Exceptions;
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
        string FileName;
        string[] SourceCode;
        string Namespace = null;
        string[] Imports;
        List<TaiyouToken> TopLevelTokens = new List<TaiyouToken>();
        List<TaiyouBlock> TopLevelBlocks = new List<TaiyouBlock>();

        public TaiyouAssembly(string FilePath)
        {
            SourceCode = File.ReadAllLines("./program/main.tiy");
            List<string> FirstStepParserOutput = Parsers.ParserStepOne(ref SourceCode);
            List<string> SecondStepParserOutput = Parsers.ParserRemoveInlineBlockComments(ref FirstStepParserOutput);

            TopLevelTokens.AddRange(Parsers.ParserGetAtDefinitions(ref SecondStepParserOutput));
            TopLevelBlocks.AddRange(Parsers.ParserGetTopLevelRoutineBlocks(ref SecondStepParserOutput));

            Console.WriteLine("ToplevelToken:");
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
                        if (atToken.Parameters[0].GetType() != typeof(String))
                        {
                            throw new TaiyouException("Namespace parameter must be a string");
                        }

                        Namespace = (string)atToken.Parameters[0];
                        continue;
                    }
                }
            }

        }
    }
}
