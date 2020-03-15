using System.Collections;
using System.Collections.Generic;
using Game.LevelSpecial;
using UnityEngine;

namespace Game.LevelSpecial
{
    public class MovingObjectsRandomSpawner : MonoBehaviour
    {
        public List<SimpleDirectionalMovement> ObjectList;
        public Vector2 RandomDelay;
        public bool RandomizeScaleX;
        public Vector2 RandomScaleX;
        public bool RandomizeScaleY;
        public Vector2 RandomScaleY;
        public bool RandomizePosition;
        public Vector2 PositionRectSize;
        public bool RandomizeSpeed;
        public Vector2 RandomSpeed;

        private float _Timer;
        private float _Delay;

        private Dictionary<SimpleDirectionalMovement, MovingObjectsPool> _PoolsDict = new Dictionary<SimpleDirectionalMovement, MovingObjectsPool>();

        private void Start()
        {
            CreatePools();
        }

        private void Update()
        {
            if (_Timer >= _Delay)
            {
                SpawnRandomObject();
                _Delay = Random.Range(RandomDelay.x, RandomDelay.y);
                _Timer = 0;
            }
            else
            {
                _Timer += Time.deltaTime;
            }
        }

        private void CreatePools()
        {
            var objectsContainer = new GameObject("CloudsContainer");
            objectsContainer.transform.SetParent(this.transform);
            foreach (var obj in ObjectList)
            {
                var pool = new MovingObjectsPool();
                pool.Initialize(this.transform, obj);
                _PoolsDict.Add(obj, pool);
            }
        }

        private void SpawnRandomObject()
        {
            var index = Random.Range(0, ObjectList.Count);
            var objTemplate = ObjectList[index];
            var scaleX = transform.localScale.x;
            var scaleY = transform.localScale.y;
            var localPosX = 0f;
            var localPosY = 0f;
            var localPosZ = 0f;
            if (RandomizeScaleX)
                scaleX = Random.Range(RandomScaleX.x, RandomScaleX.y);
            if (RandomizeScaleX)
                scaleY = Random.Range(RandomScaleY.x, RandomScaleY.y);
            if (RandomizePosition)
            {
                localPosX = Random.Range(-PositionRectSize.x / 2, PositionRectSize.x / 2);
                localPosY = Random.Range(-PositionRectSize.y / 2, PositionRectSize.y / 2);
            }
            var pool = _PoolsDict[objTemplate];
            var obj = pool.GetObject();
            obj.transform.localScale = new Vector3(scaleX, scaleY, obj.transform.localScale.z);
            obj.transform.localPosition = new Vector3(localPosX, localPosY, localPosZ);
            if (RandomizeSpeed)
                obj.Speed = Random.Range(RandomSpeed.x, RandomSpeed.y);
        }

        private void OnDrawGizmosSelected()
        {
            if (!RandomizePosition)
                return;
            var point1 = transform.position + new Vector3(-PositionRectSize.x / 2, PositionRectSize.y / 2, 0);
            var point2 = transform.position + new Vector3(PositionRectSize.x / 2, PositionRectSize.y / 2, 0);
            var point3 = transform.position + new Vector3(PositionRectSize.x / 2, -PositionRectSize.y / 2, 0);
            var point4 = transform.position + new Vector3(-PositionRectSize.x / 2, -PositionRectSize.y / 2, 0);
            Gizmos.DrawLine(point1, point2);
            Gizmos.DrawLine(point2, point3);
            Gizmos.DrawLine(point3, point4);
            Gizmos.DrawLine(point4, point1);
        }
    }
}