using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] VariableJoystick variableJoystick;
    [SerializeField] Rigidbody rb;
    [SerializeField] GameObject items;

    void FixedUpdate()
    {
        // Platform rotation speed
        // Rotate the platform based on the joystick
        //rb.AddTorque(transform.forward * variableJoystick.Horizontal * -speed * Time.fixedDeltaTime * 50000);
        //rb.AddTorque(transform.right * variableJoystick.Vertical * speed * Time.fixedDeltaTime * 50000);

        // Copy the platform rotation to the items of the platform
        //items.transform.rotation = transform.rotation;
    }
}
