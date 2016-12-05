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
            Func<T, TFailure> onFailure,
            Func<T, TSuccess> onSuccess
        ) => validation(_value)
            ? onSuccess(_value)
            : (Try<TFailure, TSuccess>) onFailure(_value);

        public Option<TSuccess> Validate<TSuccess>(
            Func<T, bool> validation,
            Func<T, TSuccess> onSuccess
        ) => validation(_value)
            ? Some(onSuccess(_value))
            : None;

        public Option<T> Validate(
            Func<T, bool> validation
        ) => validation(_value)
            ? Some(_value)
            : None;
    }
}
