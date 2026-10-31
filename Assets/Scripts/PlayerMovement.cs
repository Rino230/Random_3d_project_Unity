using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(InputReader))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float gravity = -20f;

    [Header("Camera")]
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float mouseSensitivity = 2f;

    private CharacterController controller;
    private InputReader inputReader;
    private float verticalVelocity;
    private float cameraRotationX;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        inputReader = GetComponent<InputReader>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (GameEndUI.GameEnded)
            return;

        Move();
        Look();
    }

    private void Move()
    {
        Vector2 input = Vector2.ClampMagnitude(inputReader.MoveInput, 1f);
        Vector3 move = transform.right * input.x + transform.forward * input.y;

        controller.Move(move * moveSpeed * Time.deltaTime);

        if (controller.isGrounded)
        {
            if (verticalVelocity < 0f)
                verticalVelocity = -2f;

            if (inputReader.JumpPressed)
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        if (inputReader.JumpReleased && verticalVelocity > 0f)
            verticalVelocity *= 0.5f;

        verticalVelocity += gravity * Time.deltaTime;
        controller.Move(Vector3.up * verticalVelocity * Time.deltaTime);
    }

    private void Look()
    {
        Vector2 look = inputReader.LookInput;

        float mouseX = look.x * mouseSensitivity * 0.01f;
        float mouseY = look.y * mouseSensitivity * 0.01f;

        transform.Rotate(Vector3.up * mouseX);

        cameraRotationX -= mouseY;
        cameraRotationX = Mathf.Clamp(cameraRotationX, -85f, 85f);

        playerCamera.localRotation = Quaternion.Euler(cameraRotationX, 0f, 0f);
    }
}