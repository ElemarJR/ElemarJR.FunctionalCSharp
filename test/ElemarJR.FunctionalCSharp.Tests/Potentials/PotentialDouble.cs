using System;
using System.Collections.Generic;
using System.Globalization;

namespace ElemarJR.FunctionalCSharp.Tests.Potentials
{
    public struct PotentialDouble
    {
        private readonly string _source;
        private readonly double _value;
        private readonly bool _isDouble;

        private PotentialDouble(string source, double value, bool isDouble)
        {
            _source = source;
            _value = value;
            _isDouble = isDouble;
        }

        public static implicit operator PotentialDouble(string source)
        {
            double d;
            return double.TryParse(source, NumberStyles.Any, CultureInfo.InvariantCulture, out d)
                ? new PotentialDouble(source, d, true)
                : new PotentialDouble(source, 0, false);
        }

        public static implicit operator PotentialDouble(double value)
            => new PotentialDouble(null, value, true);

        public TResult Match<TResult>(
            Func<double, TResult> isDouble,
            Func<string, TResult> notIsDouble
        ) => _isDouble
            ? isDouble(_value)
            : notIsDouble(_source);

        public Try<IEnumerable<string>, double> ToDouble()
            => Match<Try<IEnumerable<string>, double>>(
                isDouble: d => d,
                notIsDouble: source => new [] { $"Failed to parse '{source}' to double." }
            );

        public static PotentialDouble Of(string value)
            => value;
    }
}