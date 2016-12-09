using System;

namespace ElemarJR.FunctionalCSharp
{
    using static Helpers;
    public struct Either<TLeft, TRight>
    {
        internal TLeft Left { get; }
        internal TRight Right { get; }

        public Option<TLeft> OptionalLeft => IsLeft ? Some(Left) : None;
        public Option<TRight> OptionalRight => IsRight ? Some(Right) : None;

        public bool IsLeft { get; }
        public bool IsRight => !IsLeft;

        private Either(TLeft left)
        {
            IsLeft = true;
            Left = left;
            Right = default(TRight);
        }

        private Either(TRight right)
        {
            IsLeft = false;
            Right = right;
            Left = default(TLeft);
        }

        public TResult Match<TResult>(Func<TLeft, TResult> left, Func<TRight, TResult> right)
            => IsLeft ? left(Left) : right(Right);

        public Unit Match(Action<TLeft> left, Action<TRight> right)
            => Match(ToFunc(left), ToFunc(right));

        public static implicit operator Either<TLeft, TRight>(TLeft left)
            => new Either<TLeft, TRight>(left);

        public static implicit operator Either<TLeft, TRight>(TRight right)
            => new Either<TLeft, TRight>(right);

        public static Either<TLeft, TRight> Of(TLeft left)
            => left;

        public static Either<TLeft, TRight> Of(TRight right)
            => right;
    }
}


