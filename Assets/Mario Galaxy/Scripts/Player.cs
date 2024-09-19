using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject _nearestPlanet;

    [Header("Player")]
    [SerializeField] private GameObject _camera;
    [SerializeField] private GameObject _playerModel;
    [SerializeField] private float _speedVelocity = 0.1f;
    [SerializeField] private float _speed = 5.0f;
    [SerializeField] private float _jumpStrength = 5.0f;
    [SerializeField] private Vector3 _nextMovement;

    [Header("Physics")]
    [SerializeField] private Vector3 _velocity;
    [SerializeField] private ObjectGravity _playerGravity;

    void Start()
    {
        _playerGravity = GetComponent<ObjectGravity>();
    }

    void Update()
    {
        InputKeys();
        Position();
    }
    
    void InputKeys()
    {
        float x = 0;
        float y = 0;
        float z = 0;
        float _speedVelDelta = _speedVelocity * Time.deltaTime;
        if (Input.GetKey(KeyCode.W)) { z += _speedVelDelta; }
        if (Input.GetKey(KeyCode.S)) { z -= _speedVelDelta; }
        if (Input.GetKey(KeyCode.A)) { x -= _speedVelDelta; }
        if (Input.GetKey(KeyCode.D)) { x += _speedVelDelta; }

        if (_velocity.y == 0) 
        {
            if (Input.GetKey(KeyCode.Space)) { _playerGravity.Jump(_jumpStrength); }
            if (x == 0) { _velocity.x = Mathf.MoveTowards(_velocity.x, 0, _speedVelDelta); }
            if (z == 0) { _velocity.z = Mathf.MoveTowards(_velocity.z, 0, _speedVelDelta); }
        }
        y = _playerGravity.velocity;
        _velocity.y = y;

        _velocity += (new Vector3(x, 0, z));
        _velocity.x = Mathf.Clamp(_velocity.x, -_speed, _speed);
        _velocity.z = Mathf.Clamp(_velocity.z, -_speed, _speed);
    }

    void Position()
    {
        _playerModel.transform.localEulerAngles = new Vector3(0, _camera.transform.localEulerAngles.y);
        transform.rotation = Quaternion.LookRotation(_playerModel.transform.forward, -_playerGravity._downVector);

        float x = 0;
        float y = 0;
        float z = 0;
        if (transform.eulerAngles.x > 180) { x = transform.eulerAngles.x - 360; }
        if (transform.eulerAngles.y < -180) { x = transform.eulerAngles.x + 360; }
        if (transform.eulerAngles.y > 180) { y = transform.eulerAngles.y - 360; }
        if (transform.eulerAngles.y < -180) { y = transform.eulerAngles.y + 360; }
        if (transform.eulerAngles.z > 180) { z = transform.eulerAngles.z - 360; }
        if (transform.eulerAngles.z < -180) { z = transform.eulerAngles.z + 360; }
        
        transform.eulerAngles = new Vector3(x != 0 ? x : transform.eulerAngles.x,
                                                                          y != 0 ? y : transform.eulerAngles.y, 
                                                                          z != 0 ? z : transform.eulerAngles.z);

        _playerModel.transform.localRotation = new Quaternion();
        transform.Translate(_velocity * Time.deltaTime, Space.Self);

        Debug.DrawRay(transform.position, transform.forward * 2f, Color.blue, 0.01f);
        Debug.DrawRay(transform.position, transform.right * 2f, Color.red, 0.01f);
        Debug.DrawRay(transform.position, transform.up * 2f, Color.green, 0.01f);
    }
}
    