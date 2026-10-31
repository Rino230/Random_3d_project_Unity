using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{
    private PlayerControls controls;

    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }

    public bool JumpPressed { get; private set; }
    public bool JumpReleased { get; private set; }
    public bool ShootPressed { get; private set; }

    private void Awake()
    {
        controls = new PlayerControls();

        controls.Player.Move.performed += context =>
            MoveInput = context.ReadValue<Vector2>();
        controls.Player.Move.canceled += context =>
            MoveInput = Vector2.zero;

        controls.Player.Look.performed += context =>
            LookInput = context.ReadValue<Vector2>();
        controls.Player.Look.canceled += context =>
            LookInput = Vector2.zero;

        controls.Player.Jump.started += context => JumpPressed = true;
        controls.Player.Jump.canceled += context => JumpReleased = true;
        controls.Player.Shoot.started += context => ShootPressed = true;
    }

    private void OnEnable()
    {
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }

    private void LateUpdate()
    {
        JumpPressed = false;
        JumpReleased = false;
        ShootPressed = false;
    }
}