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

    public static class Option
    {
        public static Option<T> Of<T>(T value)
            => new Option<T>(value, value != null);

        public static Option<TResult> Apply<T, TResult>
            (this Option<Func<T, TResult>> @this, Option<T> arg)
            => @this.Bind(f => arg.Map(f));

        public static Option<Func<TB, TResult>> Apply<TA, TB, TResult>
             (this Option<Func<TA, TB, TResult>> @this, Option<TA> arg)
             => Apply(@this.Map(Helpers.Curry), arg);


        public static Option<TR> Map<T, TR>(
            this Option<T> @this,
            Func<T, TR> mapfunc
            ) =>
                @this.IsSome
                    ? Some(mapfunc(@this.Value))
                    : None;

        public static Option<TR> Bind<T, TR>(
                this Option<T> @this,
                Func<T, Option<TR>> bindfunc
            ) =>
            @this.IsSome
                ? bindfunc(@this.Value)
                : None;

        public static T GetOrElse<T>(
            this Option<T> @this,
            Func<T> fallback
            ) =>
                @this.Match(
                    some: value => value,
                    none: fallback
                    );

        public static T GetOrElse<T>(
            this Option<T> @this,
            T @else
            ) =>
                GetOrElse(@this, () => @else);

    }

    public struct NoneType { }

    public static partial class Helpers
    {
        public static Option<T> Some<T>(T value) => Option.Of(value);
        public static readonly NoneType None = new NoneType();
    }

}

namespace System.Linq
{
    using ElemarJR.FunctionalCSharp;
    using static Helpers;
    public static partial class LinqExtensions
    {
        public static Option<TResult> Select<T, TResult>(
                this Option<T> @this,
                Func<T, TResult> func)
            => @this.Map(func);

        public static Option<TResult> SelectMany<T, TB, TResult>(
            this Option<T> @this,
            Func<T, Option<TB>> bind,
            Func<T, TB, TResult> projector
        ) => @this.Match(
            none: () => None,
            some: (t) => @this.Bind(bind).Match(
                none: () => None,
                some: (tb) => Some(projector(t, tb))
            )
        );

        public static Option<T> Where<T>(
            this Option<T> option,
            Func<T, bool> predicate
        ) => option.Match(
            none: () => None,
            some: o => predicate(o) ? option : None
        );
    }
}

