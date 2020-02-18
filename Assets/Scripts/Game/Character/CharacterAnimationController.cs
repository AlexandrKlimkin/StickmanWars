using System.Collections;
using System.Collections.Generic;
using Character.Movement;
using UnityEngine;

public class CharacterAnimationController : MonoBehaviour
{
    private Animator Animator;
    private MovementController _MovementController;

    private void Awake()
    {
        Animator = GetComponent<Animator>();
        _MovementController = GetComponentInParent<MovementController>();
    }

    private void Update()
    {
        Animator.SetFloat("Horizontal", Mathf.Abs(_MovementController.Horizontal));
        Animator.SetBool("Grounded", _MovementController.IsGrounded);
        Animator.SetFloat("DistanseToGround", _MovementController.MinDistanceToGround);
        Animator.SetBool("FallingDown", _MovementController.FallingDown);
        Animator.SetBool("WallSliding", _MovementController.WallSliding);
        Animator.SetFloat("Speed", Mathf.Abs(_MovementController.Rigidbody.velocity.x / 50f));
    }
}