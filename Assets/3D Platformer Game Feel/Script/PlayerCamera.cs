using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera3D : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private Vector2 maxCameraSpeed = Vector2.one;
    [SerializeField] private Vector2 cameraAcceleration = Vector2.one;
    [SerializeField] private Vector2 currentCameraSpeed = Vector2.one;
    [SerializeField] private Vector2 cameraInputs = Vector2.zero;
    [SerializeField] private Vector2 maxUpDownCamera = Vector2.one;

    private Player_Inputs playerInputs;
    [SerializeField] private Player3D player3D;

    // Start is called before the first frame update
    void Start()
    {
        playerInputs = Player_Inputs.instance;
        player3D = Player3D.instance;
    }

    // Update is called once per frame
    void Update()
    {
        GetInputs();
        UpdateCamera();
    }

    private void GetInputs()
    {
        cameraInputs = playerInputs.camMovement;
    }

    private void UpdateCamera()
    {
        cameraInputs.x = Mathf.Clamp(cameraInputs.x, -0.75f, 0.75f);
        cameraInputs.y = Mathf.Clamp(cameraInputs.y, -0.75f, 0.75f);

        cameraInputs.x *= maxCameraSpeed.x * -1;
        cameraInputs.y *= maxCameraSpeed.y;

        if (Mathf.Abs(cameraInputs.x) > 0.15f) { currentCameraSpeed.x = Mathf.Lerp(currentCameraSpeed.x, cameraInputs.x, cameraAcceleration.x * Time.deltaTime); }
        else { currentCameraSpeed.x = Mathf.Lerp(currentCameraSpeed.x, 0, cameraAcceleration.x); }

        if (Mathf.Abs(cameraInputs.y) > 0.15f) { currentCameraSpeed.y = Mathf.Lerp(currentCameraSpeed.y, cameraInputs.y, cameraAcceleration.y * Time.deltaTime); }
        else { currentCameraSpeed.y = Mathf.Lerp(currentCameraSpeed.y, 0, cameraAcceleration.y); }

        gameObject.transform.RotateAround(player3D.gameObject.transform.position, new Vector3(1,0,0), currentCameraSpeed.y);
        gameObject.transform.RotateAround(player3D.gameObject.transform.position, new Vector3(0,1,0), currentCameraSpeed.x);

        transform.eulerAngles = new Vector3(Mathf.Clamp(transform.eulerAngles.x, maxUpDownCamera.x, maxUpDownCamera.y), transform.eulerAngles.y, 0);
        transform.LookAt(player3D.transform, transform.up);
    }
}
