using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_FPS : MonoBehaviour
{
    [SerializeField] private GameObject camera;
    [SerializeField] private float sensitivity = 1.0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float rotateHorizontal = Input.GetAxis("Mouse X");
        float rotateVertical = Input.GetAxis("Mouse Y");

        Vector3 rotation = new Vector3(rotateVertical * sensitivity, -rotateHorizontal * sensitivity, 0);

        camera.transform.eulerAngles -= rotation;
    }
}
