using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SimpleCCDAngleSwitcher : MonoBehaviour
{
    [Button("Switch")]
    public bool Switcher;

    private void Switch() {
        var ccds = new List<SimpleCCD>();
        GetComponentsInChildren(ccds);
        var enabled = ccds.First().DrawAnglesGizmos;
        ccds.ForEach(_ => _.DrawAnglesGizmos = !enabled);
    }
}
