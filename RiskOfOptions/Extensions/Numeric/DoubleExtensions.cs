using System;

namespace RiskOfOptions.Extensions.Numeric;

/// <summary>
/// Provides extension methods for doubles.
/// </summary>
public static class DoubleExtensions
{
    /// <summary>
    /// Rounds the value up to the specified number of fractional digits.
    /// </summary>
    /// <param name="value">The double-precision floating-point number to round.</param>
    /// <param name="digits">The number of fractional digits in the return value.</param>
    /// <returns>
    /// The smallest number that is greater than or equal to <paramref name="value"/> 
    /// with the specified number of <paramref name="digits"/>.
    /// </returns>
    /// <remarks>
    /// This method uses <see cref="Math.Ceiling(double)"/> after scaling the value by a power of 10.
    /// </remarks>
    public static double RoundUp(this double value, int digits)
    {
        double multiplier = Math.Pow(10, digits);
        return Math.Ceiling(value * multiplier) / multiplier;
    }
}
