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
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        PlanetCalculations();
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
    void Gravity()
    {
        if (!_onGround)
        {
            _velocity += _currentGravity * Time.deltaTime;
            if (_velocity > _velocityMax) { _velocity = _velocityMax; }

            transform.position += _downVector * _velocity * _currentMass * Time.deltaTime;
            transform.rotation = Quaternion.LookRotation(_downVector, -Vector3.up);
        }
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