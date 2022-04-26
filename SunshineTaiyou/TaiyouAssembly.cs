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
        string Namespace;
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
                Console.WriteLine(token.ToString());
            }

        }
    }
}
