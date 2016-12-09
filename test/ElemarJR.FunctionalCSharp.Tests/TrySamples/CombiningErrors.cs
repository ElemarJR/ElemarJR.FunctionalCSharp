using System;
using System.Collections.Generic;
using System.Linq;
using ElemarJR.FunctionalCSharp.Tests.Potentials;
using Xunit;

namespace ElemarJR.FunctionalCSharp.Tests.TrySamples
{
    public class CombiningErrors
    {
        public Try<IEnumerable<string>, double> Divide(double a, double b)
        {
            if (Math.Abs(b) < 0.0001)
                return new[] {"Divisor could not be 0"};

            return a / b;
        }

        [Fact]
        public void CombiningResultsOfMultipleTries()
        {
            PotentialDouble a = "6";
            PotentialDouble b = "3";

            var tryA = a.ToDouble();
            var tryB = b.ToDouble();

            var result = Try.Of(new Func<double, double, Try<IEnumerable<string>, double>>(Divide))
                .Apply(tryA)
                .Apply(tryB)
                .Lift();
            
            Assert.Equal(
                2d,
                result.OptionalSuccess.GetOrElse(0)
            );
        }
    }
}
