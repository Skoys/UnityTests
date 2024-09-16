using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuaternionTest : MonoBehaviour
{
    [SerializeField] private Quaternion quaternion = Quaternion.identity;

    // Update is called once per frame
    void Update()
    {
        transform.rotation = quaternion;
    }
}
