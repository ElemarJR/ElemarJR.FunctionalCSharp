namespace System.Linq
{
    using ElemarJR.FunctionalCSharp;
        
    public static partial class LinqExtensions
    {
        public static PromiseOfTry<TResult> Select<TInput, TResult>(
            this PromiseOfTry<TInput> promise,
            Func<TInput, TResult> selector
        ) => promise.Map(selector);

        public static PromiseOfTry<NewTResult> SelectMany<TInput, TResult, NewTResult>(
            this PromiseOfTry<TInput> promise,
            Func<TInput, PromiseOfTry<TResult>> binder,
            Func<TInput, TResult, NewTResult> projector
        ) => () => promise.Run().Match(
            failure: exception => exception,
            success: obj => binder(obj).Run()
                .Match<Try<Exception, NewTResult>>(
                    failure: ex => ex,
                    success: o => projector(obj, o)
                )
        );
    }
}
