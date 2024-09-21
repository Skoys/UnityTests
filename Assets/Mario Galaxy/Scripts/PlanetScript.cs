using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetScript : MonoBehaviour
{
    public float minAttraDist;
    public float maxAttraDist;
    public float gravity = 9.8f;

    private void Start()
    {
        minAttraDist = transform.lossyScale.x * 0.5f;
        if(minAttraDist > transform.lossyScale.y) { minAttraDist = transform.lossyScale.y * 0.5f; }
        if(minAttraDist > transform .lossyScale.z) { minAttraDist = transform .lossyScale.z * 0.5f; }
        maxAttraDist = minAttraDist * 3;

        gameObject.AddComponent<SphereCollider>();
        gameObject.GetComponent<SphereCollider>().isTrigger = true;
        gameObject.GetComponent<SphereCollider>().radius = maxAttraDist / transform.lossyScale.x;
    }

    private void Update()
    {
        gameObject.GetComponent<SphereCollider>().radius = maxAttraDist / transform.lossyScale.x;
        Debug.DrawRay(transform.position, transform.up * maxAttraDist, Color.red, 0.01f);
        Debug.DrawRay(transform.position, transform.up * minAttraDist, Color.black, 0.01f);
    }

    private void OnTriggerEnter(Collider other)
    {
            other.GetComponent<ObjectGravity>().AddPlanet(gameObject);
            Debug.Log("RigidBody entered");
    }

    private void OnTriggerExit(Collider other)
    {
            other.GetComponent<ObjectGravity>().RemovePlanet(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, maxAttraDist);
    }
}
