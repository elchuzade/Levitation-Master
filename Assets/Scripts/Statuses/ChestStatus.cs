using UnityEngine;
using UnityEngine.UI;
using static GlobalVariables;

public class ChestStatus : MonoBehaviour
{
    Player player;
    Navigator navigator;

    [SerializeField] GameObject chestOpenWindow;

    [SerializeField] Text diamondsText;
    [SerializeField] Text coinsText;

    [SerializeField] int redChestPrice;
    [SerializeField] int goldChestPrice;

    [SerializeField] GameObject redChest;
    [SerializeField] GameObject goldChest;
    [SerializeField] GameObject silverChest;

    // Green is just to ignore all actual colors. Like default
    ChestColors selectedChestColor = ChestColors.None;

    void Awake()
    {
        navigator = FindObjectOfType<Navigator>();
    }

    void Start()
    {
        player = FindObjectOfType<Player>();

        player.ResetPlayer();
        //return;

        player.LoadPlayer();

        SetPlayerChests();
        SetScoreboardValues();
        SetChestPrices();

        // Make autoselect always in blue chest
        SelectSilverChest();

        // Save click
        System.DateTimeOffset now = System.DateTimeOffset.UtcNow;
        long date = now.ToUnixTimeMilliseconds();
        player.chestClicks.Add(date);
        player.SavePlayer();
    }

    #region Private Methods
    void SetChestPrices()
    {
        redChest.GetComponent<ChestItem>().SetPrice(redChestPrice);
        goldChest.GetComponent<ChestItem>().SetPrice(goldChestPrice);
    }

    void SetPlayerChests()
    {
        redChest.GetComponent<ChestItem>().SetCount(player.redKeyCount);
        goldChest.GetComponent<ChestItem>().SetCount(player.goldKeyCount);
        silverChest.GetComponent<ChestItem>().SetCount(player.silverKeyCount);
    }

    void ResetAllButtons()
    {
        redChest.transform.Find("OpenButton").gameObject.SetActive(false);
        goldChest.transform.Find("OpenButton").gameObject.SetActive(false);
        silverChest.transform.Find("OpenButton").gameObject.SetActive(false);
    }

    void DeselectChest(ChestColors chestColor)
    {
        switch (chestColor)
        {
            case ChestColors.Red:
                redChest.GetComponent<ChestItem>().DeselectChest();
                break;
            case ChestColors.Gold:
                goldChest.GetComponent<ChestItem>().DeselectChest();
                break;
            case ChestColors.Silver:
                silverChest.GetComponent<ChestItem>().DeselectChest();
                break;
        }
    }
    #endregion

    #region Public Methods
    // @access from ChestWindow script
    public void SetScoreboardValues()
    {
        diamondsText.text = player.diamonds.ToString();
        coinsText.text = player.coins.ToString();
    }

    // @access from Chest canvas
    public void SelectSilverChest()
    {
        // Do not deselect the same chest if clicked multiple times
        if (selectedChestColor != ChestColors.Silver)
        {
            DeselectChest(selectedChestColor);
            silverChest.transform.Find("ChestSelect").GetComponent<AnimationTrigger>().Trigger("Select");
            silverChest.GetComponent<AnimationTrigger>().Trigger("Start");
            selectedChestColor = ChestColors.Silver;
        }

        ResetAllButtons();
        if (player.silverKeyCount > 0)
        {
            silverChest.transform.Find("OpenButton").gameObject.SetActive(true);
        }
        else
        {
            silverChest.transform.Find("OpenButton").gameObject.SetActive(false);
        }
    }

    // @access from Chest canvas
    public void SelectGoldChest()
    {
        // Do not deselect the same chest if clicked multiple times
        if (selectedChestColor != ChestColors.Gold)
        {
            DeselectChest(selectedChestColor);
            goldChest.transform.Find("ChestSelect").GetComponent<AnimationTrigger>().Trigger("Select");
            goldChest.GetComponent<AnimationTrigger>().Trigger("Start");
            selectedChestColor = ChestColors.Gold;
        }

        ResetAllButtons();
        if (player.goldKeyCount > 0)
        {
            goldChest.transform.Find("OpenButton").gameObject.SetActive(true);
        }
        else
        {
            goldChest.transform.Find("OpenButton").gameObject.SetActive(false);
        }
    }

    // @access from Chest canvas
    public void SelectRedChest()
    {
        // Do not deselect the same chest if clicked multiple times
        if (selectedChestColor != ChestColors.Red)
        {
            DeselectChest(selectedChestColor);
            // Check if you can open the chest, or buy the chest
            redChest.transform.Find("ChestSelect").GetComponent<AnimationTrigger>().Trigger("Select");
            redChest.GetComponent<AnimationTrigger>().Trigger("Start");
            selectedChestColor = ChestColors.Red;
        }

        ResetAllButtons();
        if (player.redKeyCount > 0)
        {
            redChest.transform.Find("OpenButton").gameObject.SetActive(true);
        }
        else
        {
            redChest.transform.Find("OpenButton").gameObject.SetActive(false);
        }
    }

    // @access from Chest canvas
    public void ClickWatchButton()
    {
        Debug.Log("watching ad, then running these functions");
        // Save click
        System.DateTimeOffset now = System.DateTimeOffset.UtcNow;
        long date = now.ToUnixTimeMilliseconds();
        player.silverChestBuys.Add(date);

        player.silverKeyCount++;
        player.SavePlayer();
        SetPlayerChests();
        SelectSilverChest();
    }

    // @access from Chest canvas
    public void ClickBuyButton()
    {
        System.DateTimeOffset now = System.DateTimeOffset.UtcNow;
        long date = now.ToUnixTimeMilliseconds();

        switch (selectedChestColor)
        {
            case ChestColors.Red:
                if (player.diamonds >= redChestPrice)
                {
                    // Save click
                    player.redChestBuys.Add(date);

                    player.diamonds -= redChestPrice;
                    player.redKeyCount++;
                    SelectRedChest();
                }
                break;
            case ChestColors.Gold:
                if (player.diamonds >= goldChestPrice)
                {
                    // Save click
                    player.goldChestBuys.Add(date);

                    player.diamonds -= goldChestPrice;
                    player.goldKeyCount++;
                    SelectGoldChest();
                }
                break;
        }
        player.SavePlayer();
        SetPlayerChests();
        SetScoreboardValues();
    }

    // @access from Chest canvas
    public void ClickOpenButton()
    {
        switch (selectedChestColor)
        {
            case ChestColors.Red:
                player.redKeyCount--;
                SelectRedChest();
                break;
            case ChestColors.Gold:
                player.goldKeyCount--;
                SelectGoldChest();
                break;
            case ChestColors.Silver:
                player.silverKeyCount--;
                SelectSilverChest();
                break;
        }

        chestOpenWindow.SetActive(true);
        // Show only selected chest instead of all three chests
        chestOpenWindow.GetComponent<ChestWindow>().OpenChest(selectedChestColor);
        player.SavePlayer();
        SetPlayerChests();
    }

    // @access from Chest canvas
    public void ClickBackButton()
    {
        navigator.LoadMainScene();
    }
    #endregion
}
