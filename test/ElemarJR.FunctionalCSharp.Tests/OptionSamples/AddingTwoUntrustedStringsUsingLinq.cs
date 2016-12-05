using System;
using System.Linq;

using Xunit;

namespace ElemarJR.FunctionalCSharp.Tests.OptionSamples
{
    public class AddingTwoUntrustedStringsUsingLinq
    {
        private static Option<double> ParseToDouble(Untrusted<string> source)
        {
            var result = 0d;
            return source.Validate(
                s => double.TryParse(s, out result),
                onSuccess: _ => result
            );
        }

        public static Option<double> Add(
            Untrusted<string> fst,
            Untrusted<string> snd
            ) =>
                from f in ParseToDouble(fst)
                from s in ParseToDouble(snd)
                select f + s;

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
