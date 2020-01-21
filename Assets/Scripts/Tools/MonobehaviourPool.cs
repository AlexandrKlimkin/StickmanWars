using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Tools;

public abstract class MonoBehaviourPool<PoolType, ObjectType> : SingletonBehaviour<PoolType> where PoolType : MonoBehaviour where ObjectType : MonoBehaviour {

    protected ObjectType _ObjectPrefab;
    protected List<ObjectType> _Objects;
    protected abstract string _PrefabPath { get; }

    protected override void Awake() {
        base.Awake();
        _Objects = new List<ObjectType>();
        LoadPrefab();
    }

    protected void LoadPrefab() {
        _ObjectPrefab = Resources.Load<ObjectType>(_PrefabPath);
    }

    public virtual ObjectType GetObject() {
        var freeObj = _Objects.FirstOrDefault(_ => !_.gameObject.activeSelf);
        if (freeObj == null)
        {
            freeObj = AddObject();
        }
        //freeObj.gameObject.SetActive(true);
        return freeObj;
    }

    private ObjectType AddObject() {
        var newObj = Instantiate(_ObjectPrefab, transform, false);
        newObj.gameObject.SetActive(false);
        _Objects.Add(newObj);
        return newObj;
    }
}
