using BepInEx.Configuration;
using UnityEngine;

namespace RiskOfOptions.Extensions.BepInEx;

/// <summary>
/// Provides extension methods for input-related structures and classes, 
/// including <see cref="KeyboardShortcut"/> and hardware input types.
/// </summary>
public static class InputExtensions
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
    public static bool IsPressedInclusive(this KeyboardShortcut key)
    {
        if (!Input.GetKey(key.MainKey)) return false;

        foreach (KeyCode modifier in key.Modifiers)
            if (!Input.GetKey(modifier)) return false;

        return true;
    }
}
