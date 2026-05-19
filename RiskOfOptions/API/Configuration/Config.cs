using BepInEx.Configuration;

namespace RiskOfOptions.API.Configuration;

/// <summary>
/// Represents a configuration file that can contain multiple settings, allowing for the binding
/// of individual settings to <see cref="Entry{T}"/> instances for tracking and modification.
/// </summary>
public interface Config
{
    /// <summary>
    /// Binds a configuration setting to a specific definition, establishing a tracking entry for its value.
    /// </summary>
    /// <typeparam name="T">The data type of the configuration value.</typeparam>
    /// <param name="definition">The section and key of the setting.</param>
    /// <param name="defaultValue">The value used if no persisted or user-defined value is present.</param>
    /// <param name="description">The description of the setting shown to the user and other metadata.</param>
    /// <returns>An <see cref="Entry{T}"/> instance used to access, modify, and monitor the configuration.</returns>
    public Entry<T> Bind<T>(ConfigDefinition definition, T defaultValue, ConfigDescription description = null!);

    /// <summary>
    /// Binds a configuration setting to a specific section and key, establishing a tracking entry for its value.
    /// </summary>
    /// <typeparam name="T">The data type of the configuration value.</typeparam>
    /// <param name="section">The section of the setting.</param>
    /// <param name="key">The key of the setting.</param>
    /// <param name="defaultValue">The value used if no persisted or user-defined value is present.</param>
    /// <param name="description">The description of the setting shown to the user and other metadata.</param>
    /// <returns>An <see cref="Entry{T}"/> instance used to access, modify, and monitor the configuration.</returns>
    public Entry<T> Bind<T>(string section, string key, T defaultValue, ConfigDescription description = null!) => Bind(new ConfigDefinition(section, key), defaultValue, description);

    /// <summary>
    /// Binds a configuration setting to a specific section and key, establishing a tracking entry for its value.
    /// </summary>
    /// <typeparam name="T">The data type of the configuration value.</typeparam>
    /// <param name="section">The section of the setting.</param>
    /// <param name="key">The key of the setting.</param>
    /// <param name="defaultValue">The value used if no persisted or user-defined value is present.</param>
    /// <param name="description">The description of the setting shown to the user and other metadata.</param>
    /// <returns>An <see cref="Entry{T}"/> instance used to access, modify, and monitor the configuration.</returns>
    public Entry<T> Bind<T>(string section, string key, T defaultValue, string description) => Bind(new ConfigDefinition(section, key), defaultValue, new ConfigDescription(description));
}
