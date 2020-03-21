using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tools;
using UnityEngine;

public class UIManager : SingletonBehaviour<UIManager>
{
    private Dictionary<Type, IUIPanel> _PanelDict = new Dictionary<Type, IUIPanel>();
    private List<IUIPanel> _Panels = new List<IUIPanel>();

    private void Awake()
    {
        CollectPanels();
    }

    private void Start()
    {
        DeactivateAll();
        //SetActivePanel<MainPanel>();
    }

    public void DeactivateAll()
    {
        _Panels.ForEach(_=>_.Deactivate());
    }

    private void CollectPanels()
    {
        GetComponentsInChildren(_Panels);
        _PanelDict = _Panels.ToDictionary(_ => _.GetType());
    }

    public T SetActivePanel<T>() where T : IUIPanel
    {
        var type = typeof(T);
        if (!_PanelDict.ContainsKey(type))
            Debug.LogError($"UI Manager has no panel type of {type}");
        var panel = (T)_PanelDict[type];
        DeactivateAll();
        panel.Activate();
        return panel;
    }
}
