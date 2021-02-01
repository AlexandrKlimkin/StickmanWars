using System.Collections;
using System.Collections.Generic;
using Tools.VisualEffects;
using UnityEngine;

public class DintEffect : VisualEffect {
    public SpriteRenderer SpriteRenderer;
    public float ExistTime;
    public float GoToAlphaTime;

    protected override IEnumerator PlayTask() {
        SpriteRenderer.color = new Color(SpriteRenderer.color.r, SpriteRenderer.color.g, SpriteRenderer.color.b, 255);
        yield return new WaitForSeconds(ExistTime);
        var timer = GoToAlphaTime;
        while (timer > 0) {
            SpriteRenderer.color = new Color(SpriteRenderer.color.r, SpriteRenderer.color.g, SpriteRenderer.color.b, timer / GoToAlphaTime);
            timer -= Time.deltaTime;
            yield return null;
        }
        SpriteRenderer.color = new Color(SpriteRenderer.color.r, SpriteRenderer.color.g, SpriteRenderer.color.b, 0);
        this.gameObject.SetActive(false);
    }
}