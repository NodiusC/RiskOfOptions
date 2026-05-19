using System;
using System.IO;
using BepInEx.Configuration;

namespace RiskOfOptions.API.Configuration;

/// <summary>
/// Represents a synchronized handle for an individual configuration setting, providing 
/// strongly-typed access, change tracking, and serialization capabilities.
/// </summary>
/// <typeparam name="T">The data type of the configuration value.</typeparam>
public interface Entry<T>
{
    /// <summary>Gets or sets the strongly-typed current value of the configuration.</summary>
    public T Value { get; set; }
    /// <summary>Fired when the setting is changed.</summary>
    public event EventHandler SettingChanged;

    /// <summary>Gets the section and key of this configuration.</summary>
    public ConfigDefinition Definition { get; }
    /// <summary>Gets the description of the setting shown to the user and other metadata.</summary>
    public ConfigDescription Description { get; }
    /// <summary>Gets the <see cref="Type"/> of the <see cref="Value" /> that this configuration holds.</summary>
    public Type SettingType { get; }

    /// <summary>Gets the default fallback value used when no user-defined value is present.</summary>
    public T DefaultValue { get; }

    /// <summary>
    /// Gets the serialized string representation of the setting's current value.
    /// </summary>
    /// <returns>A string formatted for storage in a configuration file.</returns>
    public string GetSerializedValue();

    /// <summary>
    /// Sets the value by parsing a serialized string representation.
    /// </summary>
    /// <param name="value">The raw string value to parse and apply.</param>
    public void SetSerializedValue(string value);

    /// <summary>
    /// Writes the formatted description and metadata of the setting to the specified stream.
    /// </summary>
    /// <param name="writer">The destination stream writer.</param>
    public void WriteDescription(StreamWriter writer);

    /// <summary>
    /// Gets the parent <see cref="Config"/> file instance that contains this entry.
    /// </summary>
    public Config ConfigFile { get; }
}
