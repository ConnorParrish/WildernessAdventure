using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private VirtualController virtualController;
    private PlayerInput playerInput;
    private PlayerAimController aimController;

    void Awake()
    {
        virtualController = GetComponent<VirtualController>();

    }

    private void OnMouseDown() {
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void OnEnable() {
        playerInput.Player.Enable();
    }
}
