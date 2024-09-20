using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGravity : MonoBehaviour
{

    [Header("Planets Variables")]
    [SerializeField] private float _planetMinDist;
    [SerializeField] private float _planetMaxDist;
    [SerializeField] private float _planetGravity = 9.8f;

    [Header("Planets Calculations")]
    [SerializeField] private float _gravity;
    [SerializeField] private float _planetGravDist;
    [SerializeField] private GameObject _nearestPlanet;
    [SerializeField] private PlanetScript _planetScript;

    [Header("Gravity")]
    public bool grounded;
    public float objectMass = 1.0f;
    public float bounciness = 0.0f;
    public float fallOff = 0.0f;
    public Vector3 velocity;
    public Vector3 downVector;

    [Header("Ray")]
    [SerializeField] private float _rayDist = 1.0f;
    [SerializeField] private LayerMask _floorLayer;

    private void Start()
    {
        _planetScript = _nearestPlanet.GetComponent<PlanetScript>();
        _planetMinDist = _planetScript.minAttraDist;
        _planetMaxDist = _planetScript.maxAttraDist;
        _planetGravity = _planetScript.gravity;
    }

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

        float _playerPosition = Vector3.Distance(_nearestPlanet.transform.position, transform.position);
        _playerPosition = Mathf.Clamp(_playerPosition, _planetMaxDist, _planetMinDist);
        float _playerPosNorm = (_playerPosition - _planetMaxDist) / (_planetMinDist - _planetMaxDist);
        _gravity = _planetGravity * _playerPosNorm;
    }

    void CheckCollision()
    {
        Ray ray = new Ray(transform.position, downVector);
        RaycastHit hit;

        if (Physics.Raycast(ray.origin, ray.direction, out hit, _rayDist, _floorLayer))
        {
            velocity.y = -velocity.y * bounciness;
            if(0.25f >velocity.y && velocity.y > -0.25f)
            {
                velocity.y = 0;
            }
            else
            {
                transform.Translate(new Vector3(0, _rayDist - hit.distance - 0.01f), Space.Self);
            }
            grounded = true;
        }
        else
        {
            grounded = false;
        }

        Debug.DrawRay(ray.origin, ray.direction * _rayDist, Color.red, 0.01f);
    }

    void Gravity()
    {
        if (!grounded)
        {
            velocity.y -= _gravity * objectMass * Time.deltaTime;
            if (velocity.y > 100) { velocity.y = 100; }
        }

        Vector3 deplacement = transform.forward * velocity.z + -downVector * velocity.y + transform.right * velocity.x;
        transform.localPosition += deplacement * Time.deltaTime;

        Vector3 _direction = (_nearestPlanet.transform.position - transform.position).normalized;
        Quaternion _newRotation = Quaternion.FromToRotation(-transform.up, _direction);
        transform.rotation = _newRotation * transform.rotation;

        Debug.DrawRay(transform.position, transform.forward * velocity.x, Color.blue, 0.01f);
        Debug.DrawRay(transform.position, transform.right * velocity.z, Color.red, 0.01f);
        Debug.DrawRay(transform.position, transform.up * velocity.y, Color.green, 0.01f);
        Debug.DrawRay(transform.position, deplacement, Color.white, 0.01f);
    }

    public void AddImpulse(Vector3 impulse)
    {
        velocity += impulse;
        Vector3 deplacement = transform.forward * velocity.z + -downVector * velocity.y + transform.right * velocity.x;
        transform.localPosition += deplacement * Time.deltaTime;
    }
}