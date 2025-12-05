using System;
using UnityEngine;

public class UserInputManager : MonoBehaviour {
    public static UserInputManager Instance;
    private UserInputActionAsset _userInputActionAsset;

    public Vector2 moveInput = Vector2.zero;
    public bool jumpPressed = false;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            _userInputActionAsset = new UserInputActionAsset();
        }
        else {
            Destroy(gameObject);
        }
    }

    private void OnEnable() {
        _userInputActionAsset.Enable();
        _userInputActionAsset.Movement.Enable();
        _userInputActionAsset.Movement.Move.Enable();
        _userInputActionAsset.Movement.Jump.Enable();
    }

    private void Update() {
        moveInput = _userInputActionAsset.Movement.Move.ReadValue<Vector2>();
        jumpPressed = _userInputActionAsset.Movement.Jump.WasPressedThisFrame();
    }
}
