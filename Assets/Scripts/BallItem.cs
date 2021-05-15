using UnityEngine;
using static GlobalVariables;

public class BallItem : MonoBehaviour
{
    [SerializeField] int ballIndex;
    [SerializeField] BallTypes ballType;

    bool movingUp;
    float time = 0.5f;

    void Update()
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

    public BallTypes GetBallType()
    {
        return ballType;
    }

    public int GetBallIndex()
    {
        return ballIndex;
    }
}
