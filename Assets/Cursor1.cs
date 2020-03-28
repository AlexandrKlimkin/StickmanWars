using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor1 : MonoBehaviour
{
    public Texture2D Texture;
    void Start()
    {
        Cursor.SetCursor(Texture, Vector2.zero, CursorMode.Auto);
    }

}
