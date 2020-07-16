using System;
using System.Collections.Generic;

public static partial class Utils {
    public static IEnumerable<T> ForEach<T>(this IEnumerable<T> collection, Action<T> action) {
        foreach (var item in collection) {
            action(item);
        }
        return collection;
    }

    public static bool IsNullOrEmpty<T>(this ICollection<T> collection) {
        if (collection == null || collection.Count == 0)
            return true;
        return false;
    }

    public static string RemoveClonePostfix(string name) {
        return name.Replace("(Clone)", "");
    }
}
