using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGravity : MonoBehaviour
{
    [Header("Planets Calculations")]
    [SerializeField] private float _planetGravity;
    [SerializeField] private GameObject _nearestPlanet;

    [Header("Gravity")]
    public Vector3 velocity;
    public Vector3 downVector;

    [SerializeField] private float _objectMass = 1.0f;
    [SerializeField] private float _bounciness = 0.0f;
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
            downVector = new Vector3(0, -1, 0);
            return;
        }
        downVector = _nearestPlanet.transform.position - transform.position;
        Debug.DrawRay(transform.position, downVector, Color.blue, 0.01f);
    }

    void CheckCollision()
    {
        Ray ray = new Ray(transform.position, downVector);
        RaycastHit hit;

        if (Physics.Raycast(ray.origin, ray.direction, out hit, _rayDist, _floorLayer))
        {
            velocity.y = -velocity.y * _bounciness;
            if(velocity.y < 1f && velocity.y > -1f)
            {
                velocity.y = 0;
            }
            transform.Translate(new Vector3(0, hit.distance - _rayDist - 0.01f), Space.Self);
            _grounded = true;
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
            velocity.y -= _planetGravity * _objectMass * Time.deltaTime;
            if (velocity.y > 100) { velocity.y = 100; }
        }

        Vector3 _direction = (_nearestPlanet.transform.position - transform.position).normalized;
        Quaternion _newRotation = Quaternion.FromToRotation(-transform.up, _direction);
        transform.rotation = _newRotation * transform.rotation;

        transform.Translate(velocity * Time.deltaTime, Space.Self);

        Debug.DrawRay(transform.position, transform.forward * 2f, Color.blue, 0.01f);
        Debug.DrawRay(transform.position, transform.right * 2f, Color.red, 0.01f);
        Debug.DrawRay(transform.position, transform.up * 2f, Color.green, 0.01f);
    }
}