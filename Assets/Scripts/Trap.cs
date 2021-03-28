using UnityEngine;

public class Trap : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ball")
        {
            other.gameObject.GetComponent<Ball>().AttemptTrapBall();
            Destroy(transform.gameObject);
        }
    }
}
