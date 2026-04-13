using UnityEngine;

namespace RiskOfOptions.Utilities;

using static MathUtils;

public static class ColorUtils
{
    /// <summary>
    /// The maximum value of an 8-bit color channel (255). 
    /// Used to scale normalized [0, 1] color components to standard byte values.
    /// </summary>
    public const float ByteMax = 255f;

    /// <summary>
    /// Creates a Unity <see cref="Color"/> from a 24-bit integer hex value and a normalized alpha value.
    /// </summary>
    /// <param name="hex">The RGB hex value represented as an integer (e.g. 0xFFFFFF).</param>
    /// <param name="alpha">The normalized alpha (opacity) component, ranging from 0 to 1.</param>
    /// <returns>A <see cref="Color"/> object with components scaled by <see cref="ByteMax"/>.</returns>
    public static Color FromRGBHex(int hex, float alpha = 1f) => new(((hex >> 16) & 0xFF) / ByteMax, ((hex >> 8) & 0xFF) / ByteMax, (hex & 0xFF) / ByteMax, alpha);

    /// <summary>
    /// Creates a Unity <see cref="Color"/> from HSV values using a radian-based hue.
    /// </summary>
    /// <param name="hue">The hue component expressed in radians (0 to 2π).</param>
    /// <param name="sat">The normalized saturation component (0 to 1).</param>
    /// <param name="val">The normalized value (brightness) component (0 to 1).</param>
    /// <returns>A <see cref="Color"/> converted from HSV to RGB space.</returns>
    public static Color FromHSV(float hue, float sat, float val) => Color.HSVToRGB(hue / Tau, sat, val);
}
