using System;
using BepInEx.Configuration;
using RiskOfOptions.API.Configuration;

namespace RiskOfOptions.Core.Configuration;

/// <summary>
/// A BepInEx-backed implementation of <see cref="Config"/> that wraps a <see cref="Content"/>.
/// </summary>
internal class BepInConfig : Config
{
    /// <summary>The underlying BepInEx configuration file being wrapped.</summary>
    private ConfigFile Content { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="BepInConfig"/> class.
    /// </summary>
    /// <param name="config">The native BepInEx configuration file to wrap.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="config"/> is null.</exception>
    internal BepInConfig(ConfigFile config)
    {
        Content = config ?? throw new ArgumentNullException($"{nameof(config)} cannot be null.");
    }

    /// <inheritdoc/>
    public Entry<T> Bind<T>(ConfigDefinition definition, T defaultValue, ConfigDescription description = null!)
    {
        ConfigEntry<T> entry = Content.Bind(definition, defaultValue, description);
        return new BepInEntry<T>(this, entry);
    }

    /// <inheritdoc/>
    public bool Equals(BepInConfig other) => Content.Equals(other?.Content);

    /// <inheritdoc/>
    public override bool Equals(object obj) => (obj is BepInConfig other) && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => Content.GetHashCode();

    /// <summary>Compares two config files for equality based on their underlying BepInEx configurations.</summary>
    public static bool operator ==(BepInConfig left, BepInConfig right) => left is null ? right is null : left.Equals(right);
    /// <summary>Compares two config files for inequality based on their underlying BepInEx configurations.</summary>
    public static bool operator !=(BepInConfig left, BepInConfig right) => !(left == right);

    /// <summary>Converts a <see cref="BepInConfig"/> wrapper implicitly to its underlying <see cref="ConfigFile"/>.</summary>
    /// <param name="config">The wrapper instance to convert.</param>
    public static implicit operator ConfigFile(BepInConfig config) => config.Content;
}
