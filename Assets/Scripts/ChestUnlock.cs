using UnityEngine;

public class ChestUnlock : MonoBehaviour
{
    [SerializeField] ChestWindow chestWindow;

    [SerializeField] GameObject chestClosed;
    [SerializeField] GameObject chestOpened;
    [SerializeField] GameObject lockClosed;
    [SerializeField] GameObject lockOpened;

    void Start()
    {
        StartUnlocking();
    }

    #region Public Methods
    // @access from ChestUnlockWindow canvas
    public void StartUnlocking()
    {
        lockClosed.GetComponent<AnimationTrigger>().Trigger("Start");
    }

    // @access from ChestUnlockWindow canvas at the end of animation
    public void OpenLock()
    {
        lockClosed.SetActive(false);
        lockOpened.SetActive(true);

        chestClosed.SetActive(false);
        chestOpened.SetActive(true);

        //chestsStatus.GiveChestPrize();
        chestWindow.CreatePrize();
    }
    #endregion
}