using System;

namespace SunshineTaiyou.Instruction
{
    class Taiyou_Write : TaiyouInstruction
    {
        public Taiyou_Write(object[] arguments, Routine parentRoutine) : base(arguments, parentRoutine) { }

        public override void run()
        {
            Console.Write(arguments[0]);
        }
    }
}