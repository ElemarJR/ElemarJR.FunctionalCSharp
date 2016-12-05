using System;
using Xunit;

namespace ElemarJR.FunctionalCSharp.Tests.TrySamples
{
    using static Helpers;
    public class AddingTwoStrings
    {
        public static Try<string, double> Add(string fst, string snd)
        {
            double f, s;

            if (!double.TryParse(fst, out f))
                return $"Failed to parse '{fst}' to double";

            if (!double.TryParse(snd, out s))
                return $"Failed to parse '{snd}' to double";

            return f + s;
        }

        [Theory]
        [InlineData("5", "3", 8d, null)]
        [InlineData("a", "3", 0d, "Failed to parse 'a' to double")]
        [InlineData("5", "b", 0d, "Failed to parse 'b' to double")]
        public void AddScenario(string a, string b, double result, string error)
        {
            var success = false;
            Add(a, b).Match(
                failure: e => success = (e == error),
                success: v => success = Math.Abs(v - result) < 0.0001
                );
            Assert.True(success);
        }
    }
}
