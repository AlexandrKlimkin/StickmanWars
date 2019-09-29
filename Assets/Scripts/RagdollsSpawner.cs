using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RagdollsSpawner : MonoBehaviour
{
    public GameObject RagdollPrefab;

    private readonly Vector3 _ScaleVector = new Vector3(1,1,0);

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            var pos = Input.mousePosition;
            var worldPos = Vector3.Scale(Camera.main.ScreenToWorldPoint(pos), _ScaleVector);
            var doll = Instantiate(RagdollPrefab, worldPos, Quaternion.Euler(0,0,Random.Range(0,360)));
            var renderers = doll.GetComponentsInChildren<SpriteRenderer>().ToList();
            var randColor = new Color(Random.value, Random.value, Random.value);
            renderers.ForEach(_ => _.color = randColor);
        }
    }
}
