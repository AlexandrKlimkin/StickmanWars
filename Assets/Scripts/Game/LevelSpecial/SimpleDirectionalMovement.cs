using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.LevelSpecial
{
    public class SimpleDirectionalMovement : MonoBehaviour
    {
        public Vector3 Direction;
        public bool IsMoving;
        public float Speed;

        private void Update()
        {
            if(!IsMoving)
                return;
            transform.position += Direction * Speed * Time.deltaTime;
        }
    }
}
