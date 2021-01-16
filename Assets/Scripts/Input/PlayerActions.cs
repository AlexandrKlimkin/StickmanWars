using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class PlayerActions : PlayerActionSet {
    public readonly PlayerAction Fire;
    public readonly PlayerAction Jump;
    public readonly PlayerAction Confirm;
    public readonly PlayerAction ThrowOutWeapon;
    public readonly PlayerAction ThrowOutVehicle;
    public readonly PlayerAction Left;
    public readonly PlayerAction Right;
    public readonly PlayerAction Up;
    public readonly PlayerAction Down;

    public readonly PlayerAction AimLeft;
    public readonly PlayerAction AimRight;
    public readonly PlayerAction AimUp;
    public readonly PlayerAction AimDown;

    public readonly PlayerTwoAxisAction Move;
    public readonly PlayerTwoAxisAction Aim;

    public PlayerActions() {
        Fire = CreatePlayerAction("Fire");
        Jump = CreatePlayerAction("Jump");
        Confirm = CreatePlayerAction("Confirm");
        ThrowOutWeapon = CreatePlayerAction("ThrowOutWeapon");
        ThrowOutVehicle = CreatePlayerAction("ThrowOutVehicle");

        Left = CreatePlayerAction("Move Left");
        Right = CreatePlayerAction("Move Right");
        Up = CreatePlayerAction("Move Up");
        Down = CreatePlayerAction("Move Down");
        Move = CreateTwoAxisPlayerAction(Left, Right, Down, Up);

        AimLeft = CreatePlayerAction("Aim Move Left");
        AimRight = CreatePlayerAction("Aim Move Right");
        AimUp = CreatePlayerAction("Aim Move Up");
        AimDown = CreatePlayerAction("Aim Move Down");
        Aim = CreateTwoAxisPlayerAction(AimLeft, AimRight, AimDown, AimUp);
    }

    public static PlayerActions CreateWithKeyboardBindings() {
        var playerActions = new PlayerActions();

        playerActions.Fire.AddDefaultBinding(Mouse.LeftButton);

        playerActions.Jump.AddDefaultBinding(Key.Space);

        playerActions.Confirm.AddDefaultBinding(Key.Space);

        playerActions.ThrowOutWeapon.AddDefaultBinding(Key.C);
        playerActions.ThrowOutVehicle.AddDefaultBinding(Key.X);

        playerActions.Up.AddDefaultBinding(Key.UpArrow);
        playerActions.Down.AddDefaultBinding(Key.DownArrow);
        playerActions.Left.AddDefaultBinding(Key.LeftArrow);
        playerActions.Right.AddDefaultBinding(Key.RightArrow);

        playerActions.Up.AddDefaultBinding(Key.W);
        playerActions.Down.AddDefaultBinding(Key.S);
        playerActions.Left.AddDefaultBinding(Key.A);
        playerActions.Right.AddDefaultBinding(Key.D);

        playerActions.AimUp.AddDefaultBinding(Mouse.PositiveY);
        playerActions.AimDown.AddDefaultBinding(Mouse.NegativeY);
        playerActions.AimLeft.AddDefaultBinding(Mouse.NegativeX);
        playerActions.AimRight.AddDefaultBinding(Mouse.PositiveX);

        playerActions.ListenOptions.IncludeModifiersAsFirstClassKeys = true;
        playerActions.ListenOptions.MaxAllowedBindings = 1;
        playerActions.ListenOptions.UnsetDuplicateBindingsOnSet = true;
        playerActions.ListenOptions.IncludeMouseButtons = true;
        playerActions.ListenOptions.IncludeKeys = true;
        playerActions.ListenOptions.IncludeMouseScrollWheel = true;

        return playerActions;
    }

    public static PlayerActions CreateWithJoystickBindings() {
        var playerActions = new PlayerActions();

        playerActions.Fire.AddDefaultBinding(InputControlType.RightBumper);

        playerActions.Jump.AddDefaultBinding(InputControlType.LeftBumper);

        playerActions.Confirm.AddDefaultBinding(InputControlType.Action1);

        playerActions.ThrowOutWeapon.AddDefaultBinding(InputControlType.Action4);
        playerActions.ThrowOutVehicle.AddDefaultBinding(InputControlType.Action3);

        playerActions.Left.AddDefaultBinding(InputControlType.LeftStickLeft);
        playerActions.Right.AddDefaultBinding(InputControlType.LeftStickRight);
        playerActions.Up.AddDefaultBinding(InputControlType.LeftStickUp);
        playerActions.Down.AddDefaultBinding(InputControlType.LeftStickDown);

        playerActions.AimUp.AddDefaultBinding(InputControlType.RightStickUp);
        playerActions.AimDown.AddDefaultBinding(InputControlType.RightStickDown);
        playerActions.AimLeft.AddDefaultBinding(InputControlType.RightStickLeft);
        playerActions.AimRight.AddDefaultBinding(InputControlType.RightStickRight);

        playerActions.ListenOptions.IncludeUnknownControllers = true;
        playerActions.ListenOptions.MaxAllowedBindings = 4;
        playerActions.ListenOptions.UnsetDuplicateBindingsOnSet = true;

        return playerActions;
    }

    //public static PlayerActions CreateWithDefaultBindings() {
    //    var playerActions = new PlayerActions();

    //    playerActions.Fire.AddDefaultBinding(Mouse.LeftButton);
    //    playerActions.Fire.AddDefaultBinding(InputControlType.RightBumper);

    //    playerActions.Jump.AddDefaultBinding(Key.Space);
    //    playerActions.Jump.AddDefaultBinding(InputControlType.LeftBumper);

    //    playerActions.ThrowOutWeapon.AddDefaultBinding(Key.C);
    //    playerActions.ThrowOutVehicle.AddDefaultBinding(Key.X);
    //    playerActions.ThrowOutWeapon.AddDefaultBinding(InputControlType.Action4);
    //    playerActions.ThrowOutVehicle.AddDefaultBinding(InputControlType.Action3);

    //    playerActions.Up.AddDefaultBinding(Key.UpArrow);
    //    playerActions.Down.AddDefaultBinding(Key.DownArrow);
    //    playerActions.Left.AddDefaultBinding(Key.LeftArrow);
    //    playerActions.Right.AddDefaultBinding(Key.RightArrow);

    //    playerActions.Up.AddDefaultBinding(Key.W);
    //    playerActions.Down.AddDefaultBinding(Key.S);
    //    playerActions.Left.AddDefaultBinding(Key.A);
    //    playerActions.Right.AddDefaultBinding(Key.D);

    //    playerActions.Left.AddDefaultBinding(InputControlType.LeftStickLeft);
    //    playerActions.Right.AddDefaultBinding(InputControlType.LeftStickRight);
    //    playerActions.Up.AddDefaultBinding(InputControlType.LeftStickUp);
    //    playerActions.Down.AddDefaultBinding(InputControlType.LeftStickDown);

    //    playerActions.AimUp.AddDefaultBinding(InputControlType.RightStickUp);
    //    playerActions.AimDown.AddDefaultBinding(InputControlType.RightStickDown);
    //    playerActions.AimLeft.AddDefaultBinding(InputControlType.RightStickLeft);
    //    playerActions.AimRight.AddDefaultBinding(InputControlType.RightStickRight);

    //    playerActions.AimUp.AddDefaultBinding(Key.W);
    //    playerActions.AimDown.AddDefaultBinding(Key.S);
    //    playerActions.AimLeft.AddDefaultBinding(Key.A);
    //    playerActions.AimRight.AddDefaultBinding(Key.D);

    //    playerActions.ListenOptions.IncludeUnknownControllers = true;
    //    playerActions.ListenOptions.IncludeModifiersAsFirstClassKeys = true;
    //    playerActions.ListenOptions.MaxAllowedBindings = 4;
    //    playerActions.ListenOptions.UnsetDuplicateBindingsOnSet = true;
    //    playerActions.ListenOptions.IncludeMouseButtons = true;
    //    playerActions.ListenOptions.IncludeKeys = true;
    //    playerActions.ListenOptions.IncludeMouseScrollWheel = true;

    //    playerActions.ListenOptions.OnBindingFound = (action, binding) => {
    //        if (binding == new KeyBindingSource(Key.Escape)) {
    //            action.StopListeningForBinding();
    //            return false;
    //        }

    //        return true;
    //    };

    //    playerActions.ListenOptions.OnBindingAdded += (action, binding) => { Debug.Log("Binding added... " + binding.DeviceName + ": " + binding.Name); };

    //    playerActions.ListenOptions.OnBindingRejected += (action, binding, reason) => { Debug.Log("Binding rejected... " + reason); };

    //    return playerActions;
    //}

}
