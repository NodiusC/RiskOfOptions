using RoR2;

namespace RiskOfOptions.API.Localization;

/// <summary>
/// Represents a container for localized string data and its associated identifier token.
/// </summary>
/// <param name="Text">The default text string fallback.</param>
/// <param name="Token">The unique localization token.</param>
public record struct LocalizedText(string Text, string Token = "")
{
    /// <summary>Gets or sets the default fallback text content.</summary>
    public string Text  { get; set => field = value ?? ""; } = Text;
    /// <summary>Gets or sets the token used for localization lookups.</summary>
    public string Token { get; set => field = string.IsNullOrWhiteSpace(value) ? "" : value; } = Token;

    /// <summary>
    /// Resolves the string value by checking the localization engine first, 
    /// falling back to the default text if no translation is found.
    /// </summary>
    /// <returns>The localized translation string; otherwise, the value of <see cref="Text"/>.</returns>
    public readonly string Get() => Token == "" ? Text : ((Language.GetString(Token) is var l && l != Token) ? l : Text);
}
