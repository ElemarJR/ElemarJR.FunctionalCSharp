using System;
using System.Collections.Generic;
using System.Linq;

namespace ElemarJR.FunctionalCSharp
{
    public static class Try
    {
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
                success: f => Try<TFailure, TResult>.Of((TResult)f(a))
            )
        );

        public static Try<IEnumerable<TFailure>, Func<TB, TResult>> Apply<TFailure, TA, TB, TResult>(
            this Try<IEnumerable<TFailure>, Func<TA, TB, TResult>> func, Try<TFailure, TA> arg
        )
        {
            return arg.Match(
                failure: e => Try<IEnumerable<TFailure>, Func<TB, TResult>>.Of(
                    func.OptionalFailure.GetOrElse(Enumerable.Empty<TFailure>).Concat(new[] { e })
                ),
                success: a => func.Match(
                    failure: Try<IEnumerable<TFailure>, Func<TB, TResult>>.Of,
                    success: f => Try<IEnumerable<TFailure>, Func<TB, TResult>>.Of(b => f(a, b))
                )
            );
        }

        public static Try<IEnumerable<TFailure>, TResult> Apply<TFailure, TA, TResult>(
            this Try<IEnumerable<TFailure>, Func<TA, TResult>> func, Try<TFailure, TA> arg
        )
        {
            return arg.Match(
                failure: e => Try<IEnumerable<TFailure>, TResult>.Of(
                    func.OptionalFailure.GetOrElse(Enumerable.Empty<TFailure>).Concat(new[] { e })
                ),
                success: a => func.Match(
                    failure: Try<IEnumerable<TFailure>, TResult>.Of,
                    success: f => Try<IEnumerable<TFailure>, TResult>.Of(f(a))
                )
            );
        }
        public static Try<TFailure, RR> Map<TFailure, TSuccess, RR>(
                this Try<TFailure, TSuccess> @try,
                Func<TSuccess, RR> func
            )
            => @try.IsSucess
                ? func(@try.Success)
                : Try<TFailure, RR>.Of(@try.Failure);

        public static Try<TFailure, RR> Bind<TFailure, TSuccess, RR>(
                this Try<TFailure, TSuccess> @try,
                Func<TSuccess, Try<TFailure, RR>> func
            )
            => @try.IsSucess
                ? func(@try.Success)
                : Try<TFailure, RR>.Of(@try.Failure);

        public static Either<TFailure, TSuccess> ToEither<TFailure, TSuccess>(
            this Try<TFailure, TSuccess> @try
        ) => @try.Match<Either<TFailure, TSuccess>>(
            failure: f => f,
            success: s => s
        );
    }
}
