using System;

namespace ElemarJR.FunctionalCSharp
{
    public delegate Try<Exception, TResult> PromiseOfTry<TResult>();

    public static class PromiseOfTry
    {
        public static Try<Exception, TResult> Run<TResult>(this PromiseOfTry<TResult> promise)
        {
            try
            {
                return @promise();
            }
            catch (Exception e)
            {
                return e;
            }
        }

        public static PromiseOfTry<TResult> Map<TInput, TResult>(
            this PromiseOfTry<TInput> promise,
            Func<TInput, TResult> map
        ) => () => promise.Run()
            .Match<Try<Exception, TResult>>(
                failure: exception => exception,
                success: r => map(r)
            );

        public static PromiseOfTry<TResult> Bind<TInput, TResult>(
            this PromiseOfTry<TInput> promise,
            Func<TInput, PromiseOfTry<TResult>> f
        ) => () => promise.Run().Match(
            failure: ex => ex,
            success: t => f(t).Run()
        );
    }
}