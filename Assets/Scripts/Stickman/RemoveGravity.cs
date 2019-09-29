using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RemoveGravity : MonoBehaviour {

    public bool Gravity;
    private bool _FlagLast;

    private List<Rigidbody2D> _Rbs;

    void Start() {
        _Rbs = GetComponentsInChildren<Rigidbody2D>().ToList();
        UseGravity(Gravity);
    }

    private void Update() {
        if(_FlagLast != Gravity) {
            UseGravity(Gravity);
            _FlagLast = Gravity;
        }
    }

    private void UseGravity(bool flag) {
        _Rbs.ForEach(_ => _.gravityScale = flag ? 1 : 0);
    }

}
