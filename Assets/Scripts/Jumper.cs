using UnityEngine;

public class Jumper : MonoBehaviour
{
    LevelStatus levelStatus;

    [SerializeField] Transform ballJumpPosition;

    GameObject ball;

    // This is to not keep adding force each many times but only do it once
    bool levelComplete;
    int jumperPullSpeed = 200;

    void Awake()
    {
        levelStatus = FindObjectOfType<LevelStatus>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (levelComplete)
        {
            // Pull the ball towards the center of the jumper and push teh ball up
            ball.transform.position = Vector3.MoveTowards(ball.transform.position, ballJumpPosition.position, jumperPullSpeed * Time.fixedDeltaTime);
            // Got to the center of the jumper, just push up and complete the level
            if (ball.transform.position == ballJumpPosition.position)
            {
                GetComponent<TriggerAnimation>().Trigger();
                levelStatus.CompleteLevel();
                levelComplete = false;
                ball.GetComponent<Ball>().PushBallUp();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ball")
        {
            levelComplete = true;
            ball = other.gameObject;
            other.GetComponent<Ball>().StopMovements();
        }
    }
}
