using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterConstruction
{
    public class UnitConstructionCaller : MonoBehaviour
    {
        [Button] public bool Create;
        [Button] public bool Clear;
        public int Count;

        public List<Color> RandomColors;

        public void OnCreate()
        {
            for (var i = 0; i < Count; i++)
            {
                var constructor = new HumanoidConstructor();
                var unit = constructor.ConstructUnit("White_Punk", out var offset);
                unit.transform.position = new Vector3(i * 6, 0, 0) - offset;
                unit.transform.SetParent(transform);

                var randIndex = Random.Range(0, RandomColors.Count);
                var color = RandomColors[randIndex];
                unit.Head.SpriteRenderers[1].color = color;
                unit.Head.SpriteRenderers[2].color = color;
            }

                
        }

        public void OnClear()
        {
            for (var i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                DestroyImmediate(child.gameObject);
            }
        }
    }
}
