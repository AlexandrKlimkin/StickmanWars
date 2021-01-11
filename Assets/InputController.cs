using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnPlayerJoined() {
        Debug.LogError($"Joined, gamepad count = {Gamepad.all.Count}");
    }

    void OnPlayerLeft() {
        Debug.LogError("Left");
    }
}
