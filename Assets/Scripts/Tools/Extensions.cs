using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Tools {
    public static class MathExtensions {
        public static Vector2 ToVector2(this Vector3 vector) {
            return new Vector2(vector.x, vector.y);
        }

        public static Vector3 ToVector3(this Vector2 vector) {
            return new Vector3(vector.x, vector.y);
        }

        public static Vector3 TransformPointUnscaled(Transform t, Vector3 point) {
            return t.position + t.rotation * point;
        }
    }

    public static class IEnumerableExtentions {
        public static string ToStringInternal(this IEnumerable collection) {
            var str = "";
            var count = 0;
            foreach (var element in collection) {
                str += $" {element}";
                count++;
            }
            str += $". Total elements: {count}";
            return str;
        }
    }
}
