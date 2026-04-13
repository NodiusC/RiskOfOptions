using System.Collections.Generic;
using UnityEngine;

using static RiskOfOptions.Utilities.ColorUtils;
using static RiskOfOptions.Utilities.MathUtils;

namespace RiskOfOptions.Extensions.Unity;

/// <summary>
/// Provides extension methods for the <see cref="Color"/> structure.
/// </summary>
public static class ColorExtensions
{
    /// <summary>
    /// Determines whether all colors in the collection are approximately equal to the target color.
    /// </summary>
    /// <param name="colors">The collection of colors to check.</param>
    /// <param name="target">The color to compare against.</param>
    /// <param name="epsilon">The maximum allowed difference between color channels.</param>
    /// <returns><see langword="true"/> if every color in the collection satisfies the <see cref="IsApprox"/> condition; otherwise, <see langword="false"/>.</returns>
    /// <remarks>
    /// This method returns true immediately if the collection is empty.
    /// </remarks>
    public static bool AllApprox(this IEnumerable<Color> colors, Color target, float epsilon = Epsilon)
    {
        foreach (Color color in colors)
            if (!color.IsApprox(target, epsilon)) return false;
        
        return true;
    }

    /// <summary>
    /// Determines whether the color is approximately equal to the target color
    /// by checking if the difference between each RGBA channel is within the specified epsilon.
    /// </summary>
    /// <param name="color">The source color to check.</param>
    /// <param name="target">The color to compare against.</param>
    /// <param name="epsilon">The maximum allowed difference between color channels.</param>
    /// <returns><see langword="true"/> if all channel differences are less than epsilon; otherwise, <see langword="false"/>.</returns>
    public static bool IsApprox(this Color color, Color target, float epsilon = Epsilon) =>
        Mathf.Abs(color.r - target.r) < epsilon &&
        Mathf.Abs(color.g - target.g) < epsilon &&
        Mathf.Abs(color.b - target.b) < epsilon &&
        Mathf.Abs(color.a - target.a) < epsilon;

    /// <summary>
    /// Converts an RGB color to the HSV (Hue, Saturation, Value) color space.
    /// </summary>
    /// <param name="color">The source RGB color to convert.</param>
    /// <param name="referenceHue">The hue value to return if the color is achromatic (saturation is zero).</param>
    /// <param name="hue">When this method returns, contains the hue component of the color in radians [0, 1].</param>
    /// <param name="sat">When this method returns, contains the saturation component of the color [0, 1].</param>
    /// <param name="val">When this method returns, contains the brightness value component of the color [0, 1].</param>
    public static void ToHSV(this Color color, float referenceHue, out float hue, out float sat, out float val)
    {
        Color.RGBToHSV(color, out float h, out float s, out float v);

        sat = s;
        val = v;
        hue = (s > 0) ? h : referenceHue; // If the color is gray (H=0), we return the referenceHue.
    }

    /// <summary>
    /// Converts an RGB color to the HSV (Hue, Saturation, Value) color space using radians for the hue component.
    /// </summary>
    /// <param name="color">The source RGB color to convert.</param>
    /// <param name="referenceHue">The hue value (in radians) to return if the color is achromatic (saturation is zero).</param>
    /// <param name="hue">When this method returns, contains the hue component of the color in radians [0, 2π].</param>
    /// <param name="sat">When this method returns, contains the saturation component of the color [0, 1].</param>
    /// <param name="val">When this method returns, contains the brightness value component of the color [0, 1].</param>
    public static void ToHSVRadian(this Color color, float referenceHue, out float hue, out float sat, out float val)
    {
        Color.RGBToHSV(color, out float h, out float s, out float v);

        sat = s;
        val = v;

        // If the color is gray (H=0), we return the referenceHue.
        // Otherwise, we convert the engine's 0-1 hue to radians.
        hue = (s > 0) ? h * Tau : referenceHue;
    }

    /// <summary>
    /// Converts the color to a 24-bit <see langword="int"/> RGB hexadecimal representation (0xRRGGBB).
    /// </summary>
    /// <param name="color">The source RGB color to convert.</param>
    /// <returns>A 24-bit <see langword="int"/> where the red, green, and blue channels occupy the high, middle, and low bytes respectively.</returns>
    /// <remarks>
    /// This method ignores the alpha channel. To include transparency, 
    /// use a 32-bit ARGB conversion instead.
    /// </remarks>
    public static int ToRGBHex(this Color color) => Mathf.RoundToInt(color.r * ByteMax) << 16 | Mathf.RoundToInt(color.g * ByteMax) << 8 | Mathf.RoundToInt(color.b * ByteMax);

    /// <summary>
    /// Converts the color to an RGB hexadecimal <see langword="string"/> (e.g. "FF00FF").
    /// </summary>
    /// <param name="color">The source color to convert.</param>
    /// <param name="includeHash">Whether to prefix the <see langword="string"/> with a # symbol.</param>
    /// <returns>A hexadecimal <see langword="string"/> representation of the color.</returns>
    public static string ToRGBHexString(this Color color, bool includeHash = false) => (includeHash ? "#" : "") + color.ToRGBHex().ToString("X6");
}
