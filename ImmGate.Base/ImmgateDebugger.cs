using System;
using System.Diagnostics;

namespace ImmGate.Base
{
    public static class ImmgateDebugger
    {

        public static void ConditionalBreak(Func<bool> breakCondition)
        {
            if (breakCondition())
                SafeBreak();
        }
        public static void SafeBreak()
        {

            if (Debugger.IsAttached)
                Debugger.Break();
        }

    }
}
