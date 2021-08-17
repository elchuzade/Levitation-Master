using System.Collections;
using UnityEngine;

public class Pusher : MonoBehaviour
{
    [SerializeField] float pushSpeed;

    [SerializeField] GameObject pusherTip;

    Vector3 directionVector;

    void Start()
    {
        //pushSpeed *= 100;

        // Vector3 between tip and center of the pusher
        directionVector = (pusherTip.transform.position - transform.position).normalized * pushSpeed;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ball")
        {
            StartCoroutine(PushBallCoroutine(other.gameObject));
        }
    }

    IEnumerator PushBallCoroutine(GameObject ball)
    {
        yield return new WaitForSeconds(0.5f);

        ball.GetComponent<Ball>().PushBall(directionVector);
    }
}
