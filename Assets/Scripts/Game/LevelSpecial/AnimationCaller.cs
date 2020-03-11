using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.LevelSpecial
{
    public class AnimationCaller : MonoBehaviour
    {
        public Animation Animation;
        public Vector2 RandomDelay;
        public bool AnimateOnStart;
        public bool PingPong;

        private bool _Forward;

        private void Start()
        {
            if (AnimateOnStart)
                Animation.Play();
            StartCoroutine(AnimateRoutine());
        }

        private IEnumerator AnimateRoutine()
        {
            while (true)
            {
                var delay = Random.Range(RandomDelay.x, RandomDelay.y);
                yield return new WaitForSeconds(delay);
                if (PingPong) {

                }
                Animation.Play();
            }
        }
    }
}