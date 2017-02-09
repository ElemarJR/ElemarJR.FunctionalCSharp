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
} 