using UnityEngine;

namespace RiskOfOptions.Utilities;

public static class MathUtils
{
    /// <summary>
    /// A standard tolerance value for floating-point comparisons.
    /// For equality checks where Unity's Mathf.Epsilon is too precise to account for minor rounding errors.
    /// </summary>
    public const float Epsilon = 0.0001f;

    /// <summary>
    /// Represents the mathematical constant Tau (2 * PI).
    /// For calculations involving full rotations or circles in radians.
    /// </summary>
    public const float Tau = Mathf.PI * 2f;
}
