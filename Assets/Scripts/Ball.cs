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
    [SerializeField] GameObject shieldSkill;
    [SerializeField] GameObject bulletSkillPrefab;
    [SerializeField] GameObject shieldButton;

    [SerializeField] float speed;

    int lightningFactor = 1;
    bool shieldBuff;
    bool shieldSkillStatus;
    bool lightningBuff;

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
    void DestroyShieldSkill()
    {
        shieldSkillStatus = false;
        shieldSkill.SetActive(false);
        shieldButton.GetComponent<Skill>().ReloadSkill();
    }
    #endregion

    #region Public Methods
    // @access from trap or enemy
    public void AttemptTrapBall()
    {
        if (shieldBuff)
        {
            DestroyBuff(Buff.Shield);
        }
        else if (shieldSkillStatus)
        {
            DestroyShieldSkill();
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
        return lightningBuff;
    }

    // @access from levelStatus to see if there is a need to make a new one
    public bool GetShield()
    {
        return shieldBuff;
    }

    // @access from level status when the ball enters a box
    public void SetBuff(Buff buff)
    {
        switch (buff)
        {
            case Buff.Shield:
                shieldBuff = true;
                transform.Find("Buffs").Find("ShieldBuff").gameObject.SetActive(true);
                break;
            case Buff.Lightning:
                lightningBuff = true;
                lightningFactor = 2;
                transform.Find("Buffs").Find("LightningBuff").gameObject.SetActive(true);
                break;
        }
    }

    // @access from buff item in the canvas when time runs out
    // @access from ball killing item when ball touches it (eg step on a bomb - remove shieldBuff)
    public void DestroyBuff(Buff buff)
    {
        switch (buff)
        {
            case Buff.Shield:
                shieldBuff = false;
                transform.Find("Buffs").Find("ShieldBuff").gameObject.SetActive(false);
                break;
            case Buff.Lightning:
                lightningBuff = false;
                lightningFactor = 1;
                transform.Find("Buffs").Find("LightningBuff").gameObject.SetActive(false);
                break;
        }
    }

    // @access from LevelStatus script
    public void UseLightingSkill()
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
        shieldSkillStatus = true;
        shieldSkill.SetActive(true);
    }

    // @access from LevelStatus script
    public void UseBulletSkill()
    {
        GameObject bulletInstance = Instantiate(bulletSkillPrefab, transform.position, Quaternion.identity);
        bulletInstance.transform.SetParent(platformItems.transform);
    }
    #endregion

    #region Coroutine
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
        levelStatus.SetNextLevelMeter();
        StartCoroutine(LoadNextLevel());
    }

    IEnumerator LoadNextLevel()
    {
        yield return new WaitForSeconds(1000);

        // Load next leve only in LevelStatus to keep player data consistent
        levelStatus.LoadNextLevel();
    }
    #endregion
}
