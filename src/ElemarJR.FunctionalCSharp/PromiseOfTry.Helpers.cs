using System;

namespace ElemarJR.FunctionalCSharp
{
    public static partial class Helpers
    {
        public static PromiseOfTry<T> PromiseOfTry<T>(Func<T> func) => func as PromiseOfTry<T>;

    }
}
