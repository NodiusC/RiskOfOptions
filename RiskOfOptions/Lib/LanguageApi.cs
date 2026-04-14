using System;
using System.Collections.Generic;
using System.ComponentModel;
using MonoMod.RuntimeDetour;
using RiskOfOptions.API.Localization;
using RoR2;

namespace RiskOfOptions.Lib;

/// <summary>
/// Provides a localized string management system that supports static strings and dynamic providers.
/// </summary>
[Obsolete($"This class is deprecated and will be removed in a future version. Use {nameof(Localizer)} instead.")]
[EditorBrowsable(EditorBrowsableState.Never)]
internal static class LanguageApi
{
    /// <summary>Internal storage for static localization tokens mapped by [Token].</summary>
    [Obsolete($"This field is deprecated and will be removed in a future version.")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    private static readonly Dictionary<string, string> LanguageEntries = [];
    /// <summary>Internal storage for dynamic localization functions mapped by [Token].</summary>
    [Obsolete($"This field is deprecated and will be removed in a future version.")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    private static readonly Dictionary<string, LanguageStringDelegate> DynamicLanguageEntries = [];

    [Obsolete($"This field is deprecated and will be removed in a future version.")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    private static Hook? _languageHook;

    /// <summary>
    /// Represents a method that attempts to provide a localized string dynamically at runtime.
    /// </summary>
    /// <returns>The localized string.</returns>
    [Obsolete($"This delegate is deprecated and will be removed in a future version. Use {nameof(Localizer.TextProvider)} instead.")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal delegate string LanguageStringDelegate();

    /// <summary>
    /// Registers a single static localization token as "generic".
    /// </summary>
    /// <param name="token">The unique identifier for the string.</param>
    /// <param name="text">The localized text to display.</param>
    [Obsolete($"This method is deprecated and will be removed in a future version. Use {nameof(Localizer.Add)} instead.")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal static void Add(string token, string entry) => Localizer.Add(token, entry);

    /// <summary>
    /// Registers a dynamic provider for a token as "generic", evaluated at runtime when requested.
    /// </summary>
    /// <param name="token">The unique identifier to associate with the provider.</param>
    /// <param name="stringDelegate">The delegate function that generates the text.</param>
    [Obsolete($"This method is deprecated and will be removed in a future version. Use {nameof(Localizer.AddProvider)} instead.")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal static void AddDelegate(string token, LanguageStringDelegate stringDelegate) => Localizer.AddProvider(token, (out localized) => { localized = stringDelegate(); return true; });

    /// <summary>
    /// The detour hook for <see cref="Language.GetLocalizedStringByToken"/>.
    /// </summary>
    /// <param name="orig">The original method being hooked.</param>
    /// <param name="self">The instance of the Language class.</param>
    /// <param name="token">The localization token requested.</param>
    /// <returns>The localized string.</returns>
    [Obsolete($"This method is deprecated and will be removed in a future version.")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    private static string GetLocalizedStringByToken(Func<Language, string, string> orig, Language self, string token) => orig(self, token);

    /// <summary>
    /// Initializes the localization system, applies hooks, and loads local .language files.
    /// </summary>
    [Obsolete($"This method is deprecated and will be removed in a future version. Use {nameof(Localizer.Initialize)} instead.")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal static void Init() => Localizer.Initialize();

    /// <summary>
    /// Removes a dynamic provider from the specified language.
    /// </summary>
    /// <param name="token">The token to remove.</param>
    [Obsolete($"This method is deprecated and will be removed in a future version. Use {nameof(Localizer.RemoveProvider)} instead.")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal static void RemoveDelegate(string token) => Localizer.RemoveProvider(token);
}
