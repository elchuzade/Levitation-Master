using UnityEngine;
using static GlobalVariables;

public class BallItem : MonoBehaviour
{
    [SerializeField] int ballIndex;
    [SerializeField] BallTypes ballType;
    [SerializeField] int powerUp;
    [SerializeField] int ballSpeed;


    bool movingUp;
    float time = 0.3f;

    void FixedUpdate()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;
            if (movingUp)
            {
                transform.position += new Vector3(0, 4, 0);
            }
        } else
        {
            movingUp = false;
        }
    }

    public void MoveUp()
    {
        movingUp = true;
    }

    public int GetPowerUp()
    {
        return powerUp;
    }

    public int GetBallSpeed()
    {
        return ballSpeed;
    }

    public BallTypes GetBallType()
    {
        return ballType;
    }

    public int GetBallIndex()
    {
        return ballIndex;
    }
}
