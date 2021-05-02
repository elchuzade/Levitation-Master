using UnityEngine;

public class ChestRays : MonoBehaviour
{
    [SerializeField] GameObject rayOne;
    [SerializeField] GameObject rayTwo;
    [SerializeField] GameObject raysCore;

    void Start()
    {
        rayOne.GetComponent<AnimationTrigger>().Trigger("RayOne");
        rayTwo.GetComponent<AnimationTrigger>().Trigger("RayTwo");
        raysCore.GetComponent<AnimationTrigger>().Trigger("RaysCore");
    }
}
