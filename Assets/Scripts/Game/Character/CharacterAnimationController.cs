using System.Collections;
using System.Collections.Generic;
using Character.Movement;
using UnityEngine;

public class CharacterAnimationController : MonoBehaviour
{
    private Animator Animator;
    private MovementController _MovementController;
    private Unit _Unit;

    private void Awake()
    {
        Animator = GetComponent<Animator>();
        _MovementController = GetComponentInParent<MovementController>();
        _Unit = GetComponentInParent<Unit>();
    }

    private void Start()
    {
        _Unit.OnApplyDamage += this.OnApplyDamage;
    }

    private void OnApplyDamage() {
        //Animator.SetTrigger("BodyHit");
    }

    private void Update()
    {
        Animator.SetFloat("Horizontal", Mathf.Abs(_MovementController.Horizontal));
        Animator.SetBool("Grounded", _MovementController.IsMainGrounded);
        Animator.SetFloat("DistanseToGround", _MovementController.MinDistanceToGround);
        Animator.SetBool("FallingDown", _MovementController.FallingDown);
        Animator.SetBool("WallRun", _MovementController.WallRun);
        Animator.SetBool("WallSliding", _MovementController.WallSliding);
        Animator.SetBool("LedgeHang", _MovementController.LedgeHang);
        Animator.SetFloat("Speed", Mathf.Abs(_MovementController.Rigidbody.velocity.x / 50f));
    }
}