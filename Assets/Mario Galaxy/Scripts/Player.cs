using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject _nearestPlanet;

    [Header("Player")]
    [SerializeField] GameObject camera;
    [SerializeField] float speed = 5.0f;

    [Header("Physics")]
    [SerializeField] private float _velocity = 0;
    [SerializeField] private float _velocityMax = 15f;

    void Start()
    {
        
    }

    void Update()
    {
        InputKeys();
    }
    
    void InputKeys()
    {
        float x = 0;
        float y = 0;
        float z = 0;
        if (Input.GetKey(KeyCode.W)) { z+= speed; }
        if (Input.GetKey(KeyCode.S)) { z -= speed; }
        if (Input.GetKey(KeyCode.A)) { x -= speed; }
        if (Input.GetKey(KeyCode.D)) { x += speed; }

        transform.localPosition += (new Vector3(x, 0, z) + transform.forward) * Time.deltaTime;
        transform.localRotation = new Quaternion(0, camera.transform.rotation.y, 0, 1);
    }
    
    
}