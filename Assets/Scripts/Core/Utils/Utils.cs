using System;
using System.Collections.Generic;

public static partial class Utils {
    public static IEnumerable<T> ForEach<T>(this IEnumerable<T> collection, Action<T> action) {
        foreach (var item in collection) {
            action(item);
        }
        return collection;
    }
}
