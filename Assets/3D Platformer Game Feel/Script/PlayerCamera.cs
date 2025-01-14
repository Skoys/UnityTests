using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCamera3D : MonoBehaviour
{
    [Header("Camera Movement")]
    [SerializeField] private Vector2 maxCameraSpeed = Vector2.one;
    [SerializeField] private Vector2 cameraAcceleration = Vector2.one;
    [SerializeField] private float cameraAccelerationSpeed;
    [SerializeField] private Vector2 currentCameraPos = Vector2.one;
    [SerializeField] private Vector2 cameraInputs = Vector2.zero;
    [SerializeField] private Vector3 pivotPoint;
    [SerializeField] private Vector2 maxTopDown;

    [Header("Camera Distance")]
    [SerializeField] private float distance;
    [SerializeField] private Vector2 minMaxDistance = Vector2.one;
    [SerializeField] private float zoomMultiplier;
    [SerializeField] private float zoomInput;

    private Player_Inputs playerInputs;
    [SerializeField] private Player3D player3D;

    public static PlayerCamera3D instance;

    private void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(gameObject); }
    }

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
        cameraInputs = playerInputs.camMovement * -1;
        zoomInput = playerInputs.zoom;
    }

    private void UpdateCamera()
    {
        cameraInputs.x = Mathf.Clamp(cameraInputs.x, -0.75f, 0.75f);
        cameraInputs.y = Mathf.Clamp(cameraInputs.y, -0.75f, 0.75f);

        cameraAcceleration.x = Mathf.MoveTowards(cameraAcceleration.x, cameraInputs.x * (1 / 0.75f), cameraAccelerationSpeed * Time.deltaTime);
        cameraAcceleration.y = Mathf.MoveTowards(cameraAcceleration.y, cameraInputs.y * (1 / 0.75f), cameraAccelerationSpeed * Time.deltaTime);

        cameraAcceleration.x = Mathf.Clamp(cameraAcceleration.x, -maxCameraSpeed.x, maxCameraSpeed.x);
        cameraAcceleration.y = Mathf.Clamp(cameraAcceleration.y, -maxCameraSpeed.y, maxCameraSpeed.y);

        currentCameraPos += new Vector2(cameraAcceleration.x * maxCameraSpeed.x, cameraAcceleration.y * maxCameraSpeed.y) * Time .deltaTime;
        currentCameraPos.y = Mathf.Clamp(currentCameraPos.y, maxTopDown.y, maxTopDown.x);

        distance += zoomInput * zoomMultiplier * Time.deltaTime;
        distance = Mathf.Clamp(distance, minMaxDistance.x, minMaxDistance.y);

        float D = Mathf.Sqrt(Mathf.Pow(distance * -10, 2) - Mathf.Pow(distance - (player3D.transform.position.y + pivotPoint.y - transform.position.y), 2));
        float X = player3D.transform.position.x + pivotPoint.x + Mathf.Cos(currentCameraPos.x) * D;
        float Z = player3D.transform.position.z + pivotPoint.z + Mathf.Sin(currentCameraPos.x) * D;
        transform.position = Vector3.Lerp(transform.position, new Vector3(X, player3D.transform.position.y + pivotPoint.y + currentCameraPos.y, Z), Time.deltaTime * 3);
        transform.LookAt(player3D.transform.position + pivotPoint, player3D.transform.up);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(player3D.transform.position + pivotPoint, 0.2f);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(player3D.transform.position + pivotPoint + new Vector3(0, maxTopDown.x), player3D.transform.position + pivotPoint + new Vector3(0, maxTopDown.y));
        Gizmos.DrawLine(transform.position, new Vector3(player3D.transform.position.x + pivotPoint.x, transform.position.y, player3D.transform.position.z + pivotPoint.z));
    }
}
