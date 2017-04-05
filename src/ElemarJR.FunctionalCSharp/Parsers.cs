using System;
using System.Globalization;

namespace ElemarJR.FunctionalCSharp
{
    using static Helpers;

    public static class Float
    {
        public static Option<float> Parse(string s) =>
            float.TryParse(s, out float result) ? Some(result) : None;

        public static Option<float> Parse(NumberStyles styles, IFormatProvider format, string s) =>
            float.TryParse(s, styles, format, out float result) ? Some(result) : None;
    }

    public static class Double
    {
        public static Option<double> Parse(string s) =>
            double.TryParse(s, out double result) ? Some(result) : None;

        public static Option<double> Parse(NumberStyles styles, IFormatProvider format, string s) =>
            double.TryParse(s, styles, format, out double result) ? Some(result) : None;
    }

    public static class Int
    {
        public static Option<int> Parse(string s) =>
            int.TryParse(s, out int result) ? Some(result) : None;

        public static Option<int> Parse(NumberStyles styles, IFormatProvider format, string s) =>
            int.TryParse(s, styles, format, out int result) ? Some(result) : None;
    }

    public static class Long
    {
        public static Option<long> Parse(string s) =>
            long.TryParse(s, out long result) ? Some(result) : None;

        public static Option<long> Parse(NumberStyles styles, IFormatProvider format, string s) =>
            long.TryParse(s, styles, format, out long result) ? Some(result) : None;
    }

    public static class Decimal
    {
        public static Option<decimal> Parse(string s) =>
            decimal.TryParse(s, out decimal result) ? Some(result) : None;

        public static Option<decimal> Parse(NumberStyles styles, IFormatProvider format, string s) =>
            decimal.TryParse(s, styles, format, out decimal result) ? Some(result) : None;

    }

}
