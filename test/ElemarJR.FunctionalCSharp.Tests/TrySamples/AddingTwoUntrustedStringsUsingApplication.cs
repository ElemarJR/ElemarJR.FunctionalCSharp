using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ElemarJR.FunctionalCSharp.Tests.TrySamples
{
    public class AddingTwoUntrustedStringsUsingApplication
    {
        private static Try<string, double> ParseToDouble(Untrusted<string> source)
        {
            var result = 0d;
            return source.Validate(
                s => double.TryParse(s, out result),
                failure: s => $"Failed to parse '{s}' to double",
                success: _ => result
            );
        }

        public static Try<string, double> Add(Untrusted<string> fst, Untrusted<string> snd)
        {
            Func<double, double, double> add = (a, b) => a + b;
            return Try<string, Func<double, double, double>>.Of(add)
                .Apply(ParseToDouble(fst))   // Try<FailedToParseDoubleException, Func<double, double>>
                .Apply(ParseToDouble(snd));  // Try<FailedToParseDoubleException, double>
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
