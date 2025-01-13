using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGravity : MonoBehaviour
{

    [Header("Object variable")]
    public bool rotationAffected = false;
    public float objectMass = 1.0f;
    public float bounciness = 0.0f;
    public float fallOff = 0.99f;

    [Header("Planets Variables")]
    [SerializeField] private GameObject _nearestPlanet;
    [SerializeField] private PlanetScript _planetScript;
    [SerializeField] private float _planetMinDist;
    [SerializeField] private float _planetMaxDist;
    [SerializeField] private float _planetGravity = 9.8f;
    [SerializeField] private float _planetResistance = 0f;
    [SerializeField] private PlanetScript.PlanetShape planetShape;

    [Header("Planets Calculations")]
    public float gravity;
    public float planetGravDist;

    [Header("Gravity")]
    public bool grounded;
    public float groundDistance;
    public Vector3 velocity;
    public Vector3 downVector;
    public Vector3 lastPoint;

    [Header("Ray")]
    [SerializeField] private float _rayDist = 1.0f;
    [SerializeField] private LayerMask objectLayer;

    private void Start()
    {
        gameObject.AddComponent<Rigidbody>();
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        gameObject.GetComponent<Rigidbody>().useGravity = false;
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

        float _playerPosition = 0;
        switch (planetShape)
        {
            case PlanetScript.PlanetShape.Flat:
                downVector = new Vector3(0, -1, 0);
                _playerPosition = transform.position.y - _nearestPlanet.transform.position.y;
                break;

            case PlanetScript.PlanetShape.Round:
                downVector = (_nearestPlanet.transform.position - transform.position).normalized;
                _playerPosition = Vector3.Distance(_nearestPlanet.transform.position, transform.position);
                break;
        }
        Debug.DrawRay(transform.position, downVector, Color.blue, 0.01f);

        _playerPosition = Mathf.Clamp(_playerPosition, _planetMinDist, _planetMaxDist);
        float _playerPosNorm = (_playerPosition - _planetMaxDist) / (_planetMinDist - _planetMaxDist);
        gravity = _planetGravity * _playerPosNorm;
    }

    void CheckCollision()
    {
        if (RayCollision())
        {
            velocity = Vector3.zero;
            grounded = true;
        }
        else
        {
            grounded = false;
        }
    }

    void Gravity()
    {
        if (!grounded)
        {
            velocity += downVector * (gravity * objectMass);
            velocity *= _planetResistance;
            transform.position += velocity * Time.deltaTime;
        }
    }

    private bool RayCollision()
    {
        bool result = false;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, downVector, out hit, _rayDist, objectLayer)) 
        {
            //velocity = Vector3.Reflect(velocity, hit.normal) * bounciness;
            lastPoint = hit.point;
        }
        else
        {
            lastPoint = Vector3.positiveInfinity;
        }

        if (Vector3.Distance(lastPoint, transform.position) < groundDistance)
        {
            transform.position = lastPoint - downVector * (groundDistance - 0.01f);
            velocity.y = 0;
            result = true;
        }
        return result;
    }

    public void AddPlanet(GameObject newPlanet, PlanetScript.PlanetShape _planetShape)
    {
        _nearestPlanet = newPlanet;
        _planetScript = _nearestPlanet.GetComponent<PlanetScript>();
        _planetMinDist = _planetScript.minAttraDist;
        _planetMaxDist = _planetScript.maxAttraDist;
        _planetGravity = _planetScript.gravity;
        _planetResistance = _planetScript.airResistance;
        planetShape = _planetShape;
    }

    public void RemovePlanet(GameObject newPlanet)
    {
        _nearestPlanet = null;
        _planetScript = null;
        _planetMinDist = 1;
        _planetMaxDist = 1;
        _planetGravity = 0;
        _planetResistance = 1;
        planetShape = 0;

        gravity = 0;
    }

    public void AddImpulse(Vector3 impulse)
    {
        velocity += impulse;
        Vector3 deplacement = transform.forward * velocity.z + -downVector * velocity.y + transform.right * velocity.x;
        transform.localPosition += deplacement * Time.deltaTime;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(lastPoint, 0.5f);
        Gizmos.DrawRay(transform.position, downVector * _rayDist);
    }
}