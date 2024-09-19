using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuaternionTest : MonoBehaviour
{
    [SerializeField] private Quaternion quaternion = Quaternion.identity;
    [SerializeField] private GameObject planet;

    // Update is called once per frame
    void Update()
    {
        Vector3 directionToTarget = (planet.transform.position - transform.position).normalized;

        Quaternion rotationToTarget = Quaternion.FromToRotation(transform.up, directionToTarget);

        transform.rotation = rotationToTarget * transform.rotation;
    }
}
