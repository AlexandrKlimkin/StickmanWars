using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanel : MonoBehaviour, IUIPanel {
    public bool Active { get; private set; }
    private bool _Initialized;

    protected void Initialize()
    {
        if(_Initialized)
            return;
        _Initialized = true;
        Active = gameObject.activeSelf;
    }

    void IUIPanel.Activate()
    {
        Initialize();
        if(Active)
            return;
        Active = true;
        gameObject.SetActive(true);
    }
    void IUIPanel.Deactivate() {
        Initialize();
        if (!Active)
            return;
        Active = false;
        gameObject.SetActive(false);
    }
}

public interface IUIPanel
{
    void Activate();
    void Deactivate();
}
