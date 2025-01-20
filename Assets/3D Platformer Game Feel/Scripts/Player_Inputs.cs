using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UniSense;

public class Player_Inputs : MonoBehaviour
{
    [Header("Debug")]
    public bool jumpPressed = false;
    public bool attackPressed = false;
    public bool interactionPressed = false;
    public float dashPressed = 0f;
    public bool pausePressed = false;
    public Vector2 movement = Vector2.zero;
    public Vector2 camMovement = Vector2.zero;
    public float zoom = 0;
    public float menu = 0f;
    [SerializeField] private List<float[]> rumbleList = new List<float[]>();

    public static Player_Inputs instance;

    [Header("DualSense")]
    public DualSenseGamepadHID dualSense;
    public DualSenseGamepadState state;

    private void Awake()
    {
        if ( instance == null ) { instance = this; }
        else { Destroy(gameObject); }
    }

    private void Start()
    {
        dualSense = DualSenseGamepadHID.FindFirst();
        state = ResetDualSense();
        dualSense?.SetGamepadState(state);
        dualSense?.SetLightBarColor(Color.blue);
    }

    private void FixedUpdate()
    {
        Rumble();
    }

    void Rumble()
    {
        if (Gamepad.current == null) { return; }
        if (rumbleList.Count == 0) { Gamepad.current.SetMotorSpeeds(0, 0); return; }
        while (rumbleList[0][2] < Time.realtimeSinceStartup)
        {
            rumbleList.RemoveAt(0);
            if(rumbleList.Count == 0 ) { return; }
        }
        if (rumbleList.Count > 0)
        {
            Gamepad.current.SetMotorSpeeds(rumbleList[0][0], rumbleList[0][1]);
        }
    }

    public void AddRumble(Vector2 rumble, float time)
    {
        float[] list = { rumble.x, rumble.y, time + Time.realtimeSinceStartup };
        rumbleList.Add(list);
    }

    public void Movement(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
    }

    public void CamMovement(InputAction.CallbackContext context)
    {
        camMovement = context.ReadValue<Vector2>();
    }

    public void Zoom(InputAction.CallbackContext context)
    {
        zoom = context.ReadValue<float>();
    }

    public void Menu(InputAction.CallbackContext context)
    {
        menu = context.ReadValue<float>();
    }

    public void Attack(InputAction.CallbackContext context)
    {
        attackPressed = context.ReadValue<float>() > 0 ? true : false;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        jumpPressed = context.ReadValue<float>() > 0 ? true : false;
    }
    
    public void Interact(InputAction.CallbackContext context)
    {
        interactionPressed = context.ReadValue<float>() > 0 ? true : false;
    }
    
    public void Dash(InputAction.CallbackContext context)
    {
        dashPressed = context.ReadValue<float>();
    }

    public void Pause(InputAction.CallbackContext context)
    {
        pausePressed = context.ReadValue<float>() > 0 ? true : false;
    }

    public void ChangeState(DualSenseGamepadState _state)
    {
        state = _state;
        dualSense?.SetGamepadState(state);
        Debug.Log("State Changed");
    }

    private DualSenseGamepadState ResetDualSense()
    {
        DualSenseGamepadState _state = new();

        #region State Light Bar Color
        _state.LightBarColor = Color.blue;
        #endregion
        #region State Motor
        DualSenseMotorSpeed motorSpeed = new();
        motorSpeed.LowFrequencyMotorSpeed = 0f;
        motorSpeed.HighFrequenceyMotorSpeed = 0f;
        _state.Motor = motorSpeed;
        #endregion
        #region State Mic Led
        _state.MicLed = DualSenseMicLedState.Off;
        #endregion

        #region Triggers
        DualSenseTriggerState leftTriggerState = new();
        leftTriggerState.EffectType = DualSenseTriggerEffectType.NoResistance;
        
        DualSenseTriggerState rightTriggerState = new();
        rightTriggerState.EffectType = DualSenseTriggerEffectType.NoResistance;

        DualSenseContinuousResistanceProperties continuous = new();
        continuous.StartPosition = 0;
        continuous.Force = 0;

        DualSenseSectionResistanceProperties resistance = new();
        resistance.StartPosition = 0;
        resistance.EndPosition = 0;
        resistance.Force = 0;

        DualSenseEffectExProperties ex = new();
        ex.StartPosition = 0;
        ex.KeepEffect = false;
        ex.BeginForce = 0;
        ex.MiddleForce = 0;
        ex.EndForce = 0;
        ex.Frequency = 0;

        leftTriggerState.Continuous = continuous;
        leftTriggerState.Section = resistance;
        leftTriggerState.EffectEx = ex;

        rightTriggerState.Continuous = continuous;
        rightTriggerState.Section = resistance;
        rightTriggerState.EffectEx = ex;

        _state.LeftTrigger = leftTriggerState;
        _state.RightTrigger = rightTriggerState;
        #endregion

        #region State Led Brightness
        _state.PlayerLedBrightness = PlayerLedBrightness.Medium;
        #endregion
        #region State Led State
        PlayerLedState ledState = new PlayerLedState(false, false, false, false, false);
        _state.PlayerLed = ledState;
        #endregion

        return _state;
    }
}