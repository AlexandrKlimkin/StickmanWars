using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainGenerator : MonoBehaviour {
    public Vector2 TrainLengthVector;
    public GameObject[] VanPrefabs;
    public Vector2 SpaceBetweenVansVector;

    public GameObject GenerateTrain() {
        var trainobj = new GameObject("Train");
        var vansCount = Random.Range(TrainLengthVector.x, TrainLengthVector.y);
        for (var i = 0; i < vansCount; i++) {
            var vanobjIndex = Random.Range(0, VanPrefabs.Length);
            var van = VanPrefabs[vanobjIndex];

        }
        return trainobj;
    }
}
