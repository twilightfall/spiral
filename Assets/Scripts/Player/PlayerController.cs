using System;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Setup Variables")]
    [SerializeField]
    CharacterController controller;

    [SerializeField]
    GameObject cam;

    // Controller Variables
    [SerializeField]
    bool isControllerConnected;
    Gamepad gamepad;

    // Look Rotation Variables
    float mouseX, mouseY;
    float xRotation = 0f;

    // Movement Variables
    float horizontal, vertical;
    Vector3 velocity;

    // Jump Variables
    float jumpForce;
    public static bool canJump = false;


    // Ground Check
    [Header("Gravity Variables")]
    [SerializeField]
    Transform groundCheck;
    bool isGrounded;
    bool isJumping;
    public LayerMask groundMask;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        gamepad = Gamepad.current;
        _ = gamepad == null ? isControllerConnected = false : isControllerConnected = true;

        GravityCheck();

        Jump();
        UpdateMove();
    }

    private void LateUpdate()
    {
        Look();
    }

    private void Look()
    {
        if (isControllerConnected == false)
        {
            mouseX = Input.GetAxis("Mouse X") * 100f * Time.deltaTime;
            mouseY = Input.GetAxis("Mouse Y") * 100f * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -89f, 89f);

            transform.Rotate(Vector3.up * mouseX);
            cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }
        else
        {
            Vector2 camRotation = gamepad.rightStick.ReadValue();

            xRotation += camRotation.y;
            xRotation = Mathf.Clamp(xRotation, -89f, 89f);

            transform.Rotate(Vector3.up * camRotation.x);
            cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }
    }

    void UpdateMove()
    {
        if (isControllerConnected == false)
        {
            Vector3 movement = transform.right * InputManager.instance.movementVector.x + transform.forward * InputManager.instance.movementVector.y;

            controller.Move(5f * Time.deltaTime * movement);

            //horizontal = Input.GetAxis("Horizontal") * 30f * Time.deltaTime;
            //vertical = Input.GetAxis("Vertical") * 30f * Time.deltaTime;

            //Vector3 movement = transform.right * horizontal + transform.forward * vertical;

            //controller.Move(movement * 15f * Time.deltaTime);
        }
        else
        {
            //Vector2 moveVector = gamepad.leftStick.ReadValue();

            Vector3 movement = transform.right * InputManager.instance.movementVector.x + transform.forward * InputManager.instance.movementVector.y;

            controller.Move(5f * Time.deltaTime * movement);
        }
    }

    void GravityCheck()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, .5f, groundMask);

        if (isGrounded & velocity.y < 0f)
        {
            isJumping = false;
            velocity.y = -1f;
        }
        else
        {
            isJumping = true;
            velocity.y += Physics.gravity.y * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
    }

    void Jump()
    {
        if (canJump == true)
        {
            if (isControllerConnected == false)
            {
                if (Input.GetButtonDown("Jump") && isGrounded && !isJumping)
                {
                    isGrounded = false;
                    isJumping = true;

                    velocity.y = Mathf.Sqrt(2f * -2f * Physics.gravity.y);
                    controller.Move(velocity * Time.deltaTime);
                }
            }
            else
            {
                if (gamepad.aButton.wasPressedThisFrame && isGrounded && !isJumping)
                {
                    isGrounded = false;
                    isJumping = true;

                    velocity.y = Mathf.Sqrt(2f * -2f * Physics.gravity.y);
                    controller.Move(velocity * Time.deltaTime);
                }
            }
        }
    }
}
