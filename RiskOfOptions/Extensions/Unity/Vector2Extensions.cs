using UnityEngine;

using static RiskOfOptions.Utilities.MathUtils;

namespace RiskOfOptions.Extensions.Unity;

/// <summary>
/// Provides extension methods for the <see cref="Vector2"/> structure.
/// </summary>
public static class Vector2Extensions
{
    /// <summary>
    /// Determines whether the vector is approximately equal to the target vector
    /// by comparing the squared distance between them to a squared epsilon.
    /// </summary>
    /// <param name="vector">The source vector.</param>
    /// <param name="target">The vector to compare against.</param>
    /// <param name="epsilon">The maximum allowed distance between the vectors.</param>
    /// <returns><see langword="true"/> if the distance between vectors is less than epsilon; otherwise, <see langword="false"/>.</returns>
    /// <remarks>
    /// This uses a spherical/radial comparison, which is more performant than checking 
    /// the magnitude directly as it avoids a square root calculation.
    /// </remarks>
    public static bool IsApprox(this Vector2 vector, Vector2 target, float epsilon = Epsilon) => (vector - target).sqrMagnitude < (epsilon * epsilon);

    /// <summary>
    /// Interpolates between the current vector and a target using a SmoothStep function.
    /// </summary>
    /// <param name="vector">The starting vector.</param>
    /// <param name="target">The destination vector.</param>
    /// <param name="interpolation">The interpolation value (usually between 0 and 1).</param>
    /// <returns>A new Vector2 result of the SmoothStep interpolation.</returns>
    /// <remarks>
    /// Unlike standard Lerp, SmoothStep provides a gradual start and end (ease-in and ease-out),
    /// resulting in smoother motion.
    /// </remarks>
    public static Vector2 SmoothStepTo(this Vector2 vector, Vector2 target, float interpolation) => Vector2.LerpUnclamped(vector, target, Mathf.SmoothStep(0f, 1f, interpolation));
}
