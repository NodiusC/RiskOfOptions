using System;
using System.ComponentModel;
using RiskOfOptions.Utilities;
using UnityEngine;

namespace RiskOfOptions.Lib;

/// <summary>
/// Provides extension methods for the <see cref="Color"/> structure.
/// </summary>
[Obsolete($"This extension is deprecated and will be removed in a future version. All functionalities have been moved to {nameof(Utilities)} and {nameof(Extensions.Unity)} namespaces.")]
[EditorBrowsable(EditorBrowsableState.Never)]
public static class ColorExtensions
{
    /// <summary>
    /// The maximum value of an 8-bit color channel (255). 
    /// Used to scale normalized [0, 1] color components to standard byte values.
    /// </summary>
    [Obsolete($"This field is deprecated and will be removed in a future version. Use {nameof(ColorUtils.ByteMax)} instead.")]
    private const float Scale = 255f;

    /// <summary>
    /// Creates a Unity <see cref="Color"/> from a 24-bit integer hex value.
    /// </summary>
    /// <param name="hex">The RGB hex value represented as an integer (e.g. 0xFFFFFF).</param>
    /// <returns>A <see cref="Color"/> object with components scaled by <see cref="ByteMax"/>.</returns>
    [Obsolete($"This method is deprecated and will be removed in a future version. Use {nameof(ColorUtils.FromRGBHex)} instead.")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Color FromRGBHex(this Color orig, int hex) => ColorUtils.FromRGBHex(hex);

    /// <summary>
    /// Converts the color to a 24-bit <see langword="int"/> RGB hexadecimal representation (0xRRGGBB).
    /// </summary>
    /// <param name="color">The source RGB color to convert.</param>
    /// <returns>A 24-bit <see langword="int"/> where the red, green, and blue channels occupy the high, middle, and low bytes respectively.</returns>
    /// <remarks>
    /// This method ignores the alpha channel. To include transparency, 
    /// use a 32-bit ARGB conversion instead.
    /// </remarks>
    [Obsolete($"This method is deprecated and will be removed in a future version. Use {nameof(Extensions.Unity.ColorExtensions.ToRGBHex)} instead.")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static int ToRGBHex(this Color orig) => Extensions.Unity.ColorExtensions.ToRGBHex(orig);

    /// <summary>
    /// Creates a Unity <see cref="Color"/> from HSV values using a radian-based hue.
    /// </summary>
    /// <param name="hue">The hue component expressed in radians (0 to 2π).</param>
    /// <param name="sat">The normalized saturation component (0 to 1).</param>
    /// <param name="val">The normalized value (brightness) component (0 to 1).</param>
    /// <returns>A <see cref="Color"/> converted from HSV to RGB space.</returns>
    [Obsolete($"This method is deprecated and will be removed in a future version. Use {nameof(ColorUtils.FromHSV)} instead.")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal static Color ColorFromHSV(float hue, float sat, float val) => ColorUtils.FromHSV(hue, sat, val);

    /// <summary>
    /// Converts an RGB color to the HSV (Hue, Saturation, Value) color space using radians for the hue component.
    /// </summary>
    /// <param name="orig">The source RGB color to convert.</param>
    /// <param name="inHue">The hue value (in radians) to return if the color is achromatic (saturation is zero).</param>
    /// <param name="outHue">When this method returns, contains the hue component of the color in radians [0, 2π].</param>
    /// <param name="saturation">When this method returns, contains the saturation component of the color [0, 1].</param>
    /// <param name="value">When this method returns, contains the brightness value component of the color [0, 1].</param>
    [Obsolete($"This method is deprecated and will be removed in a future version. Use {nameof(Extensions.Unity.ColorExtensions.ToHSVRadian)} instead.")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal static void ToHSV(this Color orig, float inHue, out float outHue, out float saturation, out float value) => Extensions.Unity.ColorExtensions.ToHSVRadian(orig, inHue, out outHue, out saturation, out value);
}
