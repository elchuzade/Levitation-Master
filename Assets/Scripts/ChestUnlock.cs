using UnityEngine;

public class ChestUnlock : MonoBehaviour
{
    [SerializeField] ChestWindow chestWindow;

    [SerializeField] GameObject chestClosed;
    [SerializeField] GameObject chestOpened;
    [SerializeField] GameObject lockClosed;
    [SerializeField] GameObject lockOpened;

    public int coinMin;
    public int coinMax;
    public int diamondMin;
    public int diamondMax;
    public int skillMin;
    public int skillMax;

    [Header("Legendary")]
    public int ballMin;
    public int ballMax;
    public int commonChance;
    public int rareChance;
    public int legendaryChance;

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