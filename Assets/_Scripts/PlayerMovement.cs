using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 6f;
    [SerializeField] private float _rotationSpeed = 10f;

    [Header("Jumping")]
    [SerializeField] private float _jumpHeight = 1.5f;
    [SerializeField] private float _gravity = -20f;

    [Header("References")]
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private ParticleSystem runningPS;

    private CharacterController _controller;
    private float _verticalVelocity;

    private void Start() {
        _controller = GetComponent<CharacterController>();
    }

    private void Update() {
        Vector2 moveInput = UserInputManager.Instance.moveInput;
        bool jumpPressed = UserInputManager.Instance.jumpPressed;

        float h = moveInput.x;
        float v = moveInput.y;

        Vector3 inputDir = new Vector3(h, 0f, v).normalized;

        // Movement direction (camera-relative)
        Vector3 moveDir = Vector3.zero;

        bool isMoving = inputDir.magnitude >= 0.1f;

        if (isMoving) {
            // -- CAMERA RELATIVE DIRECTION --
            Vector3 camForward = _cameraTransform.forward;
            Vector3 camRight = _cameraTransform.right;

            camForward.y = 0;
            camRight.y = 0;

            camForward.Normalize();
            camRight.Normalize();

            moveDir = (camForward * v + camRight * h).normalized;

            // -- SMOOTH ROTATION --
            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRot,
                _rotationSpeed * Time.deltaTime
            );
        }

        // ----------------------------
        //          JUMP / GRAVITY
        // ----------------------------

        bool isGrounded = _controller.isGrounded;

        if (isGrounded) {
            // Reset downward velocity so it doesn't keep stacking
            if (_verticalVelocity < 0f)
                _verticalVelocity = -2f;

            // Jump
            if (jumpPressed) {
                _verticalVelocity = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
            }
        }
        
        // Enable Particle System
        if (isMoving && isGrounded) {
            if(!runningPS.isPlaying) 
                runningPS.Play();
        }
        else {
            runningPS.Stop();
        }

        // Gravity always applies
        _verticalVelocity += _gravity * Time.deltaTime;

        // Final movement vector
        Vector3 finalVelocity = moveDir * _moveSpeed;
        finalVelocity.y = _verticalVelocity;

        _controller.Move(finalVelocity * Time.deltaTime);
    }
    
}