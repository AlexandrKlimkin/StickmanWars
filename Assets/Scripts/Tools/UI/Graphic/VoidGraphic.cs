using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoidGraphic : Graphic {

    //protected override void Start() {
    //    OnPopulateMesh()
    //}

    protected override void OnPopulateMesh(VertexHelper vh) {
        vh.Clear();
    }
}
