using System;
using System.Linq;

using Xunit;

namespace ElemarJR.FunctionalCSharp.Tests.TrySamples
{
    public class AddingTwoUntrustedStringsUsingLinq
    {
        private static Try<string, double> ParseToDouble(Untrusted<string> source)
        {
            var result = 0d;
            return source.Validate(
                s => double.TryParse(s, out result),
                onFailure: s => $"Failed to parse '{s}' to double",
                onSuccess: _ => result
            );
        }

        
        public static Try<string, double> Add(
            Untrusted<string> fst,
            Untrusted<string> snd
            ) =>
                from f in ParseToDouble(fst)
                from s in ParseToDouble(snd)
                select f + s;

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
