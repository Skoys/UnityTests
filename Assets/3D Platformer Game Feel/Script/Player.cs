using UniSense;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

public class Player3D : MonoBehaviour
{
    [Header("Movements")]
    [SerializeField] private Vector2 movements = Vector2.zero;
    [SerializeField] private Vector2 speed = Vector2.zero;
    [SerializeField] private float rotationSpeed = 2;
    [SerializeField] private Transform orientation;
    [SerializeField] private float runTime = 1.5f;
    [SerializeField] private float currentRunTime = 0;

    [Header("Jump")]
    [SerializeField] private bool jumpPressed;
    public bool allowedToJump;
    public bool isJumping;
    [SerializeField] private float jumpBufferMaxTime, currentJumpBufferTime;
    public float jumpUpMaxTime ,currentJumpUpTime;
    [SerializeField] private int playerMask;
    [SerializeField] private float jumpForce;
    [SerializeField] private float maxCoyoteTime, currentCoyoteTime;
    [Tooltip("X = Normal, Y = Jumping, Z = Falling")]
    [SerializeField] private Vector3 gravityJump = Vector3.one;

    [Header("Dash")]
    [SerializeField] private float dashPressed;
    [SerializeField] private bool allowedToDash;
    [SerializeField] private bool canDash;
    [SerializeField] private Vector3 dashImpulse = Vector3.one;

    [Header("VFX")]
    [SerializeField] private VisualEffect walkVFX;
    [SerializeField] private VisualEffect runVFX;

    [Header("Rumbles")]
    [Tooltip("X = Left/Low, Y = Right/High, Z = Time || Left Big Vibrations, Right Small Vibrations")]
    [SerializeField] private Vector3 runRumble = Vector3.one;
    private bool runRumbleActivated;
    [Tooltip("X = Left/Low, Y = Right/High, Z = Time || Left Big Vibrations, Right Small Vibrations")]
    [SerializeField] private Vector3 dashRumble = Vector3.one;

    [Header("Elements")]
    [SerializeField] private GameObject shadowDecal;
    [SerializeField] private Player_Inputs player_Inputs;
    [SerializeField] private ObjectGravity objectGravity;
    [SerializeField] private PlayerCamera3D camera3D;
    private Rigidbody rb;
    public static Player3D instance;

    private void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(gameObject); }
    }

    void Start()
    {
        player_Inputs = Player_Inputs.instance;
        objectGravity = GetComponent<ObjectGravity>();
        camera3D = PlayerCamera3D.instance;
        rb = GetComponent<Rigidbody>();

        InputSystem.pollingFrequency = 120;
    }

    void Update()
    {
        GetInputs();
        Movement();
        Jump();
        Dash();
        Vfx();
    }

    private void GetInputs()
    {
        if (Vector2.Distance(Vector2.zero, player_Inputs.movement) > Vector2.Distance(Vector2.zero, movements)) movements = Vector2.MoveTowards(movements, player_Inputs.movement, Time.deltaTime * 2);
        else movements = Vector2.MoveTowards(movements, player_Inputs.movement, Time.deltaTime * 5);
        jumpPressed = player_Inputs.jumpPressed;
        dashPressed = player_Inputs.dashPressed;
    }

    private void Movement()
    {
        Vector3 viewDir = transform.position - new Vector3(camera3D.transform.position.x, transform.position.y, camera3D.transform.position.z);
        orientation.forward = viewDir.normalized;

        Vector3 direction = orientation.forward * movements.y + orientation.right * movements.x;
        direction.Normalize();

        if (direction != Vector3.zero)
            transform.forward = Vector3.Slerp(transform.forward, direction, Time.deltaTime * rotationSpeed);
        transform.rotation = Quaternion.LookRotation(transform.forward, transform.up);

        float absMovement = Mathf.Clamp(Mathf.Abs(movements.x) + Mathf.Abs(movements.y), 0, 1);
        float _speed = currentRunTime < runTime ? speed.x : speed.y;
        transform.position += transform.forward * absMovement * _speed * Time.deltaTime;

        if (absMovement > 0.75f)
        {
            if (!runRumbleActivated && currentRunTime >= runTime && CheckGround())
            {
                runRumbleActivated = true;
                player_Inputs.AddRumble(new Vector2(runRumble.x, runRumble.y), runRumble.z);
            }
            else if (currentRunTime < 4)
            {
                currentRunTime += Time.deltaTime;
            }
        }
        else
        {
            currentRunTime = Mathf.MoveTowards(currentRunTime, 0, Time.deltaTime * 2);
            if(currentRunTime< 0.1f) runRumbleActivated = false;
        }

        if (CheckGround()) { objectGravity.objectMass = gravityJump.x; }
        else
        {
            if (objectGravity.velocity.y < -0.1f) objectGravity.objectMass = gravityJump.z;
        }

        shadowDecal.transform.position = objectGravity.lastPoint - objectGravity.downVector.normalized * 0.5f;
     }

    private bool CheckGround()
    {
        return objectGravity.grounded;
    }

    private void Jump()
    {
        if (jumpPressed)
        {
            if(!allowedToJump) { currentJumpBufferTime = Time.time; allowedToJump = true; }
            if(currentJumpBufferTime >= Time.time - jumpBufferMaxTime && CheckGround())
            {
                objectGravity.objectMass = gravityJump.y;
                objectGravity.AddImpulse(new Vector3(0, jumpForce * objectGravity.gravity, 0));
                currentJumpBufferTime = Time.time;
                isJumping = true;
            }
            if (isJumping && currentJumpBufferTime >= Time.time - jumpBufferMaxTime)
            {
                objectGravity.AddImpulse(new Vector3(0, jumpForce * Time.deltaTime, 0));
            }
        }
        else
        {
            currentJumpBufferTime = Mathf.Infinity;
            allowedToJump = false;
            isJumping = false;
        }
    }

    private void Dash()
    {
        
        if (dashPressed > 0.45f && canDash && allowedToDash)
        {
            objectGravity.velocity = Vector3.zero;
            allowedToJump = true;
            allowedToDash = false;
            canDash = false;
            Vector3 _impulse = transform.up * dashImpulse.y + transform.forward * dashImpulse.z;
            objectGravity.AddImpulse(_impulse);
            player_Inputs.AddRumble(new Vector2(dashRumble.x, dashRumble.y), dashRumble.z);
            //DashTrigger(false);
        }
        if (!canDash) 
        { 
            canDash = CheckGround();
            //if(canDash)DashTrigger(canDash);
        }
        if (dashPressed < 0.1f) allowedToDash = true;
    }

    private void DashTrigger(bool isActive)
    {
        DualSenseGamepadState _state = player_Inputs.state;
        if (canDash)
        {
            DualSenseTriggerState rightTrigger = new();
            DualSenseSectionResistanceProperties rightResistance = new();
            rightResistance.StartPosition = 25;
            rightResistance.EndPosition = 50;
            rightResistance.Force = 50;
            rightTrigger.Section = rightResistance;
            rightTrigger.EffectType = DualSenseTriggerEffectType.SectionResistance;
            _state.RightTrigger = rightTrigger;
            Debug.Log("Unlocked");
        }
        else
        {
            DualSenseTriggerState rightTrigger = new();
            rightTrigger.EffectType = DualSenseTriggerEffectType.NoResistance;
            _state.RightTrigger = rightTrigger;
            Debug.Log("Locked");
        }
        player_Inputs.ChangeState(_state);
    }

    private void Vfx()
    {
        if (CheckGround())
        {
            if (currentRunTime > runTime) { runVFX.Play(); }
            else if (currentRunTime > 0) { walkVFX.Play(); }
            else { walkVFX.Stop(); runVFX.Stop(); }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.up * dashImpulse.y + transform.forward * dashImpulse.z);
    }
}
