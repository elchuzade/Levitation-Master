using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using static GlobalVariables;

public class Ball : MonoBehaviour
{
    Player player;
    LevelStatus levelStatus;

    DirectionArrow directionArrow;
    [SerializeField] GameObject platformItems;

    [SerializeField] ExplosionArea explosionArea;
    [SerializeField] VariableJoystick variableJoystick;
    [SerializeField] Rigidbody rb;
    [SerializeField] GameObject[] allBalls;

    [SerializeField] GameObject platform;
    [SerializeField] GameObject ballDestroy;

    [SerializeField] GameObject lightningSkill;
    [SerializeField] GameObject bulletSkillPrefab;
    [SerializeField] GameObject shieldSkill;
    [SerializeField] GameObject shieldButton;

    [SerializeField] GameObject speedSkill;
    [SerializeField] GameObject speedButton;
    [SerializeField] GameObject winParticles;

    [SerializeField] float speed;

    float speedMultiplier = 1f;
    bool shieldStatus;
    
    bool followDirection;

    // To decide whether joystick can move the ball or not
    bool idle = true;
    // This is when the ball is already in the center of the Jumper
    bool pushingUp;
    bool pushingDown;

    GameObject ballPrefab;

    void Awake()
    {
        levelStatus = FindObjectOfType<LevelStatus>();
        directionArrow = FindObjectOfType<DirectionArrow>();
    }

    void Start()
    {
        player = FindObjectOfType<Player>();
        //player.ResetPlayer();
        player.LoadPlayer();

        ballPrefab = Instantiate(allBalls[player.currentBallIndex], transform.position, Quaternion.identity);
        ballPrefab.transform.SetParent(transform);
        ballPrefab.transform.Find("RaisingTail").gameObject.SetActive(false);

        pushingDown = true;
        rb.AddForce(Vector3.down * 10000 * rb.mass);
    }

    void FixedUpdate()
    {
        if (!idle)
        {
            Vector3 direction = Vector3.forward * variableJoystick.Vertical + Vector3.right * variableJoystick.Horizontal;
            rb.AddForce(direction * speed * speedMultiplier * Time.fixedDeltaTime, ForceMode.VelocityChange);
            rb.AddForce(Vector3.down * 100 * rb.mass);
        }

        if (pushingUp) {
            transform.position += new Vector3(0, 15, 0);
            transform.rotation = Quaternion.identity;
        } else if (pushingDown)
        {
            transform.position -= new Vector3(0, 10, 0);
        }

        if (Vector3.Distance(transform.position, platform.transform.position) < 500 && pushingDown)
        {
            levelStatus.StartLevel();
            pushingDown = false;
            rb.AddForce(Vector3.down * 20000 * rb.mass);
        }
    }

    void Update()
    {
        if (!pushingDown && !followDirection)
        {
            StartCoroutine(StartTrackingBall());
            followDirection = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "MapLimit")
        {
            StartCoroutine(GameOver(0));
        }
    }

    #region Private Methods
    void DestroyShieldSkill()
    {
        shieldStatus = false;
        shieldSkill.SetActive(false);
        shieldButton.GetComponent<Skill>().ReloadSkill();
    }

    void DestroySpeedSkill()
    {
        speedMultiplier = 1;
        speedSkill.SetActive(false);
        speedButton.GetComponent<Skill>().ReloadSkill();
    }
    #endregion

    #region Public Methods
    // @access from trap or enemy
    public void AttemptTrapBall()
    {
        if (shieldStatus)
        {
            DestroyShieldSkill();
        }
        else
        {
            ballDestroy.SetActive(true);
            ballPrefab.SetActive(false);
            directionArrow.gameObject.SetActive(false);

            StartCoroutine(GameOver(1));
        }
    }

    // @access from Jumper to stop moving when the level is complete
    public void StopMovements()
    {
        idle = true;

        // Stop the rotation so Camera does not turn around
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
    }

    // @access from Jumper
    public void PushBallUp()
    {
        rb.velocity = Vector3.zero;
        pushingUp = true;
        StartCoroutine(StopBallPushingUp());
    }

    // @access from push arrow object when ball rolls over it
    public void PushBall(Vector3 pushDirection)
    {
        rb.velocity = Vector3.zero;
        rb.AddForce(pushDirection);
    }

    // @access from LevelStatus script
    public void UseLightningSkill()
    {
        // Strike lightning effect on the ball
        lightningSkill.SetActive(true);

        List<GameObject> allEnemies = explosionArea.GetAllDetectedEnemies();
        for (int i = 0; i < allEnemies.Count; i++)
        {
            if (allEnemies[i] != null)
            {
                allEnemies[i].GetComponent<Enemy>().DestroyEnemy();
            }
        }
        explosionArea.ClearAllDetectedEnemies();

        List<GameObject> allTraps = explosionArea.GetAllDetectedTraps();
        for (int i = 0; i < allTraps.Count; i++)
        {
            if (allTraps[i] != null)
            {
                allTraps[i].GetComponent<Trap>().DestroyTrap();
            }
        }
        explosionArea.ClearAllDetectedTraps();

        List<GameObject> allBarriers = explosionArea.GetAllDetectedBarriers();
        for (int i = 0; i < allBarriers.Count; i++)
        {
            if (allBarriers[i] != null)
            {
                allBarriers[i].GetComponent<Barrier>().AttemptDestroyProcess();
            }
        }
        explosionArea.ClearAllDetectedBarriers();
        StartCoroutine(StopLightningSkill());
    }

    // @access from LevelStatus script
    public void UseShieldSkill()
    {
        shieldStatus = true;
        shieldSkill.SetActive(true);
    }

    // @access from LevelStatus script
    public void UseSpeedSkill()
    {
        speedMultiplier = 1.4f;
        speedSkill.SetActive(true);
        StartCoroutine(StopSpeedSkill());
    }

    // @access from LevelStatus script
    public void UseBulletSkill()
    {
        GameObject bulletInstance = Instantiate(bulletSkillPrefab, transform.position, Quaternion.identity);
        bulletInstance.transform.SetParent(platformItems.transform);
    }
    #endregion

    #region Coroutine
    IEnumerator StopSpeedSkill()
    {
        yield return new WaitForSeconds(10f);
        DestroySpeedSkill();
    }

    IEnumerator StopLightningSkill()
    {
        yield return new WaitForSeconds(0.5f);
        lightningSkill.SetActive(false);
    }

    IEnumerator StartTrackingBall()
    {
        yield return new WaitForSeconds(3);

        idle = false;
        directionArrow.StartTracking();
    }

    IEnumerator StopBallPushingUp()
    {
        yield return new WaitForSeconds(3);
        
        pushingUp = false;
        // Stop from falling down
        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

        winParticles.SetActive(true);
        levelStatus.SetNextLevelMeter();
        StartCoroutine(LoadNextLevel());
    }

    IEnumerator LoadNextLevel()
    {
        yield return new WaitForSeconds(1000);

        // Load next leve only in LevelStatus to keep player data consistent
        levelStatus.LoadNextLevel();
    }

    IEnumerator GameOver(float time)
    {
        Debug.Log("lost, do some particles");

        yield return new WaitForSeconds(time);

        levelStatus.LoseLevel();
    }
    #endregion
}
