using System;
using System.ComponentModel;
using BepInEx.Configuration;
using RiskOfOptions.Extensions.BepInEx;

namespace RiskOfOptions;

/// <summary>
/// Provides extension methods for <see cref="KeyboardShortcut"/>.
/// </summary>
[Obsolete($"This extension is deprecated and will be removed in a future version. All functionalities have been moved to the {nameof(Extensions.BepInEx)} namespace.")]
[EditorBrowsable(EditorBrowsableState.Never)]
public static class KeyboardShortcutExtensions
{
    /// <summary>
    /// Determines whether the specified keyboard shortcut is currently being held down.
    /// </summary>
    /// <param name="key">The keyboard shortcut to check.</param>
    /// <returns>
    /// True if both the main key and all associated modifier keys are currently pressed; 
    /// otherwise, false.
    /// </returns>
    /// <remarks>
    /// This method is "inclusive", meaning it returns true even if additional keys 
    /// outside of this shortcut are also being pressed.
    /// </remarks>
    [Obsolete($"This method is deprecated and will be removed in a future version. Use {nameof(InputExtensions.IsPressedInclusive)} instead.")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static bool IsPressedInclusive(this KeyboardShortcut key) => InputExtensions.IsPressedInclusive(key);
}
