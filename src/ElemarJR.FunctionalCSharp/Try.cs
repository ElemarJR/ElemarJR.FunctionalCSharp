using System;
using System.Collections.Generic;
using System.Linq;

namespace ElemarJR.FunctionalCSharp
{
    using static Helpers;

    public struct Try<TFailure, TSuccess>
    {
        internal TFailure Failure { get; }
        internal TSuccess Success { get; }

        public bool IsFailure { get; }
        public bool IsSucess => !IsFailure;

        public Option<TFailure> OptionalFailure
            => IsFailure ? Some(Failure) : None;

        public Option<TSuccess> OptionalSuccess
            => IsSucess ? Some(Success) : None;

        internal Try(TFailure failure)
        {
            IsFailure = true;
            Failure = failure;
            Success = default(TSuccess);
        }

        internal Try(TSuccess success)
        {
            IsFailure = false;
            Failure = default(TFailure);
            Success = success;
        }

        public TResult Match<TResult>(
                Func<TFailure, TResult> failure,
                Func<TSuccess, TResult> success
            )
            => IsFailure ? failure(Failure) : success(Success);

        public Unit Match(
                Action<TFailure> failure,
                Action<TSuccess> success
            )
            => Match(ToFunc(failure), ToFunc(success));


        public static implicit operator Try<TFailure, TSuccess>(TFailure failure)
            => new Try<TFailure, TSuccess>(failure);

        public static implicit operator Try<TFailure, TSuccess>(TSuccess success)
            => new Try<TFailure, TSuccess>(success);

        public static Try<TFailure, TSuccess> Of(TSuccess obj) => obj;
        public static Try<TFailure, TSuccess> Of(TFailure obj) => obj;
    }

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
    }
}

namespace System.Linq
{
    using ElemarJR.FunctionalCSharp;
    public static partial class LinqExtensions
    {
        public static Try<TFailure, RR> Select<TFailure, TSuccess, RR>(
                this Try<TFailure, TSuccess> @this,
                Func<TSuccess, RR> func)
            => @this.Map(func);

        public static Try<TFailure, RR> SelectMany<TFailure, TSuccess, R, RR>(
            this Try<TFailure, TSuccess> @this,
            Func<TSuccess, Try<TFailure, R>> bind,
            Func<TSuccess, R, RR> projector
        ) => @this.Match(
            failure: Try<TFailure, RR>.Of,
            success: o => @this.Bind(bind).Match(
                failure: Try<TFailure, RR>.Of,
                success: projected => Try<TFailure, RR>.Of(projector(o, projected)))
        );
    }
}
