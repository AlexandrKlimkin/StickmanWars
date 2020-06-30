using System.Collections;
using System.Collections.Generic;
using Character.Movement;
using UnityEngine;

public class CharacterAnimationController : MonoBehaviour {
    private Animator Animator;
    private MovementController _MovementController;
    private CharacterUnit _CharacterUnit;

    private void Awake() {
        Animator = GetComponent<Animator>();
        _MovementController = GetComponentInParent<MovementController>();
        _CharacterUnit = GetComponentInParent<CharacterUnit>();
    }

    //private void Start()
    //{
    //    _CharacterUnit.OnApplyDamage += this.OnApplyDamage;
    //}

    //private void OnApplyDamage() {
    //    //Animator.SetTrigger("BodyHit");
    //}

    private void Update() {
        Animator.SetFloat("Horizontal", Mathf.Abs(_MovementController.Horizontal));
        Animator.SetBool("Grounded", _MovementController.IsMainGrounded);
        Animator.SetFloat("DistanseToGround", _MovementController.MinDistanceToGround);
        Animator.SetBool("FallingDown", _MovementController.FallingDown);
        Animator.SetBool("WallRun", _MovementController.WallRun);
        Animator.SetBool("WallSliding", _MovementController.WallSliding);
        Animator.SetBool("LedgeHang", _MovementController.LedgeHang);
        Animator.SetFloat("Speed", Mathf.Abs(_MovementController.Rigidbody.velocity.x / 50f));
        Animator.SetBool("Pushing", _MovementController.Pushing);
        Animator.SetFloat("TimeFallingDown", _MovementController.TimeFallingDown);
        Animator.SetFloat("TimeNotFallingDown", _MovementController.TimeNotFallingDown);
    }
}