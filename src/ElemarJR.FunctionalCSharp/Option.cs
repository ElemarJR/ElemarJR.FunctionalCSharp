using System;
using Helpers = ElemarJR.FunctionalCSharp.Helpers;

namespace ElemarJR.FunctionalCSharp
{
    using static Helpers;

    public struct Option<T>
    {
        internal T Value { get; }
        public bool IsSome { get; }
        public bool IsNone => !IsSome;

        internal Option(T value, bool isSome)
        {
            Value = value;
            IsSome = isSome;
        }

        public TR Match<TR>(Func<T, TR> some, Func<TR> none)
            => IsSome ? some(Value) : none();

        public Unit Match(Action<T> some, Action none)
            => Match(ToFunc(some), ToFunc(none));

        public static readonly Option<T> None = new Option<T>();

        public static implicit operator Option<T>(T value)
            => Some(value);

        public static implicit operator Option<T>(NoneType _)
            => None;
    }
}


