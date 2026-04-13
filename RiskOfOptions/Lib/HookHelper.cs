using System;
using System.ComponentModel;
using System.Reflection;
using MonoMod.RuntimeDetour;
using RiskOfOptions.Utilities;

namespace RiskOfOptions.Lib;

/// <summary>
/// Provides helper methods for creating method hooks and detours.
/// </summary>
[Obsolete($"This extension is deprecated and will be removed in a future version. All functionalities have been moved to {nameof(HookFactory)}.")]
[EditorBrowsable(EditorBrowsableState.Never)]
internal static class HookHelper
{
    private const BindingFlags TargetFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
    private const BindingFlags DestFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;

    /// <summary>
    /// Creates a new hook for an instance-based detour by resolving both methods via name and type metadata.
    /// </summary>
    /// <typeparam name="TTarget">The type containing the target method.</typeparam>
    /// <typeparam name="TDest">The type containing the detour method.</typeparam>
    /// <param name="targetMethodName">The name of the method to be hooked.</param>
    /// <param name="destMethodName">The name of the method that will redirect the original call.</param>
    /// <param name="instance">The object instance on which the detour method is invoked.</param>
    /// <returns>A new <see cref="Hook"/> instance.</returns>
    /// <exception cref="AmbiguousMatchException">Thrown when more than one method is found with the specified name and flags.</exception>
    /// <exception cref="MissingMethodException">Thrown when either method cannot be found on the specified types.</exception>
    [Obsolete($"This method is deprecated and will be removed in a future version. Use {nameof(HookFactory.Create)} instead.")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal static Hook NewHook<TTarget, TDest>(string targetMethodName, string destMethodName, TDest instance)
    {
        var targetMethod = typeof(TTarget).GetMethod(targetMethodName, TargetFlags);
        var destMethod = typeof(TDest).GetMethod(destMethodName, DestFlags);

        return new Hook(targetMethod, destMethod, instance);
    }

    /// <summary>
    /// Creates a new hook by resolving the target method via name and type metadata.
    /// </summary>
    /// <typeparam name="TTarget">The type containing the target method.</typeparam>
    /// <param name="targetMethodName">The name of the method to be hooked.</param>
    /// <param name="destMethod">The method that will redirect the original call.</param>
    /// <returns>A new <see cref="Hook"/> instance.</returns>
    /// <exception cref="AmbiguousMatchException">Thrown when more than one method is found with the specified name and flags.</exception>
    /// <exception cref="MissingMethodException">Thrown when the target method cannot be found on the specified type.</exception>
    [Obsolete($"This method is deprecated and will be removed in a future version. Use {nameof(HookFactory.Create)} instead.")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal static Hook NewHook<TTarget>(string targetMethodName, MethodInfo destMethod)
    {
        var targetMethod = typeof(TTarget).GetMethod(targetMethodName, TargetFlags);

        return new Hook(targetMethod, destMethod);
    }
}
