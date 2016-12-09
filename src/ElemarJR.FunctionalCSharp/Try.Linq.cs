namespace System.Linq
{
    using ElemarJR.FunctionalCSharp;
    public static partial class LinqExtensions
    {
        public static Try<TFailure, NewTSuccess> Select<TFailure, TSuccess, NewTSuccess>(
                this Try<TFailure, TSuccess> @this,
                Func<TSuccess, NewTSuccess> func)
            => @this.Map(func);

        public static Try<TFailure, NewTSuccess> SelectMany<TFailure, TSuccess, R, NewTSuccess>(
            this Try<TFailure, TSuccess> @this,
            Func<TSuccess, Try<TFailure, R>> bind,
            Func<TSuccess, R, NewTSuccess> projector
        ) => @this.Match(
            failure: Try<TFailure, NewTSuccess>.Of,
            success: o => @this.Bind(bind).Match(
                failure: Try<TFailure, NewTSuccess>.Of,
                success: projected => Try<TFailure, NewTSuccess>.Of(projector(o, projected)))
        );
    }
}
