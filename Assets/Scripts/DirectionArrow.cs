using UnityEngine;

public class DirectionArrow : MonoBehaviour
{
    [SerializeField] GameObject ball;
    [SerializeField] VariableJoystick variableJoystick;
    [SerializeField] GameObject idleDirectionArrows;

    bool tracking;

    public Vector3 direction;

    void Update()
    {
        if (tracking)
        {
            if (variableJoystick.dragging)
            {
                direction = Vector3.up * variableJoystick.Vertical + Vector3.right * variableJoystick.Horizontal;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                transform.localRotation = Quaternion.Euler(90, 0, angle - 90);
            }

            transform.position = ball.transform.position - new Vector3(0, 25, 0);
        }
    }

    // @access from Ball script when it hits the platform
    public void StartTracking()
    {
        idleDirectionArrows.SetActive(false);
        GetComponent<SpriteRenderer>().enabled = true;
        tracking = true;
    }
}
