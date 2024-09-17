using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGravity : MonoBehaviour
{
    [Header("Planets Calculations")]
    [SerializeField] private float _planetGravity;
    [SerializeField] private GameObject _nearestPlanet;

    [Header("Gravity")]
    [SerializeField] private float _objectMass = 1.0f;
    [SerializeField] private bool _grounded;
    [SerializeField] private float _velocity;
    [SerializeField] private Vector3 _downVector;

    [Header("Ray")]
    [SerializeField] private float _rayDist = 1.0f;
    [SerializeField] private LayerMask _floorLayer;

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
        Debug.DrawRay(transform.position, _downVector * 10f, Color.blue, 0.01f);
    }

    void CheckCollision()
    {
        Ray ray = new Ray(transform.position, _downVector);
        RaycastHit hit;

        if (Physics.Raycast(ray.origin, ray.direction, out hit, _rayDist, _floorLayer))
        {
            _grounded = true;
            _velocity = 0;
            transform.position += new Vector3(0, _rayDist - hit.distance - 0.001f);
        }
        else
        {
            _grounded = false;
        }

        Debug.DrawRay(ray.origin, ray.direction * _rayDist, Color.red, 0.01f);
    }

    void Gravity()
    {
        if (!_grounded)
        {
            _velocity += _planetGravity * _objectMass * Time.deltaTime;
            if (_velocity > 100) { _velocity = 100; }
        }

        //Vector3 directionDifference = transform.position - _lastPosition;

        transform.position += _downVector * _velocity * _objectMass * Time.deltaTime;
        //transform.localRotation *= (Quaternion.Euler(_downVector) * Quaternion.Euler(directionDifference));
        transform.rotation = Quaternion.LookRotation(transform.forward, -_downVector);

        Debug.DrawRay(transform.position, transform.forward * 10f, Color.green, 0.01f);
    } 
}
