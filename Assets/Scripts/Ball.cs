using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] VariableJoystick variableJoystick;
    [SerializeField] Rigidbody rb;

    void Start()
    {
        rb.AddForce(Vector3.down * 10000 * rb.mass);
    }

    void FixedUpdate()
    {
        Vector3 direction = Vector3.forward * variableJoystick.Vertical + Vector3.right * variableJoystick.Horizontal;
        rb.AddForce(direction * speed * Time.fixedDeltaTime, ForceMode.VelocityChange);
        rb.AddForce(Vector3.down * 100 * rb.mass);
    }
}
