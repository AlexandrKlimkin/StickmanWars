using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollController : MonoBehaviour
{
    public Rigidbody2D Head;
    public Rigidbody2D Body;
    public Rigidbody2D Leftshoulder;
    public Rigidbody2D LeftForearm;
    public Rigidbody2D Rightshoulder;
    public Rigidbody2D RightForearm;
    public Rigidbody2D LeftLegUp;
    public Rigidbody2D LeftLegDown;
    public Rigidbody2D RightLegUp;
    public Rigidbody2D RightLegDown;

    public float horSpeed;

    Vector3 _HeadStartPos;
    Vector3 BodyStartPos;
    Vector3 _LeftshoulderStartPos;
    Vector3 _LeftForearmStartPos;
    Vector3 _RightshoulderStartPos;
    Vector3 _RightForearmStartPos;
    Vector3 _LeftLegUpStartPos;
    Vector3 _LeftLegDownStartPos;
    Vector3 _RightLegUpStartPos;
    Vector3 _RightLegDownStartPos;

    private void Start() {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Body.AddForce(new Vector2(0, 2500f));
            //Head.AddForce(new Vector3(0, 3000f, 0f));
        }

        var horInput = Input.GetAxis("Horizontal");
        if(horInput != 0) {
            var legUp = horInput > 0 ? RightLegUp : RightLegDown;
            //legUp.AddForce(new Vector2(horInput * 500f, 0));
            legUp.velocity += new Vector2(horInput * horSpeed, 0);
        }
    }
}
