using System;

namespace ElemarJR.FunctionalCSharp
{
    using static Helpers;

    public struct Untrusted<T>
    {
        private readonly T _value;

        private Untrusted(T value)
        {
            _value = value;
        }

        public static implicit operator Untrusted<T>(T value)
            => new Untrusted<T>(value);

        public Try<TFailure, TSuccess> Validate<TFailure, TSuccess>(
            Func<T, bool> validation,
            Func<T, TFailure> failure,
            Func<T, TSuccess> success
        ) => validation(_value)
            ? success(_value)
            : (Try<TFailure, TSuccess>) failure(_value);

        public void Validate(
            Func<T, bool> validation,
            Action<T> failure,
            Action<T> success
        )
        {
            if (validation(_value)) success(_value); else failure(_value);
        } 

        public Option<TSuccess> Validate<TSuccess>(
            Func<T, bool> validation,
            Func<T, TSuccess> success
        ) => validation(_value)
            ? Some(success(_value))
            : None;

        public Option<T> Validate(
            Func<T, bool> validation
        ) => validation(_value)
            ? Some(_value)
            : None;

        public Try<TFailure, TSuccess> Validate<TFailure, TSuccess>(
            Func<T, bool> predicate,
            Func<T, TFailure> failure,
            Func<T, Try<TFailure, TSuccess>> success
        ) => predicate(_value)
            ? success(_value)
            : (Try<TFailure, TSuccess>)failure(_value);
    }
}
