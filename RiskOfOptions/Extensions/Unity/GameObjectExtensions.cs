using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace RiskOfOptions.Extensions.Unity;

/// <summary>
/// Provides extension methods for <see cref="GameObject">GameObjects</see>.
/// </summary>
public static class GameObjectExtensions
{
    private static readonly Dictionary<Type, FieldInfo[]> _fields = [];
    private static readonly Dictionary<Type, PropertyInfo[]> _properties = [];

    private const BindingFlags Flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default;

    /// <summary>
    /// Adds a component of type <typeparamref name="T"/> to the <paramref name="gameObject"/> 
    /// and clones the state of the <paramref name="source"/> component into it.
    /// </summary>
    /// <typeparam name="T">The component type.</typeparam>
    /// <param name="gameObject">The target GameObject to receive the new component.</param>
    /// <param name="source">The source component to copy values from.</param>
    /// <returns>The newly created component with values applied from the source.</returns>
    /// <remarks>
    /// <para>This method uses reflection to perform a shallow copy of fields and properties. 
    /// Reflection data is cached internally to optimize performance on subsequent calls.</para>
    /// <para>Properties such as <c>name</c>, <c>tag</c>, and <c>hideFlags</c> are explicitly excluded.</para>
    /// <para><b>Caution:</b> If <paramref name="source"/> is null, the method falls back to <c>gameObject.AddComponent&lt;T&gt;()</c>. 
    /// This will fail if <typeparamref name="T"/> is an abstract class.</para>
    /// <para>Adapted From: https://answers.unity.com/questions/530178/how-to-get-a-component-from-an-object-and-add-it-t.html</para>
    /// </remarks>
    public static T AddComponentCopy<T>(this GameObject gameObject, T source) where T : Component
    {
        if (source == null) return gameObject.AddComponent<T>();

        // Use the specific type of the source to ensure the exact same class is created
        Type type = source.GetType();

        T component = (gameObject.AddComponent(type) as T)!;

        if (!_properties.TryGetValue(type, out PropertyInfo[] properties))
        {
            properties = type.GetProperties(Flags);
            _properties[type] = properties;
        }

        // Skip name, tag, and hideFlags as they affect the GameObject/Editor state
        foreach (PropertyInfo property in properties) if (property.CanWrite && property.Name != "name" && property.Name != "tag" && property.Name != "hideFlags")
            try { property.SetValue(component, property.GetValue(source, null), null); } catch { /* Ignore properties that throw NotImplemented or internal Unity errors */ }

        if (!_fields.TryGetValue(type, out FieldInfo[] fields))
        {
            fields = type.GetFields(Flags);
            _fields[type] = fields;
        }

        foreach (FieldInfo field in fields)
            field.SetValue(component, field.GetValue(source));

        return component;
    }

    /// <summary>
    /// Returns the component of type <typeparamref name="T"/> if the GameObject has one attached, 
    /// otherwise adds and returns a new component of that type.
    /// </summary>
    /// <typeparam name="T">The type of component to retrieve or add.</typeparam>
    /// <param name="gameObject">The GameObject to search or modify.</param>
    /// <returns>An existing or newly created component.</returns>
    public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component => gameObject.TryGetComponent(out T component) ? component : gameObject.AddComponent<T>();
}
