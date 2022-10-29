using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;





public class MovementComponent : MonoBehaviour
{

    [Header("Player Attributes")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float speedMultiplier = 1f;
    [SerializeField] private float sensitivity = 1f;

    [SerializeField] private bool enableCameraYRotation = false;
    [SerializeField] private float cameraClampMin = -45f;
    [SerializeField] private float cameraClampMax = 45f;

    [Header("Player Components")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform cameraT;
    [SerializeField] public InputManager inputManager;
    [SerializeField] public Animator anim;



    [SerializeField] private InputAction moveInput;
    [SerializeField] private InputAction lookInput;

    [SerializeField] private Vector3 direction;
    [SerializeField] private float mouseX;
    [SerializeField] private float mouseY;
    [SerializeField] private float rotationX;
    [SerializeField] private float rotationY;


    private void OnEnable()
    {
        inputManager.Enable();

        moveInput = inputManager.Player.Move;
        moveInput.Enable();

        lookInput = inputManager.Player.Look;
        lookInput.Enable();
    }

    private void OnDisable()
    {
        inputManager.Disable();

        moveInput.Disable();
        lookInput.Disable();
    }


    void Awake()
    {
        inputManager = new InputManager();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        direction = transform.right * moveInput.ReadValue<Vector2>().x + transform.forward * moveInput.ReadValue<Vector2>().y;
        
        if(direction != Vector3.zero)
            anim.SetBool("isWalking", true);
        else
            anim.SetBool("isWalking", false);

        Look();
    }

    void FixedUpdate()
    {
        Move(direction);
    }

    private void Move(Vector3 _dir)
    {
        rb.AddForce(_dir * speed * speedMultiplier, ForceMode.Acceleration);
    }

    private void Look()
    {
        mouseX = lookInput.ReadValue<Vector2>().y;
        mouseY = lookInput.ReadValue<Vector2>().x;

        rotationX += mouseX * sensitivity * 0.1f;
        rotationY -= mouseY * sensitivity * 0.1f;

        if (enableCameraYRotation)
        {
            rotationX = Mathf.Clamp(rotationX, cameraClampMin, cameraClampMax);
            cameraT.localRotation = Quaternion.Euler(-rotationX, 0, 0);
        }
        else
        {
            cameraT.localRotation = Quaternion.Euler(0, 0, 0);
        }

        transform.rotation = Quaternion.Euler(0, -rotationY, 0);
    }
}
