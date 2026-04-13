using System;
using System.Reflection;
using MonoMod.RuntimeDetour;

namespace RiskOfOptions.Core.Hooks;

/// <summary>
/// Provides factory methods for creating and managing method hooks and detours.
/// </summary>
public static class HookFactory
{
    public const BindingFlags AnyTarget = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
    public const BindingFlags AnyDetour = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
    public const BindingFlags InstancedDetour = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

    /// <summary>
    /// Retrieves the <see cref="MethodInfo"/> for a specified method name on a given type, 
    /// optionally matching a specific parameter signature.
    /// </summary>
    /// <param name="type">The type to search for the method.</param>
    /// <param name="name">The name of the method to retrieve.</param>
    /// <param name="flags">The binding flags to use for the search.</param>
    /// <param name="parameters">An optional array of types representing the method's parameter signature. If null, searching is performed by name only.</param>
    /// <returns>The <see cref="MethodInfo"/> representing the method.</returns>
    /// <exception cref="AmbiguousMatchException">Thrown when more than one method is found with the specified name and flags.</exception>
    /// <exception cref="MissingMethodException">Thrown when no method matching the specified name and signature is found on the type.</exception>
    public static MethodInfo GetMethod(Type type, string name, BindingFlags flags, Type[]? parameters = null)
    {
        MethodInfo method = (parameters == null) ? type.GetMethod(name, flags) : type.GetMethod(name, flags, null, parameters, null);
        return method ?? throw new MissingMethodException($"Could not find method '{name}' with {parameters?.Length ?? 0} parameters on type '{type.Name}'.");
    }

    /// <summary>
    /// Retrieves the <see cref="MethodInfo"/> for a specified method name on a given type, 
    /// optionally matching a specific parameter signature.
    /// </summary>
    /// <typeparam name="T">The type to search for the method.</typeparam>
    /// <param name="name">The name of the method to retrieve.</param>
    /// <param name="flags">The binding flags to use for the search.</param>
    /// <param name="parameters">An optional array of types representing the method's parameter signature. If null, searching is performed by name only.</param>
    /// <returns>The <see cref="MethodInfo"/> representing the method.</returns>
    /// <exception cref="AmbiguousMatchException">Thrown when more than one method is found with the specified name and flags.</exception>
    /// <exception cref="MissingMethodException">Thrown when no method matching the specified name and signature is found on the type.</exception>
    public static MethodInfo GetMethod<T>(string name, BindingFlags flags, Type[]? parameters = null) => GetMethod(typeof(T), name, flags, parameters);

    /// <summary>
    /// Creates a new hook using the specified target and detour method metadata.
    /// </summary>
    /// <param name="target">The original method to be hooked.</param>
    /// <param name="detour">The method that will redirect the original call.</param>
    /// <returns>A new <see cref="Hook"/> instance.</returns>
    /// <exception cref="ArgumentNullException">Thrown when either <paramref name="target"/> or <paramref name="detour"/> is null.</exception>
    public static Hook Create(MethodBase target, MethodInfo detour)
    {
        if (target == null) throw new ArgumentNullException(nameof(target));
        if (detour == null) throw new ArgumentNullException(nameof(detour));

        return new Hook(target, detour);
    }

    /// <summary>
    /// Creates a new hook by resolving the target method via name and type metadata.
    /// </summary>
    /// <param name="targetType">The type containing the target method.</param>
    /// <param name="targetName">The name of the method to be hooked.</param>
    /// <param name="detour">The method that will redirect the original call.</param>
    /// <param name="targetParams">An optional array of types representing the target method's parameter signature.</param>
    /// <returns>A new <see cref="Hook"/> instance.</returns>
    /// <exception cref="AmbiguousMatchException">Thrown when more than one method is found with the specified name and flags.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="detour"/> is null.</exception>
    /// <exception cref="MissingMethodException">Thrown when the target method cannot be found on the specified type.</exception>
    public static Hook Create(Type targetType, string targetName, MethodInfo detour, Type[]? targetParams = null) =>
        Create(GetMethod(targetType, targetName, AnyTarget, targetParams), detour);

    /// <summary>
    /// Creates a new hook by resolving the target method via name and type metadata.
    /// </summary>
    /// <typeparam name="TTarget">The type containing the target method.</typeparam>
    /// <param name="targetName">The name of the method to be hooked.</param>
    /// <param name="detour">The method that will redirect the original call.</param>
    /// <param name="targetParams">An optional array of types representing the target method's parameter signature.</param>
    /// <returns>A new <see cref="Hook"/> instance.</returns>
    /// <exception cref="AmbiguousMatchException">Thrown when more than one method is found with the specified name and flags.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="detour"/> is null.</exception>
    /// <exception cref="MissingMethodException">Thrown when the target method cannot be found on the specified type.</exception>
    public static Hook Create<TTarget>(string targetName, MethodInfo detour, Type[]? targetParams = null) where TTarget : class =>
        Create(typeof(TTarget), targetName, detour, targetParams);

    /// <summary>
    /// Creates a new hook by resolving the detour method via name and type metadata.
    /// </summary>
    /// <param name="target">The original method to be hooked.</param>
    /// <param name="detourType">The type containing the detour method.</param>
    /// <param name="detourName">The name of the method that will redirect the original call.</param>
    /// <param name="detourParams">An optional array of types representing the detour method's parameter signature.</param>
    /// <returns>A new <see cref="Hook"/> instance.</returns>
    /// <exception cref="AmbiguousMatchException">Thrown when more than one method is found with the specified name and flags.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="target"/> is null.</exception>
    /// <exception cref="MissingMethodException">Thrown when the detour method cannot be found on the specified type.</exception>
    public static Hook Create(MethodBase target, Type detourType, string detourName, Type[]? detourParams = null) =>
        Create(target, GetMethod(detourType, detourName, AnyDetour, detourParams));

    /// <summary>
    /// Creates a new hook by resolving the detour method via name and type metadata.
    /// </summary>
    /// <typeparam name="TDetour">The type containing the detour method.</typeparam>
    /// <param name="target">The original method to be hooked.</param>
    /// <param name="detourName">The name of the method that will redirect the original call.</param>
    /// <param name="detourParams">An optional array of types representing the detour method's parameter signature.</param>
    /// <returns>A new <see cref="Hook"/> instance.</returns>
    /// <exception cref="AmbiguousMatchException">Thrown when more than one method is found with the specified name and flags.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="target"/> is null.</exception>
    /// <exception cref="MissingMethodException">Thrown when the detour method cannot be found on the specified type.</exception>
    public static Hook Create<TDetour>(MethodBase target, string detourName, Type[]? detourParams = null) =>
        Create(target, typeof(TDetour), detourName, detourParams);

    /// <summary>
    /// Creates a new hook by resolving both target and detour methods via name and type metadata.
    /// </summary>
    /// <param name="targetType">The type containing the target method.</param>
    /// <param name="targetName">The name of the method to be hooked.</param>
    /// <param name="detourType">The type containing the detour method.</param>
    /// <param name="detourName">The name of the method that will redirect the original call.</param>
    /// <param name="targetParams">An optional array of types representing the target method's parameter signature.</param>
    /// <param name="detourParams">An optional array of types representing the detour method's parameter signature.</param>
    /// <returns>A new <see cref="Hook"/> instance.</returns>
    /// <exception cref="AmbiguousMatchException">Thrown when more than one method is found with the specified name and flags.</exception>
    /// <exception cref="MissingMethodException">Thrown when either method cannot be found on the specified types.</exception>
    public static Hook Create(Type targetType, string targetName, Type detourType, string detourName, Type[]? targetParams = null, Type[]? detourParams = null) =>
        Create(GetMethod(targetType, targetName, AnyTarget, targetParams), GetMethod(detourType, detourName, AnyDetour, detourParams));

    /// <summary>
    /// Creates a new hook by resolving both target and detour methods via name and type metadata.
    /// </summary>
    /// <typeparam name="TTarget">The type containing the target method.</typeparam>
    /// <typeparam name="TDetour">The type containing the detour method.</typeparam>
    /// <param name="targetName">The name of the method to be hooked.</param>
    /// <param name="detourName">The name of the method that will redirect the original call.</param>
    /// <param name="targetParams">An optional array of types representing the target method's parameter signature.</param>
    /// <param name="detourParams">An optional array of types representing the detour method's parameter signature.</param>
    /// <returns>A new <see cref="Hook"/> instance.</returns>
    /// <exception cref="AmbiguousMatchException">Thrown when more than one method is found with the specified name and flags.</exception>
    /// <exception cref="MissingMethodException">Thrown when either method cannot be found on the specified types.</exception>
    public static Hook Create<TTarget, TDetour>(string targetName, string detourName, Type[]? targetParams = null, Type[]? detourParams = null) where TTarget : class where TDetour : class =>
        Create(typeof(TTarget), targetName, typeof(TDetour), detourName, targetParams, detourParams);

    /// <summary>
    /// Creates a new hook for an instance-based detour using the specified target and detour method metadata.
    /// </summary>
    /// <typeparam name="TDetour">The type containing the detour method.</typeparam>
    /// <param name="target">The original method to be hooked.</param>
    /// <param name="detour">The method that will redirect the original call.</param>
    /// <param name="instance">The object instance on which the detour method is invoked.</param>
    /// <returns>A new <see cref="Hook"/> instance.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="target"/>, <paramref name="detour"/>, or <paramref name="instance"/> is null.</exception>
    public static Hook Create<TDetour>(MethodBase target, MethodInfo detour, TDetour instance) where TDetour : class
    {
        if (instance == null) throw new ArgumentNullException(nameof(instance));
        if (target == null) throw new ArgumentNullException(nameof(target));
        if (detour == null) throw new ArgumentNullException(nameof(detour));

        return new(target, detour, instance);
    }

    /// <summary>
    /// Creates a new hook for an instance-based detour by resolving the target method via name and type metadata.
    /// </summary>
    /// <typeparam name="TDetour">The type containing the detour method.</typeparam>
    /// <param name="targetType">The type containing the target method.</param>
    /// <param name="targetName">The name of the method to be hooked.</param>
    /// <param name="detour">The method that will redirect the original call.</param>
    /// <param name="instance">The object instance on which the detour method is invoked.</param>
    /// <param name="targetParams">An optional array of types representing the target method's parameter signature.</param>
    /// <returns>A new <see cref="Hook"/> instance.</returns>
    /// <exception cref="AmbiguousMatchException">Thrown when more than one method is found with the specified name and flags.</exception>
    /// <exception cref="ArgumentNullException">Thrown when either <paramref name="detour"/> or <paramref name="instance"/> is null.</exception>
    /// <exception cref="MissingMethodException">Thrown when the target method cannot be found on the specified type.</exception>
    public static Hook Create<TDetour>(Type targetType, string targetName, MethodInfo detour, TDetour instance, Type[]? targetParams = null) where TDetour : class =>
        Create(GetMethod(targetType, targetName, AnyTarget, targetParams), detour, instance);

    /// <summary>
    /// Creates a new hook for an instance-based detour by resolving the target method via name and type metadata.
    /// </summary>
    /// <typeparam name="TTarget">The type containing the target method.</typeparam>
    /// <typeparam name="TDetour">The type containing the detour method.</typeparam>
    /// <param name="targetName">The name of the method to be hooked.</param>
    /// <param name="detour">The method that will redirect the original call.</param>
    /// <param name="instance">The object instance on which the detour method is invoked.</param>
    /// <param name="targetParams">An optional array of types representing the target method's parameter signature.</param>
    /// <returns>A new <see cref="Hook"/> instance.</returns>
    /// <exception cref="AmbiguousMatchException">Thrown when more than one method is found with the specified name and flags.</exception>
    /// <exception cref="ArgumentNullException">Thrown when either <paramref name="detour"/> or <paramref name="instance"/> is null.</exception>
    /// <exception cref="MissingMethodException">Thrown when the target method cannot be found on the specified type.</exception>
    public static Hook Create<TTarget, TDetour>(string targetName, MethodInfo detour, TDetour instance, Type[]? targetParams = null) where TTarget : class where TDetour : class =>
        Create(GetMethod(typeof(TTarget), targetName, AnyTarget, targetParams), detour, instance);

    /// <summary>
    /// Creates a new hook for an instance-based detour by resolving the detour method via name and type metadata.
    /// </summary>
    /// <typeparam name="TDetour">The type containing the detour method.</typeparam>
    /// <param name="target">The original method to be hooked.</param>
    /// <param name="detourType">The type containing the detour method.</param>
    /// <param name="detourName">The name of the method that will redirect the original call.</param>
    /// <param name="instance">The object instance on which the detour method is invoked.</param>
    /// <param name="detourParams">An optional array of types representing the detour method's parameter signature.</param>
    /// <returns>A new <see cref="Hook"/> instance.</returns>
    /// <exception cref="AmbiguousMatchException">Thrown when more than one method is found with the specified name and flags.</exception>
    /// <exception cref="ArgumentNullException">Thrown when either <paramref name="target"/> or <paramref name="instance"/> is null.</exception>
    /// <exception cref="MissingMethodException">Thrown when the detour method cannot be found on the specified type.</exception>
    public static Hook Create<TDetour>(MethodBase target, string detourName, TDetour instance, Type[]? detourParams = null) where TDetour : class =>
        Create(target, GetMethod<TDetour>(detourName, InstancedDetour, detourParams), instance);

    /// <summary>
    /// Creates a new hook for an instance-based detour by resolving both methods via name and type metadata.
    /// </summary>
    /// <typeparam name="TDetour">The type containing the detour method.</typeparam>
    /// <param name="targetType">The type containing the target method.</param>
    /// <param name="targetName">The name of the method to be hooked.</param>
    /// <param name="detourName">The name of the method that will redirect the original call.</param>
    /// <param name="instance">The object instance on which the detour method is invoked.</param>
    /// <param name="targetParams">An optional array of types representing the target method's parameter signature.</param>
    /// <param name="detourParams">An optional array of types representing the detour method's parameter signature.</param>
    /// <returns>A new <see cref="Hook"/> instance.</returns>
    /// <exception cref="AmbiguousMatchException">Thrown when more than one method is found with the specified name and flags.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="instance"/> is null.</exception>
    /// <exception cref="MissingMethodException">Thrown when either method cannot be found on the specified types.</exception>
    public static Hook Create<TDetour>(Type targetType, string targetName, string detourName, TDetour instance, Type[]? targetParams = null, Type[]? detourParams = null) where TDetour : class =>
        Create(GetMethod(targetType, targetName, AnyTarget, targetParams), GetMethod<TDetour>(detourName, InstancedDetour, detourParams), instance);

    /// <summary>
    /// Creates a new hook for an instance-based detour by resolving both methods via name and type metadata.
    /// </summary>
    /// <typeparam name="TTarget">The type containing the target method.</typeparam>
    /// <typeparam name="TDetour">The type containing the detour method.</typeparam>
    /// <param name="targetName">The name of the method to be hooked.</param>
    /// <param name="detourName">The name of the method that will redirect the original call.</param>
    /// <param name="instance">The object instance on which the detour method is invoked.</param>
    /// <param name="targetParams">An optional array of types representing the target method's parameter signature.</param>
    /// <param name="detourParams">An optional array of types representing the detour method's parameter signature.</param>
    /// <returns>A new <see cref="Hook"/> instance.</returns>
    /// <exception cref="AmbiguousMatchException">Thrown when more than one method is found with the specified name and flags.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="instance"/> is null.</exception>
    /// <exception cref="MissingMethodException">Thrown when either method cannot be found on the specified types.</exception>
    public static Hook Create<TTarget, TDetour>(string targetName, string detourName, TDetour instance, Type[]? targetParams = null, Type[]? detourParams = null) where TTarget : class where TDetour : class =>
        Create(GetMethod<TTarget>(targetName, AnyTarget, targetParams), GetMethod<TDetour>(detourName, InstancedDetour, detourParams), instance);
}
