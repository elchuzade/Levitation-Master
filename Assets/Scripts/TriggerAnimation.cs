using UnityEngine;

public class TriggerAnimation : MonoBehaviour
{
    Animator anim;

    void Awake()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    public void Trigger()
    {
        anim.SetTrigger("Start");
    }

    public void TriggerSpecificAnimation(string animationName)
    {
        anim.SetTrigger(animationName);
    }
}
