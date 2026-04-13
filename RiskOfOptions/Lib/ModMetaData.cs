using System;
using System.ComponentModel;
using RiskOfOptions.Extensions.BepInEx;

namespace RiskOfOptions.Lib;

/// <summary>
/// A lightweight container for BepInEx plugin metadata extracted from an assembly.
/// </summary>
[Obsolete($"This struct is deprecated and will be removed in a future version. Use {nameof(PluginMetadata)} instead.")]
[EditorBrowsable(EditorBrowsableState.Never)]
public struct ModMetaData
{
    /// <summary>The unique identifier (GUID) of the plugin.</summary>
    [Obsolete($"This field is deprecated and will be removed in a future version. Use {nameof(PluginMetadata.GUID)} instead.")]
    public string Guid;
    /// <summary>The display name of the plugin.</summary>
    [Obsolete($"This field is deprecated and will be removed in a future version. Use {nameof(PluginMetadata.Name)} instead.")]
    public string Name;
}
