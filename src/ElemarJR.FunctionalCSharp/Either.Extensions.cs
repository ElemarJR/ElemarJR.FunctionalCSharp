using System;

namespace ElemarJR.FunctionalCSharp
{
    public static class Either
    {
        public static Either<TLeft, TResult> Map<TLeft, TRight, TResult>(
            this Either<TLeft, TRight> @this,
            Func<TRight, TResult> func
        ) => @this.IsRight
            ? func(@this.Right)
            : (Either<TLeft, TResult>)@this.Left;

        public static Either<TLeft, TResult> Bind<TLeft, TRight, TResult>(
            this Either<TLeft, TRight> @this,
            Func<TRight, Either<TLeft, TResult>> func
        ) => @this.IsRight
            ? func(@this.Right)
            : (Either<TLeft, TResult>)@this.Left;
    }

}
