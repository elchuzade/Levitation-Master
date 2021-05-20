using System.Collections.Generic;
using UnityEngine;

public class GiftWindow : MonoBehaviour
{
    Player player;
    [Header("Where the logic for setting counts is")]
    [SerializeField] LevelStatus levelStatus;

    [SerializeField] GameObject freeButton;
    [SerializeField] GameObject freeButtonDisabled;
    [SerializeField] GameObject spinButton;

    [SerializeField] GameObject spinText;
    [SerializeField] GameObject winText;

    [SerializeField] GameObject allSpinners;

    // When all 4 spinners have completed instantiating items start spinning
    List<bool> readySpinners = new List<bool>();

    List<GameObject> completeSpinners = new List<GameObject>();

    void Start()
    {
        player = FindObjectOfType<Player>();
        player.LoadPlayer();
    }

    #region Public Methods
    public void OpenGiftWindow()
    {
        gameObject.SetActive(true);
        spinButton.SetActive(false);

        for (int i = 0; i < allSpinners.transform.childCount; i++)
        {
            allSpinners.transform.GetChild(i).GetComponent<Spinner>().InitializeSpinner();
        }
    }

    public List<GameObject> GetCompleteSpinners()
    {
        return completeSpinners;
    }

    public void ReadySpinner()
    {
        readySpinners.Add(true);

        if (readySpinners.Count == allSpinners.transform.childCount)
        {
            spinButton.SetActive(true);
        }
    }

    public void CompleteSpinner(GameObject spinner)
    {
        completeSpinners.Add(spinner);
        levelStatus.SetSpinnerGiftCounts(spinner);

        if (completeSpinners.Count == allSpinners.transform.childCount)
        {
            SpinnersComplete();
        }
    }

    public void ClickSpinButton()
    {
        // Save click
        System.DateTimeOffset now = System.DateTimeOffset.UtcNow;
        long date = now.ToUnixTimeMilliseconds();
        player.spinnerClicks.Add(date);
        player.SavePlayer();

        spinButton.SetActive(false);
        freeButtonDisabled.SetActive(true);
        StartSpinning();
    }

    public void CloseGiftWindow()
    {
        spinText.SetActive(true);
        winText.SetActive(false);
        spinButton.SetActive(true);
        freeButton.SetActive(false);
        freeButtonDisabled.SetActive(false);

        readySpinners.Clear();
        completeSpinners.Clear();

        for (int i = 0; i < allSpinners.transform.childCount; i++)
        {
            allSpinners.transform.GetChild(i).GetComponent<Spinner>().ResetSpinner();
        }

        gameObject.SetActive(false);
    }
    #endregion

    #region Private Methods
    void StartSpinning()
    {
        for (int i = 0; i < allSpinners.transform.childCount; i++)
        {
            allSpinners.transform.GetChild(i).GetComponent<Spinner>().StartSpinning();
        }
    }

    void SpinnersComplete()
    {
        freeButtonDisabled.SetActive(false);
        freeButton.SetActive(true);
        spinText.SetActive(false);
        winText.SetActive(true);
    }
    #endregion
}
