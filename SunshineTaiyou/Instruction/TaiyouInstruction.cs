namespace SunshineTaiyou.Instruction
{
    // Interface for Taiyou Instruction
    abstract class TaiyouInstruction
    {
        internal object[] arguments;
        internal Routine ParentRoutine;

        public TaiyouInstruction(object[] arguments, Routine parentRoutine) { this.arguments = arguments; ParentRoutine = parentRoutine; }

        public abstract void run();

    }

}