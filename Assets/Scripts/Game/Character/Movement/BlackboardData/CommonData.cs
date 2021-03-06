﻿using Character.Shooting;
using UnityEngine;

namespace Character.Movement.Modules
{
    public class CommonData : BlackboardData
    {
        public Transform ObjTransform;
        public Rigidbody2D ObjRigidbody;
        public Collider2D Collider;
        public MovementController MovementController;
        public WeaponController WeaponController;
    }
}