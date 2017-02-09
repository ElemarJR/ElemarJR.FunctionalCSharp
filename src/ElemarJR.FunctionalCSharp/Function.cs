using System;

namespace ElemarJR.FunctionalCSharp
{
    public static class Function
    {
        public static Func<TResult> Map<TInput, TResult>(
            this Func<TInput> that,
            Func<TInput, TResult> g
        ) => () => g(that());

        public static Func<TResult> Bind<TInput, TResult>(
            this Func<TInput> that,
            Func<TInput, Func<TResult>>  g
        ) => () => g(that())();

    }
}