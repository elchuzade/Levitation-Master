using UnityEngine;
using static GlobalVariables;

public class BuffItem : MonoBehaviour
{
    [SerializeField] GameObject fill;
    [SerializeField] float timer;
    float timerInit;

    [SerializeField] Buff buff;

    // To count full seconds
    int timerSeconds;
    int timerSecondsInit;
    Vector3 fillMove;
    Vector3 fillMoveInitPos;
    float itemHeight = 100;

    Ball ball;

    void Awake()
    {
        ball = FindObjectOfType<Ball>();
    }

    void Start()
    {
        timerSeconds = (int)timer;
        fillMove = new Vector3(0, itemHeight / timerSeconds, 0);
        fillMoveInitPos = fill.transform.position;
        // To reset values
        timerInit = timer;
        timerSecondsInit = timerSeconds;
    }

    void Update()
    {
        if (timer > 0)
        {
            // Check if a full second is gone
            if (Mathf.Ceil(timer) <= timerSeconds)
            {
                // Decrease full seconds and move the fill up by one step
                timerSeconds--;
                fill.transform.position += fillMove;
            }
            
            timer -= Time.deltaTime;
        } else
        {
            ball.DestroyBuff(buff);
            Destroy(gameObject);
        }
    }

    public Buff GetBuff()
    {
        return buff;
    }

    public void ResetBuff()
    {
        fill.transform.position = fillMoveInitPos;
        timer = timerInit;
    }
}
