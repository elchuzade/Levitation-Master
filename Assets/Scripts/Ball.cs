using UnityEngine;
using static GlobalVariables;

public class Ball : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] VariableJoystick variableJoystick;
    [SerializeField] Rigidbody rb;

    int lightningFactor = 1;
    bool shield;
    bool lightning;

    // To decide whether joystick can move the ball or not
    bool idle = true;
    // This is when the ball is already in the center of the Jumper
    bool pushingUp;

    void Start()
    {
        idle = false;
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
        }
    }

    // @access from trap or enemy
    public void AttemptTrapBall()
    {
        if (shield)
        {
            DestroyBuff(Buff.Shield);
        } else
        {
            Destroy(gameObject);
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
        //rb.AddForce(Vector3.up * 100000 * rb.mass);
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
}
