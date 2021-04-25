using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{
    Animator anim;

    public void Trigger(string animationName)
    {
        anim = gameObject.GetComponent<Animator>();
        anim.SetTrigger(animationName);
    }
}