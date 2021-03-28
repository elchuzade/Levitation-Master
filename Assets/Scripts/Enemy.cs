using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Represents a player's ball
    Ball ball;

    [SerializeField] int detectRadius;
    [SerializeField] int followSpeed;

    void Awake()
    {
        ball = FindObjectOfType<Ball>();
    }

    void Update()
    {
        if (ball != null)
        {
            if (Vector3.Distance(transform.position, ball.transform.position) < detectRadius)
            {
                transform.LookAt(ball.transform);
                Vector3 pos = Vector3.MoveTowards(transform.position, ball.transform.position, followSpeed * Time.deltaTime);
                transform.position = pos;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ball")
        {
            other.gameObject.GetComponent<Ball>().AttemptTrapBall();
            Destroy(transform.gameObject);
        }
    }
}
