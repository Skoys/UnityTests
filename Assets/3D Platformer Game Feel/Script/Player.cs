using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XInput;

public class Player3D : MonoBehaviour
{
    [Header("Movements")]
    [SerializeField] private Vector2 movements = Vector2.zero;
    [SerializeField] private float speed = 4;
    [SerializeField] private float rotationSpeed = 2;
    [SerializeField] private Transform orientation;

    [Header("Jump")]
    [SerializeField] private bool jumpPressed;
    private bool alreadyPressed;
    [SerializeField] private float jumpBufferMaxTime;
    private float currentJumpBufferTime;
    [SerializeField] private float groundTestDistance;

    private Rigidbody rb;

    [SerializeField] private Player_Inputs player_Inputs;
    [SerializeField] private PlayerCamera3D camera3D;
    public static Player3D instance;

    private void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(gameObject); }
    }

    void Start()
    {
        player_Inputs = Player_Inputs.instance;
        camera3D = PlayerCamera3D.instance;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        GetInputs();
        Movement();
        Jump();
    }

    private void GetInputs()
    {
        movements = player_Inputs.movement;
        jumpPressed = player_Inputs.jumpPressed;
    }

    private void Movement()
    {
        Vector3 viewDir = transform.position - new Vector3(camera3D.transform.position.x, transform.position.y, camera3D.transform.position.z);
        orientation.forward = viewDir.normalized;

        Vector3 direction = orientation.forward * movements.y + orientation.right * movements.x;
        direction.Normalize();

        if (direction != Vector3.zero)
            transform.forward = Vector3.Slerp(transform.forward, direction, Time.deltaTime * rotationSpeed);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

        float absMovement = Mathf.Clamp(Mathf.Abs(movements.x) + Mathf.Abs(movements.y), 0, 1);
        transform.position += transform.forward * absMovement * speed * Time.deltaTime;
    }

    private void Jump()
    {
        if (jumpPressed)
        {
            if(!alreadyPressed) { currentJumpBufferTime = Time.time; alreadyPressed = true; }
        }
        else
        {
            currentJumpBufferTime = 0;
            alreadyPressed = false;
        }
    }
}
