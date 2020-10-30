using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainGenerator : MonoBehaviour {
    public GameObject TrainTemplate;
    public Vector2 TrainLengthVector;
    public GameObject[] VanPrefabs;
    public GameObject VanCartPrefabs;
    public Vector2 SpaceBetweenVansVector;

    private void Start() {
        var train = GenerateTrain();
    }

    public GameObject GenerateTrain() {
        var trainobj = Instantiate(TrainTemplate, transform);
        trainobj.transform.localPosition = Vector3.zero;
        trainobj.transform.localRotation = Quaternion.identity;

        var vansCount = Random.Range(TrainLengthVector.x, TrainLengthVector.y);

        var vanXPos = 0f;
        var lastVanWidth = 0f;
        for (var i = 0; i < vansCount; i++) {
            var vanobjIndex = Random.Range(0, VanPrefabs.Length);
            var vanPrefab = VanPrefabs[vanobjIndex];
            var van = Instantiate(vanPrefab, trainobj.transform);
            var vanCollider = van.GetComponentInChildren<Collider2D>();
            var vanWidth = vanCollider.bounds.size.x;
            var vanWidthAdd = 0f;
            if(i > 0)
                vanWidthAdd = vanWidth;
            var space = 0f;
            if (i > 0)
                space = Random.Range(SpaceBetweenVansVector.x, SpaceBetweenVansVector.y);
            vanXPos -= (lastVanWidth / 2 + vanWidthAdd / 2 + space) / trainobj.transform.localScale.x;
            lastVanWidth = vanWidth;
            van.transform.localPosition = new Vector3(vanXPos, 0, 0);
        }
        return trainobj;
    }
}
