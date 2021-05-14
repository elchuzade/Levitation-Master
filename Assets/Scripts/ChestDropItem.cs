using UnityEngine;

public class ChestDropItem : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    float time = 0.25f;

    bool pushingDown;
    bool complete;

    float pushX;
    float pushY = 800;
    float pushZ; // always negative, so tha coin comes towards the camera

    // If last prize then wont move up when dropped. will wait for collect button click
    public bool lastPrize;

    void Start()
    {
        pushX = Random.Range(-170, 170);
        pushZ = Random.Range(-180, -50);
    }

    void Update()
    {
        if (!pushingDown)
        {
            if (time > 0)
            {
                time -= Time.deltaTime;
            }
            else
            {
                pushingDown = true;
            }
        }
    }

    void FixedUpdate()
    {
        if(complete)
        {
            rb.AddForce(-pushX, pushY * 10f, -pushZ, ForceMode.Impulse);
        } else
        {
            if (pushingDown)
            {
                rb.AddForce(pushX, -pushY, pushZ, ForceMode.Impulse);
            }
            else
            {
                rb.AddForce(pushX, pushY, pushZ, ForceMode.Impulse);
            }
        }
    }

    public void MoveUp()
    {
        if (!lastPrize)
        {
            complete = true;
            rb.constraints = RigidbodyConstraints.None;
            Destroy(gameObject, 2);
        }
    }
}
