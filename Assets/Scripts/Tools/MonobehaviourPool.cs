using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Tools
{
    public abstract class MonoBehaviourPool<ObjectType> where ObjectType : MonoBehaviour
    {
        protected ObjectType _ObjectPrefab;
        protected List<ObjectType> _Objects;
        protected string _PrefabPath;
        protected Transform PoolTransform;

        protected void LoadPrefab()
        {
            _ObjectPrefab = Resources.Load<ObjectType>(_PrefabPath);
        }

        public virtual void Initialize(Transform root, string prefabPath)
        {
            PoolTransform = root;
            _PrefabPath = prefabPath;
            _Objects = new List<ObjectType>();
            LoadPrefab();
        }

        public virtual void Initialize(Transform root, ObjectType prefab) {
            PoolTransform = root;
            _ObjectPrefab = prefab;
            _Objects = new List<ObjectType>();
        }

        public virtual ObjectType GetObject()
        {
            var freeObj = _Objects.FirstOrDefault(_ => !_.gameObject.activeSelf);
            if (freeObj == null)
            {
                freeObj = AddObject();
            }

            freeObj.gameObject.SetActive(true);
            return freeObj;
        }

        public virtual void PutObject(ObjectType obj)
        {
            obj.transform.SetParent(PoolTransform);
            obj.gameObject.SetActive(false);
        }

        private ObjectType AddObject()
        {
            var newObj = GameObject.Instantiate(_ObjectPrefab, PoolTransform, false);
            newObj.gameObject.SetActive(false);
            _Objects.Add(newObj);
            return newObj;
        }
    }
}
