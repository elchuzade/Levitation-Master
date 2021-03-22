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
        Debug.Log("test");
        anim.SetTrigger("Start");
    }

    public void TriggerSpecificAnimation(string animationName)
    {
        Debug.Log(anim);
        anim.SetTrigger(animationName);
    }
}
