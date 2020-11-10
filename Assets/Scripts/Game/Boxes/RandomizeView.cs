using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeView : MonoBehaviour {
    public bool RandomizeSprite;
    public SpriteRenderer SpriteRenderer;
    public List<Sprite> RandomSprites;

    public bool RandomizeRotation;
    public List<float> RandomRotations;

    private void Start() {
        SetRandomSprite();
        SetRandomRoatation();
    }

    private void SetRandomSprite() {
        if(!RandomizeSprite)
            return;
        if (RandomSprites == null)
            return;
        var count = RandomSprites.Count;
        if (count <= 0)
            return;
        var index = Random.Range(0, count);
        SpriteRenderer.sprite = RandomSprites[index];
    }

    private void SetRandomRoatation() {
        if(!RandomizeRotation)
            return;
        if (RandomRotations == null)
            return;
        var count = RandomRotations.Count;
        if (count <= 0)
            return;
        var index = Random.Range(0, count);
        var eulerRot = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(eulerRot.x, eulerRot.y, RandomRotations[index]);
    }
}