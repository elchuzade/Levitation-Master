using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Represents a player's ball
    Ball ball;

    DirectionArrow directionArrow;

    [SerializeField] GameObject components;
    [SerializeField] GameObject enemyDestroy;

    [SerializeField] GameObject leftBladeIdle;
    [SerializeField] GameObject rightBladeIdle;

    [SerializeField] GameObject leftBladeHunt;
    [SerializeField] GameObject rightBladeHunt;

    [SerializeField] GameObject idleParticles;
    [SerializeField] GameObject huntParticles;

    [SerializeField] Material EnemyBodyMaterial;
    [SerializeField] Material EnemyBladeMaterial;
    [SerializeField] Material EnemyBodyMaterialTransparent;
    [SerializeField] Material EnemyBladeMaterialTransparent;
    [SerializeField] GameObject[] Bodies;
    [SerializeField] GameObject[] Blades;



    int detectRadius = 250;
    int followSpeed = 50;

    bool following;

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
                FollowPlayer();
            } else
            {
                if (following)
                {
                    StopFollowingPlayer();
                }
            }
        }
    }

    void SetVisibleMaterialToEnemy()
    {
        for (int i = 0; i < Bodies.Length; i++)
        {
            Bodies[i].GetComponent<MeshRenderer>().material = EnemyBodyMaterial;
        }

        for (int i = 0; i < Blades.Length; i++)
        {
            Blades[i].GetComponent<MeshRenderer>().material = EnemyBladeMaterial;
        }
    }

    void SetTransparentMaterialToEnemy()
    {
        for (int i = 0; i < Bodies.Length; i++)
        {
            Bodies[i].GetComponent<MeshRenderer>().material = EnemyBodyMaterialTransparent;
        }

        for (int i = 0; i < Blades.Length; i++)
        {
            Blades[i].GetComponent<MeshRenderer>().material = EnemyBladeMaterialTransparent;
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
        else if (other.gameObject.tag == "Wall")
        {
            SetTransparentMaterialToEnemy();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Wall")
        {
            SetVisibleMaterialToEnemy();
        }
    }

    #endregion

    #region Private Methods
    void FollowPlayer()
    {
        Vector3 distanceVector = directionArrow.transform.position - transform.position;

        float angle = Mathf.Atan2(distanceVector.x, distanceVector.z) * Mathf.Rad2Deg;
        transform.localRotation = Quaternion.Euler(0, angle, 0);

        distanceVector = distanceVector.normalized;
        transform.localPosition += new Vector3(distanceVector.x, 0, distanceVector.z) * followSpeed * Time.deltaTime;

        if (!following)
        {
            idleParticles.SetActive(false);
            huntParticles.SetActive(true);

            leftBladeIdle.SetActive(false);
            rightBladeIdle.SetActive(false);
            leftBladeHunt.SetActive(true);
            rightBladeHunt.SetActive(true);
        }
        following = true;
    }

    void StopFollowingPlayer()
    {
        if (following)
        {
            idleParticles.SetActive(true);
            huntParticles.SetActive(false);

            leftBladeIdle.SetActive(true);
            rightBladeIdle.SetActive(true);
            leftBladeHunt.SetActive(false);
            rightBladeHunt.SetActive(false);
        }
        following = false;
    }

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
