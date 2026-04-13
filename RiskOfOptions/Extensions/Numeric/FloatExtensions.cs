using UnityEngine;

namespace RiskOfOptions.Extensions.Numeric;

/// <summary>
/// Provides extension methods for floats.
/// </summary>
public static class FloatExtensions
{
    /// <summary>
    /// Maps a value from a source range to a target range with support for inverted ranges and optional clamping.
    /// </summary>
    /// <param name="value">The current value to be remapped.</param>
    /// <param name="fromMin">The lower bound of the source range.</param>
    /// <param name="fromMax">The upper bound of the source range.</param>
    /// <param name="toMin">The lower bound of the target range.</param>
    /// <param name="toMax">The upper bound of the target range.</param>
    /// <param name="clamp">Determines if the output is constrained to the target range boundaries. Supports inverted ranges.</param>
    /// <returns>The value mapped to the target range. Returns <paramref name="toMin"/> if the source range is zero to prevent division by zero.</returns>
    /// <remarks>
    /// This is the safer remapping method. It handles division-by-zero internally and correctly 
    /// clamps even if the target range is inverted (e.g. remapping 0.5 from [0, 1] to [100, 0]).
    /// </remarks>
    public static float Rescale(this float value, float fromMin, float fromMax, float toMin, float toMax, bool clamp = true)
    {
        float range = fromMax - fromMin;

        // Check to avoid division by zero
        if (Mathf.Approximately(range, 0f)) return toMin;

        // Calculate the normalized 0-1 value
        float interpolant = (value - fromMin) / range;

        // Linear interpolation to the new range
        float result = toMin + interpolant * (toMax - toMin);

        if (clamp)
        {
            // Ensure boundaries are correctly ordered for Mathf.Clamp,
            // preparing for inverted target ranges (e.g., toMin: 100, toMax: 0).
            float min = Mathf.Min(toMin, toMax);
            float max = Mathf.Max(toMin, toMax);
            return Mathf.Clamp(result, min, max);
        }

        return result;
    }

    /// <summary>
    /// Maps a value from a source range to a target range using Unity's built-in interpolation.
    /// </summary>
    /// <param name="value">The current value to be remapped.</param>
    /// <param name="fromMin">The lower bound of the source range.</param>
    /// <param name="fromMax">The upper bound of the source range.</param>
    /// <param name="toMin">The lower bound of the target range.</param>
    /// <param name="toMax">The upper bound of the target range.</param>
    /// <returns>The value proportionally mapped and clamped to the new range.</returns>
    /// <remarks>
    /// The result is always clamped to the target range.
    /// This function may behave unexpectedly with inverted source ranges due to internal clamping in Mathf.InverseLerp.
    /// </remarks>
    public static float UnityRescale(this float value, float fromMin, float fromMax, float toMin, float toMax) => Mathf.Lerp(toMin, toMax, Mathf.InverseLerp(fromMin, fromMax, value));

    /// <summary>
    /// Maps a value from a source range to a target range without safety checks or clamping.
    /// </summary>
    /// <param name="value">The current value to be remapped.</param>
    /// <param name="fromMin">The lower bound of the source range.</param>
    /// <param name="fromMax">The upper bound of the source range.</param>
    /// <param name="toMin">The lower bound of the target range.</param>
    /// <param name="toMax">The upper bound of the target range.</param>
    /// <returns>The value proportionally mapped to the new range, but allows for values outside the target range.</returns>
    /// <remarks>
    /// This function performs no clamping; values outside the source range will result in extrapolation.
    /// It does not check for division by zero if fromMin equals fromMax approximately.
    /// </remarks>
    public static float UnsafeRescale(this float value, float fromMin, float fromMax, float toMin, float toMax) => toMin + (value - fromMin) * (toMax - toMin) / (fromMax - fromMin);
}
