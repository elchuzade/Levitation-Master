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

    [Header("Balls")]
    public int ballChance;
    public int ballMin;
    public int ballMax;
    public int commonChance;
    public int uncommonChance;
    public int rareChance;
    public int legendaryChance;

    [Header("Keys")]
    public int keyChance;
    public int keyMin;
    public int keyMax;
    public int silverKeyChance;
    public int goldKeyChance;
    public int redKeyChance;

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

        chestWindow.CreatePrize();
    }
    #endregion
}