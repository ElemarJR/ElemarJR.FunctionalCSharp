using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ElemarJR.FunctionalCSharp.Tests.OptionSamples
{
    public class AddingTwoUntrustedStringsUsingApplication
    {
        private static Option<double> ParseToDouble(Untrusted<string> source)
        {
            var result = 0d;
            return source.Validate(
                s => double.TryParse(s, out result),
                success: _ => result
            );
        }

        public static Option<double> Add(Untrusted<string> fst, Untrusted<string> snd)
            => Option.Of<Func<double, double, double>>((a, b) => a + b)
                .Apply(ParseToDouble(fst))   // Option<Func<double, double>>
                .Apply(ParseToDouble(snd));  // Option<double>

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
