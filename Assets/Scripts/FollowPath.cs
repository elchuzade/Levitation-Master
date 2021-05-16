using UnityEngine;

public class FollowPath : MonoBehaviour
{
    [SerializeField] GameObject wayPoints;
    [SerializeField] float delay;

    int currentWaypointIndex = 0;
    float speed = 50f;

    float time;
    bool moving = false;
    bool looping = false;

    void Start()
    {
        // count down from delay time to start moving
        time = delay;
        // if first and last coordinates are the same, this chainsaw is looping
        if (wayPoints.transform.GetChild(0).position ==
            wayPoints.transform.GetChild(wayPoints.transform.childCount - 1).position)
        {
            looping = true;
        }
    }

    void Update()
    {
        if (!moving)
        {
            if (time > 0)
            {
                time -= Time.deltaTime;
            }
            else
            {
                moving = true;
            }
        } else
        {
            if (currentWaypointIndex < wayPoints.transform.childCount)
            {
                if (wayPoints.transform.GetChild(currentWaypointIndex).position != transform.position)
                {
                    transform.position = Vector3.MoveTowards(transform.position, wayPoints.transform.GetChild(currentWaypointIndex).position, speed * Time.deltaTime);
                }
                else
                {
                    currentWaypointIndex++;
                }
            } else
            {
                if (looping)
                {
                    currentWaypointIndex = 0;
                }
            }
        }
    }
}
