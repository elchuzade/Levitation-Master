using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Represents a player's ball
    Ball ball;

    DirectionArrow directionArrow;

    [SerializeField] GameObject components;
    [SerializeField] GameObject enemyDestroy;

    int detectRadius = 250;
    int followSpeed = 50;

    #region Unity Methods
    void Awake()
    {
        ball = FindObjectOfType<Ball>();
        directionArrow = FindObjectOfType<DirectionArrow>();
    }

    void Update()
    {
        if (ball != null)
        {
            if (Vector3.Distance(transform.position, ball.transform.position) < detectRadius)
            {
                Vector3 distanceVector = directionArrow.transform.position - transform.position;

                float angle = Mathf.Atan2(distanceVector.x, distanceVector.z) * Mathf.Rad2Deg;
                transform.localRotation = Quaternion.Euler(0, angle, 0);

                distanceVector = distanceVector.normalized;
                transform.localPosition += new Vector3(distanceVector.x, 0, distanceVector.z) * followSpeed * Time.deltaTime;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ball")
        {
            other.gameObject.GetComponent<Ball>().AttemptTrapBall();

            AttemptDestroyProcess();
        }
        else if (other.gameObject.tag == "Bullet")
        {
            Destroy(other.gameObject);
            AttemptDestroyProcess();
        }
    }
    #endregion

    #region Private Methods
    void AttemptDestroyProcess()
    {
        enemyDestroy.SetActive(true);
        components.SetActive(false);
        GetComponent<SphereCollider>().enabled = false;
        Destroy(gameObject, 1);
    }
    #endregion

    #region Public Methods
    // @access from Ball script
    public void DestroyEnemy()
    {
        AttemptDestroyProcess();
    }
    #endregion
}
