using System;
using Xunit;

using ElemarJR.FunctionalCSharp;

namespace ElemarJR.FunctionalCSharp.Tests.OptionSamples
{
    public class AddingTwoUntrustedStrings
    {
        private static Option<double> ParseToDouble(Untrusted<string> source)
        {
            var result = 0d;
            return source.Validate(
                s => double.TryParse(s, out result),
                success: _ => result
            );
        }

        [Theory]
        [InlineData("5", 5d)]
        [InlineData("a", 0d)]
        public void ParseToDoubleReturnsOption(string input, double result)
        {
            var success = false;
            ParseToDouble(input).Match(
                none: () => success = (0d == result),
                some: v => success = Math.Abs(v - result) < 0.0001
                );
            Assert.True(success);
        }

        public static Option<double> Add(Untrusted<string> fst, Untrusted<string> snd)
            => ParseToDouble(fst).Bind(f => ParseToDouble(snd).Map(s => f + s));

        [Theory]
        [InlineData("5", "3", 8d)]
        [InlineData("a", "3", 0d)]
        [InlineData("5", "b", 0d)]
        public void AddScenario(string a, string b, double result)
        {
            var success = false;
            Add(a, b).Match(
                none: () => success = (0d == result),
                some: v => success = Math.Abs(v - result) < 0.0001
                );
            Assert.True(success);
        }
    }
}
