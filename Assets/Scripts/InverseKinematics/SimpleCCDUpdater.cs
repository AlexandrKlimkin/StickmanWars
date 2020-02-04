using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SimpleCCDUpdater : MonoBehaviour
{
    public List<SimpleCCD> CCDs;

    private void Update()
    {
        CCDs.ForEach(_=>_.UpdateRemotely());
    }
}
