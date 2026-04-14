using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Bootstrap;
using MonoMod.RuntimeDetour;
using Newtonsoft.Json;
using RiskOfOptions.Core.Hooks;
using RoR2;
using UnityEngine;

namespace RiskOfOptions.API.Localization;

/// <summary>
/// Provides a localized string management system that runs in parallel with or as a fallback for R2API.Language.
/// Supports static strings, dynamic providers, and high-priority overrides via a tiered lookup system.
/// </summary>
public static class Localizer
{
    public const string GenericLanguage = "generic";
    public const string StringsLanguage = "strings";

    /// <summary>Internal storage for static localization tokens mapped by [Language][Token].</summary>
    private static readonly Dictionary<string, Dictionary<string, string>> Languages = [with(StringComparer.OrdinalIgnoreCase)];
    /// <summary>Internal storage for dynamic localization functions mapped by [Language][Token].</summary>
    private static readonly Dictionary<string, Dictionary<string, TextProvider>> Providers = [with(StringComparer.OrdinalIgnoreCase)];
    /// <summary>Internal storage for high-priority string overrides mapped by [Language][Token].</summary>
    private static readonly Dictionary<string, Dictionary<string, string>> Overrides = [with(StringComparer.OrdinalIgnoreCase)];

    private static bool _initialized;
    private static Hook? _language;

    /// <summary>
    /// Represents a method that attempts to provide a localized string dynamically at runtime.
    /// </summary>
    /// <param name="localized">When this method returns, contains the localized string if successful; otherwise, null.</param>
    /// <returns><see langword="true"/> if the provider successfully generated a string; otherwise, <see langword="false"/>.</returns>
    public delegate bool TextProvider(out string localized);

    /// <summary>
    /// Registers a single static localization token for the specified language.
    /// </summary>
    /// <param name="token">The unique identifier for the string.</param>
    /// <param name="text">The localized text to display.</param>
    /// <param name="language">The language name (e.g. "en", "zh"). Defaults to "generic".</param>
    /// <returns><see langword="true"/> if the token was successfully registered; otherwise, <see langword="false"/>.</returns>
    public static bool Add(string token, string text, string language = GenericLanguage) => !string.IsNullOrWhiteSpace(token) && !string.IsNullOrWhiteSpace(text) && GetInternal(Languages, language).TryAdd(token, text);

    /// <summary>
    /// Registers a collection of static localization tokens for the specified language.
    /// </summary>
    /// <param name="tokens">A dictionary where keys are tokens and values are localized strings.</param>
    /// <param name="language">The language name (e.g. "en", "zh"). Defaults to "generic".</param>
    /// <returns>The number of tokens successfully registered.</returns>
    public static int Add(IDictionary<string, string> tokens, string language = GenericLanguage)
    {
        if (tokens == null || tokens.Count <= 0) return 0;

        var localized = GetInternal(Languages, language);

        int added = 0;

        foreach (var (token, text) in tokens)
        {
            if (!string.IsNullOrWhiteSpace(token) && !string.IsNullOrWhiteSpace(text) && localized.TryAdd(token, text)) added++;
        }

        return added;
    }

    /// <summary>
    /// Registers a high-priority override for a token that takes precedence over providers and standard tokens.
    /// </summary>
    /// <param name="token">The unique identifier for the string to override.</param>
    /// <param name="text">The override text. If null, defaults to an empty string.</param>
    /// <param name="language">The language name (e.g. "en", "zh"). Defaults to "generic".</param>
    /// <returns><see langword="true"/> if the override was successfully registered; otherwise, <see langword="false"/>.</returns>
    public static bool AddOverride(string token, string text, string language = GenericLanguage) => !string.IsNullOrWhiteSpace(token) && GetInternal(Overrides, language).TryAdd(token, text ?? "");

    /// <summary>
    /// Registers a collection of high-priority overrides that take precedence over providers and standard tokens.
    /// </summary>
    /// <param name="tokens">A dictionary of tokens and their corresponding override text.</param>
    /// <param name="language">The language name (e.g. "en", "zh"). Defaults to "generic".</param>
    /// <returns>The number of overrides successfully registered.</returns>
    public static int AddOverrides(IDictionary<string, string> tokens, string language = GenericLanguage)
    {
        if (tokens == null || tokens.Count <= 0) return 0;

        var localized = GetInternal(Overrides, language);

        int added = 0;

        foreach(var (token, text) in tokens)
        {
            if (!string.IsNullOrWhiteSpace(token) && localized.TryAdd(token, text ?? "")) added++;
        }

        return added;
    }

    /// <summary>
    /// Registers a dynamic provider for a token, evaluated at runtime when requested.
    /// </summary>
    /// <param name="token">The unique identifier to associate with the provider.</param>
    /// <param name="provider">The delegate function that generates the text.</param>
    /// <param name="language">The language name. Defaults to "generic".</param>
    /// <returns><see langword="true"/> if the provider was successfully registered; otherwise, <see langword="false"/>.</returns>
    public static bool AddProvider(string token, TextProvider provider, string language = GenericLanguage) => !string.IsNullOrWhiteSpace(token) && provider != null && GetInternal(Providers, language).TryAdd(token, provider);

    /// <summary>
    /// Registers a collection of dynamic providers, evaluated at runtime when requested.
    /// </summary>
    /// <param name="providers">A dictionary of tokens and their corresponding <see cref="TextProvider"/> delegates.</param>
    /// <param name="language">The language name. Defaults to "generic".</param>
    /// <returns>The number of providers successfully registered.</returns>
    public static int AddProviders(IDictionary<string, TextProvider> providers, string language = GenericLanguage)
    {
        if (providers == null || providers.Count <= 0) return 0;

        var localized = GetInternal(Providers, language);

        int added = 0;

        foreach(var (token, provider) in providers)
        {
            if (!string.IsNullOrWhiteSpace(token) && provider != null && localized.TryAdd(token, provider)) added++;
        }

        return added;
    }

    /// <summary>
    /// Standardizes the language name and retrieves the corresponding sub-dictionary from the specified collection.
    /// </summary>
    /// <typeparam name="TValue">The type of the value stored in the language dictionary.</typeparam>
    /// <param name="languages">The master dictionary to search within.</param>
    /// <param name="language">The target language name.</param>
    /// <returns>The sub-dictionary for the specified language.</returns>
    private static Dictionary<string, TValue> GetInternal<TValue>(Dictionary<string, Dictionary<string, TValue>> languages, string language = GenericLanguage)
    {
        if (language == null || language == StringsLanguage) language = GenericLanguage;
        
        if (!languages.TryGetValue(language, out var localized))
        {
            localized = [with(StringComparer.OrdinalIgnoreCase)];
            languages[language] = localized;
        }

        return localized;
    }

    /// <summary>
    /// The detour hook for <see cref="Language.GetLocalizedStringByToken"/>.
    /// Implements tiered lookup: Overrides -> Providers -> Standard -> Original Game Logic.
    /// </summary>
    /// <param name="orig">The original method being hooked.</param>
    /// <param name="self">The instance of the Language class.</param>
    /// <param name="token">The localization token requested.</param>
    /// <returns>The localized string from the highest priority source, or the result of the original game logic.</returns>
    private static string GetLocalizedStringByToken(Func<Language, string, string> orig, Language self, string token)
    {
        if (string.IsNullOrWhiteSpace(token)) return orig(self, token);

        var language = self.name;
        if (language == StringsLanguage) language = GenericLanguage;

        // Local helper to perform the tiered search on a specific language bucket
        static bool TryGet(string language, string token, out string localized)
        {
            if (Overrides.TryGetValue(language, out var o) && o.TryGetValue(token, out localized)) return true;
            if (Providers.TryGetValue(language, out var p) && p.TryGetValue(token, out var provider) && provider(out localized)) return true;
            if (Languages.TryGetValue(language, out var l) && l.TryGetValue(token, out localized)) return true;

            localized = token;
            return false;
        }

        // Search the active language first
        if (TryGet(language, token, out string localized)) return localized;

        // If not found, fall back to the generic bucket
        if (language != GenericLanguage && TryGet(GenericLanguage, token, out string fallback)) return fallback;

        return orig(self, token);
    }

    /// <summary>
    /// Initializes the localization system, applies hooks, and loads local .language files.
    /// </summary>
    internal static void Initialize()
    {
        if (_initialized) return;

        _initialized = true;

        if (_language?.IsValid != true)
        {
            _language?.Dispose();
            _language = HookFactory.Create<Language>(nameof(Language.GetLocalizedStringByToken), HookFactory.GetMethod(typeof(Localizer), nameof(GetLocalizedStringByToken), BindingFlags.NonPublic | BindingFlags.Static));
        }

        LoadLanguageFiles();
    }

    /// <summary>
    /// Scans plugin directories for .language files and parses them into the static collection.
    /// </summary>
    /// <remarks>
    /// This process is skipped if R2API.Language is detected to prevent duplicate loading.
    /// </remarks>
    private static void LoadLanguageFiles()
    {
        // Exit early if R2API.Language is already handling this; otherwise, Risk of Options will handle this
        if (Chainloader.PluginInfos.ContainsKey("com.bepis.r2api.language")) return;

        string[] paths = Directory.GetFiles(Paths.PluginPath, "*.language", SearchOption.AllDirectories);

        if (paths.Length <= 0) return;

        foreach (var file in paths)
        {
            try
            {
                string content = File.ReadAllText(file);

                var languages = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(content);

                if (languages == null || languages.Count <= 0) continue;

                // Pre-process: Merge legacy 'strings' keys into the 'generic' bucket
                if (languages.TryGetValue(StringsLanguage, out var strings))
                {
                    if (languages.TryGetValue(GenericLanguage, out var generic))
                        foreach (var (token, text) in strings) generic.TryAdd(token, text);
                    else
                        languages[GenericLanguage] = strings;

                    languages.Remove(StringsLanguage);
                }

                if (languages.Remove("", out var ignored)) Debug.LogWarning($"[Risk of Options] [Localization] undefined language in file {file}: {ignored.Count} entries."); // Clean up malformed empty language keys

                foreach (var (language, tokens) in languages)
                {
                    if (!Languages.TryGetValue(language, out var localized))
                    {
                        localized = [with(StringComparer.OrdinalIgnoreCase)];
                        Languages[language] = localized;
                    }

                    foreach (var (token, text) in tokens)
                    {
                        if (string.IsNullOrWhiteSpace(token))
                        {
                            Debug.LogWarning($"[Risk of Options] [Localization] empty token in file: {file} ({language})");
                            continue;
                        }

                        if (string.IsNullOrWhiteSpace(text))
                        {
                            Debug.LogWarning($"[Risk of Options] [Localization] missing text for token '{token}' in file: {file} ({language})");
                            continue;
                        }

                        if (!localized.TryAdd(token, text)) Debug.LogWarning($"[Risk of Options] [Localization] duplicate token '{token}' in file: {file} ({language}).");
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"[Risk of Options] [Localization] failed to parse file '{file}': {e.Message}");
            }
        }
    }

    /// <summary>
    /// Removes a static localization token from the specified language.
    /// </summary>
    /// <param name="token">The token to remove.</param>
    /// <param name="language">The language name (e.g. "en", "zh"). If <see langword="null"/>, the operation will fail.</param>
    /// <returns><see langword="true"/> if the token was found and removed; otherwise, <see langword="false"/>.</returns>
    public static bool Remove(string token, string language = GenericLanguage)
    {
        if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(language)) return false;
        if (language == StringsLanguage) language = GenericLanguage;
        return Languages.TryGetValue(language, out var languages) && languages.Remove(token);
    }

    /// <summary>
    /// Removes a high-priority override from the specified language.
    /// </summary>
    /// <param name="token">The token to remove.</param>
    /// <param name="language">The language name (e.g. "en", "zh"). If <see langword="null"/>, the operation will fail.</param>
    /// <returns><see langword="true"/> if the override was found and removed; otherwise, <see langword="false"/>.</returns>
    public static bool RemoveOverride(string token, string language = GenericLanguage)
    {
        if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(language)) return false;
        if (language == StringsLanguage) language = GenericLanguage;
        return Overrides.TryGetValue(language, out var overrides) && overrides.Remove(token);
    }

    /// <summary>
    /// Removes a dynamic provider from the specified language.
    /// </summary>
    /// <param name="token">The token to remove.</param>
    /// <param name="language">The language name (e.g. "en", "zh"). If <see langword="null"/>, the operation will fail.</param>
    /// <returns><see langword="true"/> if the provider was found and removed; otherwise, <see langword="false"/>.</returns>
    public static bool RemoveProvider(string token, string language = GenericLanguage)
    {
        if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(language)) return false;
        if (language == StringsLanguage) language = GenericLanguage;
        return Providers.TryGetValue(language, out var providers) && providers.Remove(token);
    }

    /// <summary>
    /// Disables the system, removes hooks, and clears internal storage.
    /// </summary>
    internal static void Terminate()
    {
        if (!_initialized) return;

        _language?.Dispose();
        _language = null;

        Overrides.Clear();
        Providers.Clear();
        Languages.Clear();

        _initialized = false;
    }
}
