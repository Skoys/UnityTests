using UnityEngine;
using UnityEngine.InputSystem;

public class QuaternionScript : MonoBehaviour
{

    //[SerializeField] private Quaternion quaternion = new Quaternion();
    [SerializeField] private float speed = 10f;
    [SerializeField] private float xTurnSpeed = 0.4f;
    [SerializeField] private float yTurnSpeed = 0.4f;
    [SerializeField] private float zTurnSpeed = 0.4f;
    //[SerializeField] private float inputXTurn = 0.0f;
    //[SerializeField] private float inputYTurn = 0.0f;
    //[SerializeField] private float inputZTurn = 0.0f;

    public Vector3 rotationAxis = new Vector3(); // Axis of rotation
    public float rotationAngle = 45f;         // Angle in degrees

    void Update()
    {
        //if (quaternion.x > 1) { quaternion.x -= 2; }
        //if (quaternion.x < -1) { quaternion.x += 2; }
        //if (quaternion.y > 1) { quaternion.y -= 2; }
        //if (quaternion.y < -1) { quaternion.y += 2; }
        //if (quaternion.z > 1) { quaternion.z -= 2; }
        //if (quaternion.z < -1) { quaternion.z += 2; }

        // Apply the rotation to the current rotation
        transform.rotation = MultiplyQuaternions(transform.rotation, TurnQuaternion(rotationAxis, rotationAngle));

        //transform.rotation = quaternion;

        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private Quaternion TurnQuaternion(Vector3 rotationAxis, float angle)
    {
        // Convert the angle to radians
        float angleRad = angle * Mathf.Deg2Rad * Time.deltaTime;

        // Normalize the axis of rotation
        Vector3 axis = rotationAxis.normalized;

        // Calculate the quaternion components manually
        float halfAngle = angleRad / 2f;
        float sinHalfAngle = Mathf.Sin(halfAngle);

        // Quaternion components
        float w = Mathf.Cos(halfAngle);
        float x = axis.x * sinHalfAngle;
        float y = axis.y * sinHalfAngle;
        float z = axis.z * sinHalfAngle;

        // Create the quaternion manually
        return new Quaternion(x, y, z, w);
    }

    // Manual quaternion multiplication
    Quaternion MultiplyQuaternions(Quaternion q1, Quaternion q2)
    {
        return new Quaternion(
            q1.w * q2.x + q1.x * q2.w + q1.y * q2.z - q1.z * q2.y, // x
            q1.w * q2.y - q1.x * q2.z + q1.y * q2.w + q1.z * q2.x, // y
            q1.w * q2.z + q1.x * q2.y - q1.y * q2.x + q1.z * q2.w, // z
            q1.w * q2.w - q1.x * q2.x - q1.y * q2.y - q1.z * q2.z  // w
        );
    }

    public void Pitch(InputAction.CallbackContext context)
    {
        rotationAxis.x = context.ReadValue<float>() * xTurnSpeed;
    }

    public void Roll(InputAction.CallbackContext context)
    {
        rotationAxis.z = context.ReadValue<float>() * zTurnSpeed;
    }

    public void Yaw(InputAction.CallbackContext context)
    {
        rotationAxis.y = context.ReadValue<float>() * yTurnSpeed;
    }
}
