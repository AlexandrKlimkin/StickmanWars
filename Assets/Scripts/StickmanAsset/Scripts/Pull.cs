using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Tools;
using UnityEngine;

public class Pull : MonoBehaviour
{
    public float force = 70;
    private Rigidbody2D _Rigidbody;

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            var collider = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            if (collider != null)
            {
                _Rigidbody = collider.gameObject.GetComponentInParent<Rigidbody2D>();
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            _Rigidbody = null;
        }

        if (Input.GetKey(KeyCode.Mouse0)) {
            if(_Rigidbody == null)
                return;
            var screenMousePos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var dir = _Rigidbody.position - screenMousePos;
            dir = dir.normalized;
            //_Rigidbody.transform.position -= (Vector3)dir * force * Time.deltaTime;
            if(Vector2.Distance(screenMousePos, _Rigidbody.position) > 1f)
                _Rigidbody.MovePosition(_Rigidbody.position + -dir * force * Time.deltaTime);
            else
            {
                _Rigidbody.MovePosition(screenMousePos);
            }
            //_Rigidbody.AddForce(-dir * force * Time.deltaTime * 1000);
        }
    }
}
