using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using static GlobalVariables;

public class Ball : MonoBehaviour
{
    Player player;
    LevelStatus levelStatus;

    DirectionArrow directionArrow;
    [SerializeField] ExplosionArea explosionArea;
    [SerializeField] VariableJoystick variableJoystick;
    [SerializeField] Rigidbody rb;
    [SerializeField] GameObject[] allBalls;

    [SerializeField] GameObject platform;
    [SerializeField] GameObject ballDestroy;

    [SerializeField] float speed;

    int lightningFactor = 1;
    bool shield;
    bool lightning;

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

        pushingDown = true;
        rb.AddForce(Vector3.down * 10000 * rb.mass);
    }

    void FixedUpdate()
    {
        if (!idle)
        {
            Vector3 direction = Vector3.forward * variableJoystick.Vertical + Vector3.right * variableJoystick.Horizontal;
            rb.AddForce(direction * speed * lightningFactor * Time.fixedDeltaTime, ForceMode.VelocityChange);
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

    #region Private Methods
    #endregion

    #region Public Methods
    // @access from trap or enemy
    public void AttemptTrapBall()
    {
        if (shield)
        {
            DestroyBuff(Buff.Shield);
        }
        else
        {
            ballDestroy.SetActive(true);
            ballPrefab.SetActive(false);
            directionArrow.gameObject.SetActive(false);

            Destroy(gameObject, 1);
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

    // @access from levelStatus to see if there is a need to make a new one
    public bool GetLightning()
    {
        return lightning;
    }

    // @access from levelStatus to see if there is a need to make a new one
    public bool GetShield()
    {
        return shield;
    }

    // @access from level status when the ball enters a box
    public void SetBuff(Buff buff)
    {
        switch (buff)
        {
            case Buff.Shield:
                shield = true;
                transform.Find("Buffs").Find("ShieldBuff").gameObject.SetActive(true);
                break;
            case Buff.Lightning:
                lightning = true;
                lightningFactor = 2;
                transform.Find("Buffs").Find("LightningBuff").gameObject.SetActive(true);
                break;
        }
    }

    // @access from buff item in the canvas when time runs out
    // @access from ball killing item when ball touches it (eg step on a bomb - remove shield)
    public void DestroyBuff(Buff buff)
    {
        switch (buff)
        {
            case Buff.Shield:
                shield = false;
                transform.Find("Buffs").Find("ShieldBuff").gameObject.SetActive(false);
                break;
            case Buff.Lightning:
                lightning = false;
                lightningFactor = 1;
                transform.Find("Buffs").Find("LightningBuff").gameObject.SetActive(false);
                break;
        }
    }

    // @access from LevelStatus script
    public void StrikeLighting()
    {
        List<GameObject> allEnemies = explosionArea.GetAllDetectedEnemies();
        for (int i = 0; i < allEnemies.Count; i++)
        {
            allEnemies[i].GetComponent<Enemy>().DestroyEnemy();
        }
        explosionArea.ClearAllDetectedEnemies();

        List<GameObject> allTraps = explosionArea.GetAllDetectedTraps();
        for (int i = 0; i < allTraps.Count; i++)
        {
            allTraps[i].GetComponent<Trap>().DestroyTrap();
        }
        explosionArea.ClearAllDetectedTraps();

        List<GameObject> allBarriers = explosionArea.GetAllDetectedBarriers();
        for (int i = 0; i < allBarriers.Count; i++)
        {
            allBarriers[i].GetComponent<Barrier>().AttemptDestroyProcess();
        }
        explosionArea.ClearAllDetectedBarriers();
    }
    #endregion

    #region Coroutine
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
        levelStatus.SetNextLevelMeter();
        StartCoroutine(LoadNextLevel());
    }

    IEnumerator LoadNextLevel()
    {
        yield return new WaitForSeconds(2);

        // Load next leve only in LevelStatus to keep player data consistent
        levelStatus.LoadNextLevel();
    }
    #endregion
}
