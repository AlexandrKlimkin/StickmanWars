using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCaller : MonoBehaviour
{
    public Animation Animation;
    public Vector2 RandomDelay;
    public bool AnimateOnStart;

    private void Start()
    {
        if(AnimateOnStart)
            Animation.Play();
        StartCoroutine(AnimateRoutine());
    }

    private IEnumerator AnimateRoutine()
    {
        while (true)
        {
            var delay = Random.Range(RandomDelay.x, RandomDelay.y);
            yield return new WaitForSeconds(delay);
            Animation.Play();
        }
    }
}