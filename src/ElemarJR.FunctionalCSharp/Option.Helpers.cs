namespace ElemarJR.FunctionalCSharp
{
    public struct NoneType { }

    public static partial class Helpers
    {
        public static Option<T> Some<T>(T value) => Option.Of(value);
        public static readonly NoneType None = new NoneType();
    }
}
