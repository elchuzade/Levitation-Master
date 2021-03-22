using UnityEngine;

public class ChestUnlock : MonoBehaviour
{
    [SerializeField] GameObject chestClosed;
    [SerializeField] GameObject chestOpened;
    [SerializeField] GameObject lockClosed;
    [SerializeField] GameObject lockOpened;

    void Start()
    {
        //StartUnlocking();
    }

    public void StartUnlocking()
    {
        lockClosed.GetComponent<TriggerAnimation>().Trigger();
    }

    public void OpenLock()
    {
        lockClosed.SetActive(false);
        lockOpened.SetActive(true);

        chestClosed.SetActive(false);
        chestOpened.SetActive(true);
    }
}
