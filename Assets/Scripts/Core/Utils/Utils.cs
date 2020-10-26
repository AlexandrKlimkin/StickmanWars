using System;
using System.Collections.Generic;
using UnityEngine;

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

    public static int GetClosestIndex(List<Vector2> points, Vector2 targetPoint) {
        if (points == null || points.Count == 0)
            return 0;
        Vector2 closestPos = points[0];
        float closestSqrDist = float.MaxValue;
        foreach (var point in points) {
            var sqrDist = Vector2.SqrMagnitude(targetPoint - point);
            if (sqrDist < closestSqrDist) {
                closestSqrDist = sqrDist;
                closestPos = point;
            }
        }
        return points.IndexOf(closestPos);
    }
}
