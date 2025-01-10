using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    [SerializeField] private Transform player;

    public float one;
    public float two;
    void Update()
    {
        one = Vector3.Dot(transform.forward, player.forward);
        Vector3 inverse = transform.InverseTransformDirection(player.position - transform.position);
        two = inverse.z;
        if (Vector3.Dot(transform.forward, player.forward) < 0 && inverse.z > 0)
        {
            GetComponent<MeshRenderer>().enabled = false;
        }
        else
        {
            GetComponent<MeshRenderer>().enabled = true;
        }
    }
}
