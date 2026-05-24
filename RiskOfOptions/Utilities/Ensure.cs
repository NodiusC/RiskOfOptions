using System;
using System.Runtime.CompilerServices;

namespace RiskOfOptions.Utilities;

/// <summary>
/// Provides static guard methods to enforce preconditions and validate arguments.
/// </summary>
public static class Ensure
{
    /// <summary>
    /// Throws an <see cref="ArgumentNullException"/> if the specified reference type value is null.
    /// </summary>
    /// <typeparam name="T">The reference type of the value to check.</typeparam>
    /// <param name="value">The value to validate for null.</param>
    /// <param name="expression">The expression passed as the <paramref name="value"/> argument.</param>
    /// <returns>The original <paramref name="value"/> if it is not null.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <remarks>The <paramref name="expression"/> argument is automatically populated by the compiler.</remarks>
    public static T NotNull<T>(T? value, [CallerArgumentExpression(nameof(value))] string? expression = null)
    where T : class => value ?? throw new ArgumentNullException(expression ?? "value");

    /// <summary>
    /// Throws an <see cref="ArgumentNullException"/> if the specified nullable value type is null.
    /// </summary>
    /// <typeparam name="T">The underlying value type of the nullable value to check.</typeparam>
    /// <param name="value">The nullable value to validate for null.</param>
    /// <param name="expression">The expression passed as the <paramref name="value"/> argument.</param>
    /// <returns>The underlying <typeparamref name="T"/> value if <paramref name="value"/> is not null.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <remarks>The <paramref name="expression"/> argument is automatically populated by the compiler.</remarks>
    public static T NotNull<T>(T? value, [CallerArgumentExpression(nameof(value))] string? expression = null)
    where T : struct => value ?? throw new ArgumentNullException(expression ?? "value");

    /// <summary>
    /// Throws an <see cref="ArgumentNullException"/> if the specified string is null, 
    /// or an <see cref="ArgumentException"/> if it is empty or consists only of whitespace characters.
    /// </summary>
    /// <param name="value">The string value to validate.</param>
    /// <param name="expression">The expression passed as the <paramref name="value"/> argument.</param>
    /// <returns>The original <paramref name="value"/> if it is not null, empty, or whitespace.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="value"/> is empty or contains only whitespace characters.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is null.</exception>
    /// <remarks>The <paramref name="expression"/> argument is automatically populated by the compiler.</remarks>
    public static string NotNullOrWhiteSpace(string? value, [CallerArgumentExpression(nameof(value))] string? expression = null)
    {
        string nonNull = NotNull(value, expression);

        if (string.IsNullOrWhiteSpace(nonNull)) throw new ArgumentException(
            "The value cannot be an empty string or composed entirely of whitespace.",
            expression ?? "value"
        );

        return nonNull;
    }
}
