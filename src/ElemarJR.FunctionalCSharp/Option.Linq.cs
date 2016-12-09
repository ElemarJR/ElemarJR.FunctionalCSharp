using ElemarJR.FunctionalCSharp;

namespace System.Linq
{
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
