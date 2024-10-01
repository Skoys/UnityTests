using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGravity : MonoBehaviour
{

    [Header("Object variable")]
    public bool rotationAffected = false;
    public float objectMass = 1.0f;
    public float bounciness = 0.0f;
    public float fallOff = 0.0f;

    [Header("Planets Variables")]
    [SerializeField] private GameObject _nearestPlanet;
    [SerializeField] private PlanetScript _planetScript;
    [SerializeField] private float _planetMinDist;
    [SerializeField] private float _planetMaxDist;
    [SerializeField] private float _planetGravity = 9.8f;
    [SerializeField] private float _planetResistance = 0f;

    [Header("Planets Calculations")]
    [SerializeField] private float _gravity;
    [SerializeField] private float _planetGravDist;

    [Header("Gravity")]
    public bool grounded;
    public Vector3 velocity;
    public Vector3 downVector;
    private Vector3 oldPos;
    public Vector3 newVelo;

    [Header("Ray")]
    [SerializeField] private float _rayDist = 1.0f;
    [SerializeField] private LayerMask _floorLayer;

    private void Start()
    {
        gameObject.AddComponent<Rigidbody>();
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
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
        downVector = (_nearestPlanet.transform.position - transform.position).normalized;
        Debug.DrawRay(transform.position, downVector, Color.blue, 0.01f);

        float _playerPosition = Vector3.Distance(_nearestPlanet.transform.position, transform.position);
        _playerPosition = Mathf.Clamp(_playerPosition, _planetMinDist, _planetMaxDist);
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

        newVelo += downVector * (_gravity * objectMass);
        newVelo *= _planetResistance;
        transform.position += newVelo * Time.deltaTime;

        //if (!grounded)
        //{
        //    velocity.y -= _gravity * objectMass * Time.deltaTime;
        //    if (velocity.y > 100) { velocity.y = 100; }
        //}

        //Vector3 deplacement = transform.forward * velocity.z + -downVector * velocity.y + transform.right * velocity.x;
        //transform.localPosition += deplacement * Time.deltaTime;

        //if (_nearestPlanet != null)
        //{
        //    Vector3 _direction = (_nearestPlanet.transform.position - transform.position).normalized;
        //    Quaternion _newRotation = Quaternion.FromToRotation(-transform.up, _direction);
        //    transform.rotation = _newRotation * transform.rotation;
        //}

        //oldPos = transform.position;

        //Debug.DrawRay(transform.position, transform.forward * velocity.x, Color.blue, 0.01f);
        //Debug.DrawRay(transform.position, transform.right * velocity.z, Color.red, 0.01f);
        //Debug.DrawRay(transform.position, transform.up * velocity.y, Color.green, 0.01f);
        //Debug.DrawRay(transform.position, deplacement, Color.white, 0.01f);
    }

    public void AddPlanet(GameObject newPlanet)
    {
        _nearestPlanet = newPlanet;
        _planetScript = _nearestPlanet.GetComponent<PlanetScript>();
        _planetMinDist = _planetScript.minAttraDist;
        _planetMaxDist = _planetScript.maxAttraDist;
        _planetGravity = _planetScript.gravity;
        _planetResistance = _planetScript.airResistance;
    }

    public void RemovePlanet(GameObject newPlanet)
    {
        _nearestPlanet = null;
        _planetScript = null;
        _planetMinDist = 0;
        _planetMaxDist = 0;
        _planetGravity = 0;
        _planetResistance = 1;
    }

    public void AddImpulse(Vector3 impulse)
    {
        velocity += impulse;
        Vector3 deplacement = transform.forward * velocity.z + -downVector * velocity.y + transform.right * velocity.x;
        transform.localPosition += deplacement * Time.deltaTime;
    }
}