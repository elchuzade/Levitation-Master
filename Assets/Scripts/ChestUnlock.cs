using UnityEngine;

public class ChestUnlock : MonoBehaviour
{
    ChestsStatus chestsStatus;

    [SerializeField] GameObject chestClosed;
    [SerializeField] GameObject chestOpened;
    [SerializeField] GameObject lockClosed;
    [SerializeField] GameObject lockOpened;

    [SerializeField] GameObject chestOpenParticles;
    [SerializeField] GameObject chestOpenRays;

    void Awake()
    {
        chestsStatus = FindObjectOfType<ChestsStatus>();
    }

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

        chestOpenRays.SetActive(true);
        chestOpenParticles.SetActive(true);
        chestsStatus.GiveChestPrize();
    }
    #endregion
}
