using System.Collections;
using System.Collections.Generic;
using Character.Movement;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : UIPanel
{
    public Button _FastButton;
    public Button _SlowButton;

    private void Start()
    {
        _FastButton.onClick.AddListener(SetFastParameters);
        _SlowButton.onClick.AddListener(SetSlowParameters);
        //SetSlowParameters();
    }

    private void SetFastParameters()
    {
        _FastButton.image.color = Color.green;
        _SlowButton.image.color = Color.white;
        var player = GameObject.FindGameObjectWithTag("Player");
        var moveController = player.GetComponent<MovementController>();
        moveController.Speed = 90f;
        moveController.GroundAcceleration = 30f;
        moveController.AirAcceleration = 7f;
        moveController.JumpSpeed = 170f;
        moveController.WallJumpSpeed = 250f;
        moveController.NormalGravityScale = 7f;
        moveController.FallGravityScale = 9f;
        moveController.WallSlideSpeed = 100f;
    }

    private void SetSlowParameters()
    {
        _SlowButton.image.color = Color.green;
        _FastButton.image.color = Color.white;
        var player = GameObject.FindGameObjectWithTag("Player");
        var moveController = player.GetComponent<MovementController>();
        moveController.Speed = 65f;
        moveController.GroundAcceleration = 30f;
        moveController.AirAcceleration = 7f;
        moveController.JumpSpeed = 145f;
        moveController.WallJumpSpeed = 180f;
        moveController.NormalGravityScale = 5f;
        moveController.FallGravityScale = 7f;
        moveController.WallSlideSpeed = 60f;
    }
}