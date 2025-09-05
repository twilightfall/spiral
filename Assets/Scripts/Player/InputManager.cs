using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class InputManager : MonoBehaviour
{
    public Vector2 movementVector;
    public Vector2 lookVector;

    private Gamepad gamepad;
    private bool isControllerConnected;

    public static InputManager instance { get; private set; }

    private PlayerInput playerInput;

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;

        playerInput = GetComponent<PlayerInput>();
        gamepad = Gamepad.current;
    }

    private void Update()
    {
        _ = gamepad == null ? isControllerConnected = false : isControllerConnected = true;

        if (isControllerConnected && playerInput.currentControlScheme != "Gamepad")
        {
            print("Controller connected");
            playerInput.SwitchCurrentControlScheme("Gamepad");
        }
        else if (!isControllerConnected && playerInput.currentControlScheme != "Keyboard&Mouse")
        {
            playerInput.SwitchCurrentControlScheme("Keyboard&Mouse");
        }
    }

    public void OnMove(InputValue value)
    {
        print(value.Get<Vector2>());
        movementVector = value.Get<Vector2>();
    }
}
