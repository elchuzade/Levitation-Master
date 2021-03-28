using UnityEngine;

public class Move : MonoBehaviour
{
    // Speed with which the movement will occur
    [SerializeField] float speed;
    // To save position of object to return to
    [SerializeField] Transform initPosition;
    // Indicator of movement, back - forth
    int directionFactor = 1;
    bool towardsMove = true;
    [SerializeField] Rigidbody rb;

    void FixedUpdate()
    {
        // Do not let the force accumulate. Restart it every frame and set again
        rb.velocity = Vector3.zero;
        Vector3 directionVector = (transform.position - initPosition.position).normalized;
        rb.AddForce(directionVector * speed * directionFactor * Time.fixedDeltaTime * 1000);

        if (Vector3.Distance(rb.transform.position, transform.position) < 10 && towardsMove)
        {
            // Reached movePosition, reverse the movement to go back
            directionFactor = -1;
            towardsMove = false;
        }
        else if (Vector3.Distance(rb.transform.position, initPosition.position) < 10 && !towardsMove)
        {
            // Reached initPosition, reverse the movement to go forth
            directionFactor = 1;
            towardsMove = true;
        }
    }
}
