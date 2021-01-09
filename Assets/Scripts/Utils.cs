using System;
using System.Collections.Generic;

public static class Utils
{
    /// <summary>
    /// Returns the index of the first matching element in a set. Returns -1 if no element
    /// matches the filter.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the collection.</typeparam>
    /// <param name="collection">The collection to search in.</param>
    /// <param name="filter">The filter callback. Returns true for the searched element.</param>
    /// <returns>The index of the first matching element in the collection. Returns -1 if no
    /// matching element was found.</returns>
    public static int IndexOf<T>(this IEnumerable<T> collection, Func<T, bool> filter)
    {
        int currentIndex = 0;
        foreach (var item in collection)
        {
            if (filter(item)) return currentIndex;
            currentIndex += 1;
        }
        return -1;
    }
}
