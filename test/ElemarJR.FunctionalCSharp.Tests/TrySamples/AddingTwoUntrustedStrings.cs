using System;
using Xunit;

using ElemarJR.FunctionalCSharp;

namespace ElemarJR.FunctionalCSharp.Tests.TrySamples
{
    public class AddingTwoUntrustedStrings
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

        [Theory]
        [InlineData("5", 5d, null)]
        [InlineData("a", 0d, "Failed to parse 'a' to double")]
        public void ParseToDoubleReturnsDoubleOrAnError(string input, double result, string error)
        {
            var success = false;
            ParseToDouble(input).Match(
                failure: e => success = (e == error),
                success: v => success = Math.Abs(v - result) < 0.0001
                );
            Assert.True(success);
        }

        public static Try<string, double> Add(Untrusted<string> fst, Untrusted<string> snd)
            => ParseToDouble(fst).Match(
                success: f => ParseToDouble(snd).Match<Try<string, double>>(
                    success: s => f + s,
                    failure: e => e
                ),
                failure: e => e
            );

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


        public static Try<string, double> Add2(Untrusted<string> fst, Untrusted<string> snd)
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
        public void Add2_WhenBothParametersAreValidResultsSuccess(string a, string b, double result, string error)
        {
            var success = false;
            Add2(a, b).Match(
                failure: e => success = (e == error),
                success: v => success = Math.Abs(v - result) < 0.0001
                );
            Assert.True(success);
        }
    }
}
