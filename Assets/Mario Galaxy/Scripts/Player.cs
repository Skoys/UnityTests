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
    [SerializeField] private float _speed = 5.0f;
    [SerializeField] private Vector3 _nextMovement;

    [Header("Physics")]
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
        if (Input.GetKey(KeyCode.W)) { z += _speed; }
        if (Input.GetKey(KeyCode.S)) { z -= _speed; }
        if (Input.GetKey(KeyCode.A)) { x -= _speed; }
        if (Input.GetKey(KeyCode.D)) { x += _speed; }

        y = -_playerGravity.velocity;

        _nextMovement = new Vector3(x, y, z);
    }

    void Position()
    {
        _playerModel.transform.localEulerAngles = new Vector3(0, _camera.transform.localEulerAngles.y);
        transform.rotation = Quaternion.LookRotation(_playerModel.transform.forward, transform.up);
        _playerModel.transform.rotation = new Quaternion();
        //transform.forward = _playerModel.transform.forward;
        transform.Translate(_nextMovement * Time.deltaTime, Space.Self);

        Debug.DrawRay(transform.position, transform.forward * 5f, Color.blue, 0.01f);
        Debug.DrawRay(transform.position, -transform.up * 5f, Color.yellow, 0.01f);
        Debug.DrawRay(transform.position, _nextMovement * 5f, Color.gray, 0.01f);
    }
}
    