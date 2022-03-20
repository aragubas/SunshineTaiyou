using System;

namespace SunshineTaiyou.Instruction
{
    class Taiyou_WriteLine : TaiyouInstruction
    {
        public Taiyou_WriteLine(object[] arguments, Routine parentRoutine) : base(arguments, parentRoutine) { }

        public override void run()
        {
            Console.WriteLine(arguments[0]);
        }
    }
}