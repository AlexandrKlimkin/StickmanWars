using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSprite : MonoBehaviour {
    public Sprite SpriteToChange;
    public float Delay;

    private SpriteRenderer _SpriteRenderer;


    private void Start() {
        _SpriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(ChangeSpriteRoutine());
    }

    private IEnumerator ChangeSpriteRoutine() {
        yield return new WaitForSeconds(Delay);
        _SpriteRenderer.sprite = SpriteToChange;
    }
}