using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BepInEx;

namespace RiskOfOptions.Extensions.BepInEx;

/// <summary>
/// Provides extension methods for <see cref="Assembly"/> objects to facilitate plugin discovery and metadata retrieval.
/// </summary>
public static class PluginExtensions
{
    /// <summary>Internal cache to store results of assembly scans, including failed lookups (null).</summary>
    private static readonly Dictionary<Assembly, PluginMetadata?> _cache = [];

    /// <summary>
    /// Attempts to retrieve the <see cref="BepInPlugin"/> metadata from the specified assembly.
    /// </summary>
    /// <param name="assembly">The assembly to scan.</param>
    /// <param name="metadata">When this method returns, a <see cref="PluginMetadata"/> containing the metedata if found; otherwise, the default value.</param>
    /// <returns><see langword="true"/> if a <see cref="BepInPlugin"/> attribute was found; otherwise, <see langword="false"/>.</returns>
    /// <remarks>
    /// <para>This method is <b>not thread-safe</b> and should be called from the main thread or within a synchronized context.</para>
    /// <para>All results, including failed lookups, are cached internally to prevent redundant reflection costs.</para>
    /// <para>Results, including failed lookups, are cached to prevent redundant reflection costs after the first attempt.</para>
    /// </remarks>
    public static bool TryGetPlugin(this Assembly assembly, out PluginMetadata metadata)
    {
        // Check if this assembly had been scanned before
        if (_cache.TryGetValue(assembly, out PluginMetadata? cache))
        {
            metadata = cache ?? default;
            return cache.HasValue;
        }

        if (TryScanAssembly(assembly, out metadata))
        {
            _cache[assembly] = metadata;
            return true;
        }

        _cache[assembly] = null; // Cache the failure so this assembly won't be scanned again
        return false;
    }

    /// <summary>
    /// Performs the reflection scan on the assembly's types.
    /// </summary>
    /// <param name="assembly">The assembly to scan.</param>
    /// <param name="metadata">When this method returns, a <see cref="PluginMetadata"/> containing the metedata if found; otherwise, the default value.</param>
    /// <returns><see langword="true"/> if a <see cref="BepInPlugin"/> attribute was found; otherwise, <see langword="false"/>.</returns>
    private static bool TryScanAssembly(Assembly assembly, out PluginMetadata metadata)
    {
        try
        {
            return TryScanTypes(assembly.GetTypes(), out metadata);
        }
        catch (ReflectionTypeLoadException e)
        {
            return TryScanTypes(e.Types.Where(type => type != null), out metadata);
        }
        catch
        {
            metadata = default;
            return false;
        }
    }

    /// <summary>
    /// Iterates through a collection of types to find the first one decorated with <see cref="BepInPlugin"/>.
    /// </summary>
    /// <param name="types">The collection of types to scan</param>
    /// <param name="metadata">When this method returns, a <see cref="PluginMetadata"/> containing the metedata if found; otherwise, the default value.</param>
    /// <returns><see langword="true"/> if a <see cref="BepInPlugin"/> attribute was found; otherwise, <see langword="false"/>.</returns>
    private static bool TryScanTypes(IEnumerable<Type> types, out PluginMetadata metadata)
    {
        foreach (var type in types)
        {
            var attribute = type.GetCustomAttribute<BepInPlugin>();

            if (attribute != null)
            {
                metadata = new PluginMetadata(attribute.GUID, attribute.Name);
                return true;
            }
        }

        metadata = default;
        return false;
    }
}

/// <summary>
/// A lightweight container for BepInEx plugin metadata extracted from an assembly.
/// </summary>
/// <param name="GUID">The unique identifier (GUID) of the plugin.</param>
/// <param name="Name">The display name of the plugin.</param>
public readonly record struct PluginMetadata(string GUID, string Name);
