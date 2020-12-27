using Character.Shooting;
using UnityEngine;

namespace Character.Movement.Modules
{
    public class CommonData : BlackboardData
    {
        public Transform ObjTransform;
        public Rigidbody2D ObjRigidbody;
        public Collider2D BodyCollider;
        public Collider2D GroundCollider;
        public MovementController MovementController;
        public WeaponController WeaponController;
    }
}