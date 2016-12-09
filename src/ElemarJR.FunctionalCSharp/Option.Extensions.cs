using System;

namespace ElemarJR.FunctionalCSharp
{
    using static Helpers;

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
}
