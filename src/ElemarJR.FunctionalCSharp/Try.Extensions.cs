using System;
using System.Collections.Generic;
using System.Linq;

namespace ElemarJR.FunctionalCSharp
{
    public static class Try
    {
        #region Of
        public static Try<IEnumerable<TFailure>, Func<TA, TB, Try<IEnumerable<TFailure>, TSuccess>>> Of
            <TA, TB, TFailure, TSuccess>(
                Func<TA, TB, Try<IEnumerable<TFailure>, TSuccess>> func
            ) => func;

        public static Try<IEnumerable<TFailure>, Func<TA, TB, TC, Try<IEnumerable<TFailure>, TSuccess>>> Of
            <TA, TB, TC, TFailure, TSuccess>(
                Func<TA, TB, TC, Try<IEnumerable<TFailure>, TSuccess>> func
            ) => func;
        #endregion

        #region Lift

        public static Try<TFailure, TSuccess> Lift<TFailure, TSuccess>(
            this Try<TFailure, Try<TFailure, TSuccess>> @try
        ) => @try.Match(
            failure: f => f,
            success: s => s
        );
        #endregion

        #region Apply
        public static Try<TFailure, Func<TB, TResult>> Apply<TFailure, TA, TB, TResult>(
            this Try<TFailure, Func<TA, TB, TResult>> func, Try<TFailure, TA> arg
        )
        {
            return arg.Match(
                failure: e => e,
                success: a => func.Match(
                    failure: e2 => e2,
                    success: f => Try<TFailure, Func<TB, TResult>>.Of(b => f(a, b))
                )
            );
        }

        public static Try<TFailure, TResult> Apply<TFailure, TA, TResult>(
            this Try<TFailure, Func<TA, TResult>> func, Try<TFailure, TA> arg
        ) => arg.Match(
            failure: e => e,
            success: a => func.Match(
                failure: e2 => e2,
                success: f => Try<TFailure, TResult>.Of(f(a))
            )
        );

        public static Try<IEnumerable<TFailure>, Func<TB, TResult>> Apply<TFailure, TA, TB, TResult>(
            this Try<IEnumerable<TFailure>, Func<TA, TB, TResult>> func, Try<TFailure, TA> arg
        ) => arg.Match(
            failure: e => Try<IEnumerable<TFailure>, Func<TB, TResult>>.Of(
                func.OptionalFailure.GetOrElse(Enumerable.Empty<TFailure>).Concat(new[] { e })
            ),
            success: a => func.Match(
                failure: Try<IEnumerable<TFailure>, Func<TB, TResult>>.Of,
                success: f => Try<IEnumerable<TFailure>, Func<TB, TResult>>.Of(b => f(a, b))
            )
        );


        public static Try<IEnumerable<TFailure>, TResult> Apply<TFailure, TA, TResult>(
            this Try<IEnumerable<TFailure>, Func<TA, TResult>> func, Try<TFailure, TA> arg
        ) => arg.Match(
                failure: e => Try<IEnumerable<TFailure>, TResult>.Of(
                    func.OptionalFailure.GetOrElse(Enumerable.Empty<TFailure>).Concat(new[] { e })
                ),
                success: a => func.Match(
                    failure: Try<IEnumerable<TFailure>, TResult>.Of,
                    success: f => Try<IEnumerable<TFailure>, TResult>.Of(f(a))
                )
            );
        #endregion

        #region Map
        public static Try<TFailure, NewTSuccess> Map<TFailure, TSuccess, NewTSuccess>(
                this Try<TFailure, TSuccess> @try,
                Func<TSuccess, NewTSuccess> func
            )
            => @try.IsSucess
                ? func(@try.Success)
                : Try<TFailure, NewTSuccess>.Of(@try.Failure);

        public static Try<TFailure, Func<TB, NewTSuccess>> Map<TFailure, TSuccess, TB, NewTSuccess>(
            this Try<TFailure, TSuccess> @this,
            Func<TSuccess, TB, NewTSuccess> func
        ) => @this.Map(func.Curry());
        #endregion

        #region Bind
        public static Try<TFailure, NewTSuccess> Bind<TFailure, TSuccess, NewTSuccess>(
                this Try<TFailure, TSuccess> @try,
                Func<TSuccess, Try<TFailure, NewTSuccess>> func
            )
            => @try.IsSucess
                ? func(@try.Success)
                : Try<TFailure, NewTSuccess>.Of(@try.Failure);

        public static Either<TFailure, TSuccess> ToEither<TFailure, TSuccess>(
            this Try<TFailure, TSuccess> @try
        ) => @try.Match<Either<TFailure, TSuccess>>(
            failure: f => f,
            success: s => s
        );
        #endregion
    }
}
