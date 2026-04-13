using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using RiskOfOptions.Extensions.BepInEx;
using RiskOfOptions.Extensions.Numeric;
using RiskOfOptions.Extensions.Unity;
using RiskOfOptions.Lib;
using UnityEngine;

namespace RiskOfOptions;

[Obsolete($"This extension is deprecated and will be removed in a future version. All functionalities have been moved to the {nameof(Extensions)} namespace.")]
[EditorBrowsable(EditorBrowsableState.Never)]
internal static class ExtensionMethods
{
    /// <summary>
    /// Returns the absolute value of a double-precision floating-point number.
    /// </summary>
    /// <param name="num">A number that is greater than or equal to <see cref="Double.MinValue"/>, but less than or equal to <see cref="Double.MaxValue"/>.</param>
    /// <returns>A double-precision floating-point number, x, such that 0 ≤ x ≤ <see cref="Double.MaxValue"/>.</returns>
    [Obsolete($"This method is deprecated and will be removed in a future version. Use {nameof(Math.Abs)} instead.")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal static double Abs(double num) => Math.Abs(num);

    /// <summary>
    /// Adds a component of type <typeparamref name="T"/> to the <see cref="GameObject"/> 
    /// and clones the state of the <paramref name="toAdd"/> component into it.
    /// </summary>
    /// <typeparam name="T">The component type.</typeparam>
    /// <param name="go">The target GameObject to receive the new component.</param>
    /// <param name="toAdd">The source component to copy values from.</param>
    /// <returns>The newly created component with values applied from the source.</returns>
    /// <remarks>
    /// <para>This method uses reflection to perform a shallow copy of fields and properties. 
    /// Reflection data is cached internally to optimize performance on subsequent calls.</para>
    /// <para>Properties such as <c>name</c>, <c>tag</c>, and <c>hideFlags</c> are explicitly excluded.</para>
    /// <para><b>Caution:</b> If <paramref name="toAdd"/> is null, the method falls back to <c>gameObject.AddComponent&lt;T&gt;()</c>. 
    /// This will fail if <typeparamref name="T"/> is an abstract class.</para>
    /// <para>Adapted From: https://answers.unity.com/questions/530178/how-to-get-a-component-from-an-object-and-add-it-t.html</para>
    /// </remarks>
    [Obsolete($"This method is deprecated and will be removed in a future version. Use {nameof(GameObjectExtensions.AddComponentCopy)} instead.")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal static T AddComponent<T>(this GameObject go, T toAdd) where T : UnityEngine.Component => go.AddComponentCopy(toAdd);

    /// <summary>
    /// <inheritdoc cref="Extensions.Unity.ColorExtensions.IsApprox"/>
    /// </summary>
    /// <param name="a">The source color to check.</param>
    /// <param name="b">The color to compare against.</param>
    /// <returns><see langword="true"/> if all channel differences are less than epsilon; otherwise, <see langword="false"/>.</returns>
    [Obsolete($"This method is deprecated and will be removed in a future version. Use {nameof(Extensions.Unity.ColorExtensions.IsApprox)} instead.")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal static bool CloseEnough(Color a, Color b) => a.IsApprox(b);

    /// <summary>
    /// <inheritdoc cref="Extensions.Unity.ColorExtensions.AllApprox"/>
    /// </summary>
    /// <param name="a">The collection of colors to check.</param>
    /// <param name="b">The color to compare against.</param>
    /// <returns><see langword="true"/> if every color in the collection satisfies the <see cref="Extensions.Unity.ColorExtensions.IsApprox"/> condition; otherwise, <see langword="false"/>.</returns>
    /// <remarks>
    /// This method returns true immediately if the collection is empty.
    /// </remarks>
    [Obsolete($"This method is deprecated and will be removed in a future version. Use {nameof(Extensions.Unity.ColorExtensions.AllApprox)} instead.")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal static bool CloseEnough(Color[] a, Color b) => a.AllApprox(b);

    /// <summary>
    /// <inheritdoc cref="Vector2Extensions.IsApprox"/>
    /// </summary>
    /// <param name="a">The source vector.</param>
    /// <param name="b">The vector to compare against.</param>
    /// <returns><see langword="true"/> if the distance between vectors is less than epsilon; otherwise, <see langword="false"/>.</returns>
    /// <remarks>
    /// This uses a spherical/radial comparison, which is more performant than checking 
    /// the magnitude directly as it avoids a square root calculation.
    /// </remarks>
    [Obsolete($"This method is deprecated and will be removed in a future version. Use {nameof(Vector2Extensions.IsApprox)} instead.")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal static bool CloseEnough(Vector2 a, Vector2 b) => a.IsApprox(b);

    /// <summary>
    /// Performs a shallow clone of the <paramref name="toAdd"/> component into
    /// the component <paramref name="comp"/> using Reflection.
    /// </summary>
    /// <typeparam name="T">The component type.</typeparam>
    /// <param name="comp">The target component to copy into.</param>
    /// <param name="other">The source component to copy values from.</param>
    /// <returns>The newly created component with values applied from the source.</returns>
    /// <remarks>
    /// <para>Direct usage is discouraged in favor of the <c>AddComponentCopy</c> extension.</para>
    /// <para>Sourced From: https://answers.unity.com/questions/530178/how-to-get-a-component-from-an-object-and-add-it-t.html</para>
    /// </remarks>
    [Obsolete($"This method is deprecated and will be removed in a future version. The logic has been absorbed by {nameof(GameObjectExtensions.AddComponentCopy)}.")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal static T GetCopyOf<T>(this UnityEngine.Component comp, T other) where T : UnityEngine.Component
    {
        Type type = comp.GetType();
        if (type != other.GetType()) return null!; // type mis-match
        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default;
        PropertyInfo[] pinfos = type.GetProperties(flags);
        foreach (var pinfo in pinfos)
        {
            if (pinfo.CanWrite)
            {
                try
                {
                    pinfo.SetValue(comp, pinfo.GetValue(other, null), null);
                }
                catch
                {
                    // In case of NotImplementedException being thrown.
                }
            }
        }
        FieldInfo[] finfos = type.GetFields(flags);
        foreach (var finfo in finfos)
        {
            finfo.SetValue(comp, finfo.GetValue(other));
        }
        return (comp as T)!;
    }

    /// <summary>
    /// Attempts to retrieve BepInEx mod metadata from the specified assembly by scanning for the <see cref="BepInEx.BepInPlugin"/> attribute.
    /// </summary>
    /// <param name="assembly">The assembly to scan for mod types.</param>
    /// <returns>A <see cref="ModMetaData"/> representing the mod's metadata if found; otherwise, the default identity.</returns>
    [Obsolete($"This method is deprecated and will be removed in a future version. Use {nameof(PluginExtensions.TryGetPlugin)} instead.")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal static ModMetaData GetModMetaData(this Assembly assembly) => assembly.TryGetPlugin(out PluginMetadata info) ? new ModMetaData { Guid = info.GUID, Name = info.Name } : default;

    /// <summary>
    /// <inheritdoc cref="GameObjectExtensions.GetOrAddComponent"/>
    /// </summary>
    /// <typeparam name="T">The type of component to retrieve or add.</typeparam>
    /// <param name="gameObject">The GameObject to search or modify.</param>
    /// <returns>An existing or newly created component.</returns>
    [Obsolete($"This method is deprecated and will be removed in a future version. Use {nameof(GameObjectExtensions.GetOrAddComponent)} instead.")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal static T GetOrAddComponent<T>(this GameObject gameObject) where T : UnityEngine.Component => gameObject.GetOrAddComponent<T>();

    /// <summary>
    /// Attempts to retrieve all valid types from the specified assembly.
    /// </summary>
    /// <param name="assembly">The assembly to scan for types.</param>
    /// <returns>A collection of valid types if found; otherwise, an empty collection.</returns>
    /// <remarks>
    /// This method safely handles <see cref="ReflectionTypeLoadException"/>, which can occur if the assembly 
    /// has dependencies that cannot be resolved in the current environment.
    /// </remarks>
    [Obsolete($"This method is deprecated and will be removed in a future version. The logic has been absorbed by {nameof(PluginExtensions.TryGetPlugin)}.")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal static IEnumerable<Type> GetValidTypes(this Assembly assembly)
    {
        try
        {
            return assembly.GetTypes();
        }
        catch (ReflectionTypeLoadException e)
        {
            return e.Types.Where(type => type is not null);
        }
        catch (Exception)
        {
            return [];
        }
    }

    /// <summary>
    /// Maps a value from a source range to a target range with support for inverted ranges and optional clamping.
    /// </summary>
    /// <param name="value">The current value to be remapped.</param>
    /// <param name="fromMin">The lower bound of the source range.</param>
    /// <param name="fromMax">The upper bound of the source range.</param>
    /// <param name="toMin">The lower bound of the target range.</param>
    /// <param name="toMax">The upper bound of the target range.</param>
    /// <returns>The value proportionally mapped and clamped to the new range. Returns <paramref name="toMin"/> if the source range is zero to prevent division by zero.</returns>
    [Obsolete($"This method is deprecated and will be removed in a future version. Use {nameof(FloatExtensions.Rescale)} instead.")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal static float Remap(this float value, float fromMin, float fromMax, float toMin, float toMax) => value.Rescale(fromMin, fromMax, toMin, toMax, true);

    /// <summary>
    /// <inheritdoc cref="DoubleExtensions.RoundUp"/>
    /// </summary>
    /// <param name="num">The double-precision floating-point number to round.</param>
    /// <param name="place">The number of fractional digits in the return value.</param>
    /// <returns>
    /// The smallest number that is greater than or equal to <paramref name="num"/> 
    /// with the specified number of <paramref name="place"/>.
    /// </returns>
    /// <remarks>
    /// This method uses <see cref="Math.Ceiling(double)"/> after scaling the value by a power of 10.
    /// </remarks>
    [Obsolete($"This method is deprecated and will be removed in a future version. Use {nameof(DoubleExtensions.RoundUp)} instead.")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal static double RoundUpToDecimalPlace(this double num, int place) => num.RoundUp(place);

    /// <summary>
    /// <inheritdoc cref="Vector2Extensions.SmoothStepTo"/>
    /// </summary>
    /// <param name="a">The starting vector.</param>
    /// <param name="b">The destination vector.</param>
    /// <param name="t">The interpolation value (usually between 0 and 1).</param>
    /// <returns>A new Vector2 result of the SmoothStep interpolation.</returns>
    /// <remarks>
    /// Unlike standard Lerp, SmoothStep provides a gradual start and end (ease-in and ease-out),
    /// resulting in smoother motion.
    /// </remarks>
    [Obsolete($"This method is deprecated and will be removed in a future version. Use {nameof(Vector2Extensions.SmoothStepTo)} instead.")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal static Vector2 SmoothStep(Vector2 a, Vector2 b, float t) => a.SmoothStepTo(b, t);
}
