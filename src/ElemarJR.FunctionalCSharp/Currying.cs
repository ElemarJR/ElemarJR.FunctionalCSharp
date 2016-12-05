using System;

namespace ElemarJR.FunctionalCSharp
{
    public static partial class Helpers
    {
        public static Func<TA, Func<TB, TResult>> Curry<TA, TB, TResult>(
            this Func<TA, TB, TResult> func
        ) => a => b => func(a, b);

        public static Func<TA, Func<TB, Func<TC, TResult>>> Curry<TA, TB, TC, TResult>(
            this Func<TA, TB, TC, TResult> func
        ) => a => b => c => func(a, b, c);
    }
}
