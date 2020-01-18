using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpriteRenderersSwitcher : MonoBehaviour
{
    [Button("Switch")]
    public bool Switcher;

    private void Switch()
    {
        var renderers = new List<SpriteRenderer>();
        GetComponentsInChildren(renderers);
        var enabled = renderers.First().enabled;
        renderers.ForEach(_=>_.enabled = !enabled);
    }
}
