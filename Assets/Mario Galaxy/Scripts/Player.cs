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
        if (Input.GetKey(KeyCode.W)) { z += _speedVelocity; }
        if (Input.GetKey(KeyCode.S)) { z -= _speedVelocity; }
        if (Input.GetKey(KeyCode.A)) { x -= _speedVelocity; }
        if (Input.GetKey(KeyCode.D)) { x += _speedVelocity; }

        if (_velocity.y == 0) 
        {
            if (Input.GetKey(KeyCode.Space)) { _playerGravity.Jump(_jumpStrength); }
            if (x == 0) { _velocity.x = Mathf.MoveTowards(_velocity.x, 0, _speedVelocity); }
            if (z == 0) { _velocity.z = Mathf.MoveTowards(_velocity.z, 0, _speedVelocity); }
        }
        y = -_playerGravity.velocity;
        _velocity.y = y;

        _velocity += (new Vector3(x, 0, z));
        _velocity.x = Mathf.Clamp(_velocity.x, -_speed, _speed);
        _velocity.z = Mathf.Clamp(_velocity.z, -_speed, _speed);
    }

    void Position()
    {
        _playerModel.transform.localEulerAngles = new Vector3(0, _camera.transform.localEulerAngles.y);
        transform.rotation = Quaternion.LookRotation(_playerModel.transform.forward, transform.up);
        _playerModel.transform.localRotation = new Quaternion();
        //transform.forward = _playerModel.transform.forward;
        transform.Translate(_velocity * Time.deltaTime, Space.Self);

        Debug.DrawRay(transform.position, transform.forward * 5f, Color.blue, 0.01f);
        Debug.DrawRay(transform.position, -transform.up * 5f, Color.yellow, 0.01f);
        Debug.DrawRay(transform.position, _nextMovement * 5f, Color.gray, 0.01f);
    }
}
    