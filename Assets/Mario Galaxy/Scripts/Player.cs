using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject _nearestPlanet;

    [Header("Player")]
    [SerializeField] GameObject camera;
    [SerializeField] float speed = 5.0f;

    [Header("Physics")]
    [SerializeField] private bool _onGround = true;
    [SerializeField] private float _velocity = 0;
    [SerializeField] private float _velocityMax = 15f;
    [SerializeField] private float _currentGravity = 9.8f;
    [SerializeField] private float _currentMass = 1.0f;
    [SerializeField] private Vector3 _downVector = Vector3.zero;
    [SerializeField] private Vector3 _lastPosition = Vector3.zero;

    [Header("Ray")]
    [SerializeField] private float _rayDist = 1.0f;
    [SerializeField] private LayerMask _floorLayer;

    [Header("Debug")]
    [SerializeField] private Quaternion debug = new Quaternion();

    void Start()
    {
        
    }

    void Update()
    {
        PlanetCalculations();
        InputKeys();
        CheckCollision();
        Gravity();
    }
    void PlanetCalculations()
    {
        if (_nearestPlanet == null)
        {
            _downVector = new Vector3(0, -1, 0);
            return;
        }
        
        _downVector = _nearestPlanet.transform.position - transform.position;
    }
    void InputKeys()
    {
        float x = 0;
        float y = 0;
        float z = 0;
        if (Input.GetKey(KeyCode.W)) { z+= speed; }
        if (Input.GetKey(KeyCode.S)) { z -= speed; }
        if (Input.GetKey(KeyCode.A)) { x -= speed; }
        if (Input.GetKey(KeyCode.D)) { x += speed; }

        Vector3 newForward = new Vector3(camera.transform.forward.x, 0, camera.transform.forward.z);

        transform.localPosition += (new Vector3(x, 0, z) + newForward) * Time.deltaTime;

    }
    void Gravity()
    {
        if (!_onGround)
        {
            _velocity += _currentGravity * Time.deltaTime;
            if (_velocity > _velocityMax) { _velocity = _velocityMax; }
        }

        Vector3 loctoworld = camera.transform.TransformDirection(new Vector3(camera.transform.localRotation.x, 0f, camera.transform.localRotation.z));
        Vector3 newForward = new Vector3(camera.transform.forward.x, 0, camera.transform.forward.z);

        transform.position += _downVector * _velocity * _currentMass * Time.deltaTime;
        //transform.rotation = Quaternion.LookRotation(_downVector, -_downVector);
        transform.rotation = Quaternion.LookRotation(loctoworld, -_downVector);
        debug = transform.localRotation;

        Debug.DrawRay(camera.transform.position, loctoworld * 10f, Color.green, 0.01f);
        Debug.DrawRay(transform.position, transform.forward * 10f, Color.green, 0.01f);
    }
    void CheckCollision()
    {
        Ray ray = new Ray(transform.position, _downVector);
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * _rayDist, Color.red, 0.01f);
        Debug.DrawRay(ray.origin, ray.direction, Color.blue, 0.01f);

        if (Physics.Raycast(ray.origin, ray.direction, out hit, _rayDist, _floorLayer))
        {
            _onGround = true;
            _velocity = 0;
            transform.position += new Vector3(0, _rayDist - hit.distance -0.001f);
        }
        else
        {
            _onGround = false;
        }
    }
}