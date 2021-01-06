using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRigidbodiesSimulated : MonoBehaviour {
    public Rigidbody2D[] RBs;

    public bool Active;
    public float Delay;

    private Rigidbody2D _Rigidbody2D;


    private void Start() {
        StartCoroutine(ChangeSpriteRoutine());
    }

    private IEnumerator ChangeSpriteRoutine() {
        yield return new WaitForSeconds(Delay);
        RBs.ForEach(_ => _.simulated = Active);
    }
}
