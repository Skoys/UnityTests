using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGravity : MonoBehaviour
{
    [Header("Planets Calculations")]
    [SerializeField] private float _planetGravity;
    [SerializeField] private GameObject _nearestPlanet;

    [Header("Gravity")]
    public float velocity;
    public Vector3 _downVector;

    [SerializeField] private float _objectMass = 1.0f;
    [SerializeField] private bool _grounded;

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
        Debug.DrawRay(transform.position, _downVector, Color.blue, 0.01f);
    }

    void CheckCollision()
    {
        Ray ray = new Ray(transform.position, _downVector);
        RaycastHit hit;

        if (Physics.Raycast(ray.origin, ray.direction, out hit, _rayDist, _floorLayer))
        {
            _grounded = true;
            velocity = 0;
            //transform.localPosition += new Vector3(0, _rayDist - hit.distance - 0.001f);
            transform.Translate(new Vector3(0, _rayDist - hit.distance - 0.001f), Space.Self);
        }
        else
        {
            _grounded = false;
        }

        //Debug.DrawRay(ray.origin, ray.direction * _rayDist, Color.red, 0.01f);
    }

    void Gravity()
    {
        if (!_grounded)
        {
            velocity -= _planetGravity * _objectMass * Time.deltaTime;
            if (velocity > 100) { velocity = 100; }
        }

        transform.up = -_downVector;
    }

    public void Jump(float strength)
    {
        velocity = strength;
    }
}
