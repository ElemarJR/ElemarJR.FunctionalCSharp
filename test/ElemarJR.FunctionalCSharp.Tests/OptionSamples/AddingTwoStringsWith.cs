using System;
using Xunit;

namespace ElemarJR.FunctionalCSharp.Tests.OptionSamples
{
    using static Helpers;
    public class AddingTwoStrings
    {
        public static Option<double> Add(string fst, string snd)
        {
            double f, s;

            if (!double.TryParse(fst, out f))
                return None;

            if (!double.TryParse(snd, out s))
                return None;

            return Some(f + s);
        }

        [Theory]
        [InlineData("5", "3", 8d)]
        [InlineData("a", "3", 0d)]
        [InlineData("5", "b", 0d)]
        public void AddScenario(string a, string b, double result)
        {
            var success = false;
            Add(a, b).Match(
                none: () => success = (result == 0d),
                some: v => success = Math.Abs(v - result) < 0.0001
                );
            Assert.True(success);
        }
    }
}
