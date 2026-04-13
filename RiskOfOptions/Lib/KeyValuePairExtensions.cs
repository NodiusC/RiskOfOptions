using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace RiskOfOptions.Lib;

/// <summary>
/// Provides extension methods for the <see cref="KeyValuePair"/> structure.
/// </summary>
[Obsolete($"This extension is deprecated and will be removed in a future version. Use native C# tuple deconstruction instead.")]
[EditorBrowsable(EditorBrowsableState.Never)]
internal static class KeyValuePairExtensions
{
    /// <summary>
    /// <strong>Deprecated.</strong> Use `<c>var (key, value) = pair</c>` instead.
    /// </summary>
    [Obsolete($"This method is deprecated and will be removed in a future version. Use native C# tuple deconstruction instead.")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal static void Deconstruct<TKey, TValue>(this KeyValuePair<TKey, TValue> keyValuePair, out TKey key, out TValue value)
    {
        key = keyValuePair.Key;
        value = keyValuePair.Value;
    } 
}
