using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyGizmos
{
    public static void DrawRect(Rect rect)
    {
        var rightUp = rect.max;
        var leftDown = rect.min;
        var rightDown = new Vector2(rightUp.x, rightUp.y - rect.height);
        var leftUp = new Vector2(leftDown.x, leftDown.y + rect.height);
        Gizmos.DrawLine(rightDown, rightUp);
        Gizmos.DrawLine(rightDown, leftDown);
        Gizmos.DrawLine(leftUp, leftDown);
        Gizmos.DrawLine(leftUp, rightUp);

        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(rightDown, 1f);
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(leftUp, 1f);
    }
}
