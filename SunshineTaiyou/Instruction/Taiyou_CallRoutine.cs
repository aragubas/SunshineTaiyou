using System;

namespace SunshineTaiyou.Instruction
{
    class Taiyou_CallRoutine : TaiyouInstruction
    {
        public Taiyou_CallRoutine(object[] arguments, Routine parentRoutine) : base(arguments, parentRoutine) {}

        public override void run()
        {
            string RoutineName = (string)arguments[0];

            ParentRoutine.ParentNamespace.Routines[RoutineName].run();
        }
    }

}