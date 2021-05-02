using UnityEngine;
using UnityEngine.UI;

public class Fire : MonoBehaviour
{
    GameObject ball;

    [SerializeField] GameObject fireParticles;
    [SerializeField] GameObject burnSmokeParticles;

    [SerializeField] GameObject floor;

    float burnTime = 6;
    float smolderTime = 3;

    bool burning;

    float time;

    void Start()
    {
        time = burnTime;
    }

    void Update()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;
        } else
        {
            if (burning)
            {
                time = smolderTime;
                SetSmoldering();
            } else
            {
                time = burnTime;
                SetBurning();
            }
            burning = !burning;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Ball" && burning)
        {
            ball = other.gameObject;

            ball.GetComponent<Ball>().AttemptTrapBall();

            time = smolderTime;
            SetSmoldering();
        }
    }

    #region Private Methods
    void SetSmoldering()
    {
        fireParticles.SetActive(false);
        burnSmokeParticles.SetActive(false);
        floor.GetComponent<SpriteRenderer>().color = new Color32(0, 255, 0, 100);
    }

    void SetBurning()
    {
        fireParticles.SetActive(true);
        burnSmokeParticles.SetActive(true);
        floor.GetComponent<SpriteRenderer>().color = new Color32(255, 0, 0, 255);
    }
    #endregion

    #region Public Methods
    #endregion
}
