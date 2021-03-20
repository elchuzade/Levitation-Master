using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] VariableJoystick variableJoystick;
    [SerializeField] Rigidbody rb;

    [SerializeField] GameObject items;

    public void FixedUpdate()
    {
        // Rotate the platform based on the joystick
        rb.AddTorque(transform.forward * variableJoystick.Horizontal * -speed * Time.fixedDeltaTime * 10000);
        rb.AddTorque(transform.right * variableJoystick.Vertical * speed * Time.fixedDeltaTime * 10000);

        // Copy the platform rotation to the items of the platform
        items.transform.rotation = transform.rotation;
    }
}
