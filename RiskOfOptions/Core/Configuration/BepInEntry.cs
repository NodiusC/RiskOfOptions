using System;
using System.IO;
using BepInEx.Configuration;
using RiskOfOptions.API.Configuration;

namespace RiskOfOptions.Core.Configuration;

/// <summary>
/// A BepInEx-backed implementation of <see cref="Entry{T}"/> that wraps a <see cref="ConfigEntry{T}"/>.
/// </summary>
/// <typeparam name="T">The data type of the configuration value.</typeparam>
internal class BepInEntry<T> : Entry<T>, IEquatable<BepInEntry<T>>
{
    /// <summary>The underlying BepInEx configuration entry being wrapped.</summary>
    private ConfigEntry<T> Content { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="BepInEntry{T}"/> class.
    /// </summary>
    /// <param name="config">The parent configuration file wrapper.</param>
    /// <param name="entry">The native BepInEx configuration entry to wrap.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="config"/> or <paramref name="entry"/> is null.</exception>
    internal BepInEntry(BepInConfig config, ConfigEntry<T> entry)
    {
        ConfigFile  = config ?? throw new ArgumentNullException(nameof(config));
        Content = entry  ?? throw new ArgumentNullException(nameof(entry));
    }

    /// <inheritdoc/>
    public T Value
    {
        get => Content.Value;
        set => Content.Value = value;
    }
    /// <inheritdoc/>
    public event EventHandler SettingChanged
    {
        add    => Content.SettingChanged += value;
        remove => Content.SettingChanged -= value;
    }

    /// <inheritdoc/>
    public ConfigDefinition  Definition  => Content.Definition;
    /// <inheritdoc/>
    public ConfigDescription Description => Content.Description;
    /// <inheritdoc/>
    public Type              SettingType => Content.SettingType;

    /// <inheritdoc/>
    public T DefaultValue => (T) (Description.AcceptableValues?.Clamp(Content.DefaultValue) ?? Content.DefaultValue);

    /// <inheritdoc/>
    public Config ConfigFile { get; init; }

    /// <inheritdoc/>
    public string GetSerializedValue() => Content.GetSerializedValue();

    /// <inheritdoc/>
    public void SetSerializedValue(string value) => Content.SetSerializedValue(value);

    /// <inheritdoc/>
    public void WriteDescription(StreamWriter writer) => Content.WriteDescription(writer);

    /// <inheritdoc/>
    public bool Equals(BepInEntry<T> other) => Content.Equals(other?.Content);

    /// <inheritdoc/>
    public override bool Equals(object obj) => (obj is BepInEntry<T> other) && Equals(other);

    /// <inheritdoc/>
    public override int GetHashCode() => Content.GetHashCode();

    /// <summary>Compares two entries for equality based on their underlying BepInEx configurations.</summary>
    public static bool operator ==(BepInEntry<T> left, BepInEntry<T> right) => left is null ? right is null : left.Equals(right);
    /// <summary>Compares two entries for inequality based on their underlying BepInEx configurations.</summary>
    public static bool operator !=(BepInEntry<T> left, BepInEntry<T> right) => !(left == right);

    /// <summary>Converts a <see cref="BepInEntry{T}"/> wrapper implicitly to its underlying <see cref="ConfigEntry{T}"/>.</summary>
    /// <param name="entry">The wrapper instance to convert.</param>
    public static implicit operator ConfigEntry<T>(BepInEntry<T> entry) => entry.Content;
}
