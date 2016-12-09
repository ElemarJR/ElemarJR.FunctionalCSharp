using System;
using Xunit;

namespace ElemarJR.FunctionalCSharp.Tests.OptionSamples
{
    using static Helpers;
    public class BasicApplicatives
    {

        [Fact]
        public void AddingToOptionsWithValue()
        {
            Func<int, int, int> add = (a, b) => a + b;
            var optA = Some(2);
            var optB = Some(3);

            var result = optA.Map(add).Match(
                none: () => None,
                some: f => optB.Match(
                    none: () => None,
                    some: (b) => Some(f(b))
                ));

            Assert.Equal(Some(5), result);
        }

        [Fact]
        public void AddingToOptionsWithValue2()
        {
            Func<int, int, int> add = (a, b) => a + b;
            var optA = Some(2);
            var optB = Some(3);

            var result = optA.Map(add).Apply(optB);

            Assert.Equal(Some(5), result);
        }

        [Fact]
        public void AddingToOptionsWithValue3()
        {
            Func<int, int, int> add = (a, b) => a + b;
            var optA = Some(2);
            var optB = Some(3);

            var result = Some(add).Apply(optA).Apply(optB);

            Assert.Equal(Some(5), result);
        }


    }
}