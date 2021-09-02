using UnityEngine;
using UnityEngine.UI;
using static GlobalVariables;

public class Trap : MonoBehaviour
{
    GameObject ball;

    [SerializeField] TrapTypes trapType;

    [SerializeField] GameObject explosionParticles;
    [SerializeField] SphereCollider trapCollider;
    [SerializeField] GameObject components;

    [SerializeField] GameObject trapDestroy;

    // For dynamite only
    [Header("Only For Dynamite")]
    [SerializeField] Text timer;
    [SerializeField] GameObject radius;
    [SerializeField] ExplosionArea explosionArea;
    float dynamiteExplosionRadius = 200;

    [Header("Only For Chainsaw")]
    [SerializeField] GameObject wind;

    float time = 3;
    bool timerStarted;

    bool threeSeconds;
    bool twoSeconds;
    bool oneSecond;

    bool destroyAttempted;

    void Start()
    {
        ball = FindObjectOfType<Ball>().gameObject;    
    }

    void Update()
    {
        if (trapType == TrapTypes.Dynamite && timerStarted)
        {
            if (time > 0)
            {
                if (time < 1 && !oneSecond)
                {
                    timer.gameObject.GetComponent<AnimationTrigger>().Trigger("Start");
                    timer.text = "1";
                    oneSecond = true;
                } else if (time < 2 && !twoSeconds)
                {
                    timer.gameObject.GetComponent<AnimationTrigger>().Trigger("Start");
                    timer.text = "2";
                    twoSeconds = true;
                } else if (time < 3 && !threeSeconds)
                {
                    timer.gameObject.GetComponent<AnimationTrigger>().Trigger("Start");
                    timer.text = "3";
                    threeSeconds = true;
                }
                time -= Time.deltaTime;
            } else
            {
                if (!destroyAttempted)
                {
                    AttemptDestroyProcess();
                    destroyAttempted = true;
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ball")
        {
            if (trapType == TrapTypes.Dynamite)
            {
                StartTimer();
            }
            else
            {
                if (Vector3.Distance(ball.transform.position, transform.position) < 100)
                {
                    ball.GetComponent<Ball>().AttemptTrapBall();
                }
                
                if(trapType == TrapTypes.Bomb)
                {
                    AttemptDestroyProcess();
                }
            }
        }
        else if (other.gameObject.tag == "Bullet")
        {
            Destroy(other.gameObject);
            AttemptDestroyProcess();
        }
    }

    #region Private Methods
    // For dynamite only
    void StartTimer()
    {
        radius.SetActive(true);
        timerStarted = true;
        timer.gameObject.SetActive(true);
    }

    void AttemptDestroyProcess()
    {
        components.SetActive(false);
        trapCollider.enabled = false;
        explosionParticles.SetActive(true);
        trapDestroy.SetActive(true);

        if (trapType == TrapTypes.Dynamite)
        {
            if (Vector3.Distance(ball.transform.position, transform.position) < dynamiteExplosionRadius)
            {
                ball.GetComponent<Ball>().AttemptTrapBall();
            }
            radius.SetActive(false);
            timer.gameObject.SetActive(false);
        } else if (trapType == TrapTypes.Chainsaw)
        {
            wind.SetActive(false);
        }

        GetComponent<SphereCollider>().enabled = false;

        Destroy(gameObject, 1);
    }
    #endregion

    #region Public Methods
    // @access from Ball script
    public void DestroyTrap()
    {
        AttemptDestroyProcess();
    }
    #endregion
}
