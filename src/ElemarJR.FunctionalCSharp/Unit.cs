using System;

namespace ElemarJR.FunctionalCSharp
{
    public struct Unit
    { }

    public static partial class Helpers
    {
        private static readonly Unit unit = new Unit();
        public static Unit Unit() => unit;

        public static Func<T, Unit> ToFunc<T>(Action<T> action) => o =>
        {
            action(o);
            return Unit();
        };

        public static Func<Unit> ToFunc(Action action) => () =>
        {
            action();
            return Unit();
        };
    }
}