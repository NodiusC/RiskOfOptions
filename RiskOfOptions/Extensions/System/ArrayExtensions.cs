namespace RiskOfOptions.Extensions.System;

/// <summary>
/// Provides extension methods for arrays.
/// </summary>
public static class ArrayExtensions
{
    /// <summary>
    /// Attempts to get the value at the specified index.
    /// </summary>
    /// <typeparam name="T">The type of elements in the array.</typeparam>
    /// <param name="array">The source array.</param>
    /// <param name="index">The zero-based index of the element to get.</param>
    /// <param name="value">
    /// When this method returns, contains the value at the specified index if the index is found; 
    /// otherwise, the default value for the type <typeparamref name="T"/>.
    /// </param>
    /// <returns><see langword="true"/> if the index was within the bounds of the array; otherwise, <see langword="false"/>.</returns>
    /// <remarks>
    /// This method prevents <see cref="IndexOutOfRangeException"/> by performing a bounds check 
    /// before accessing the array.
    /// </remarks>
    public static bool TryGetValue<T>(this T[] array, int index, out T value)
    {
        if (index < 0 || index >= array.Length)
        {
            value = default!;
            return false;
        }
        
        value = array[index];
        return true;
    }
}
