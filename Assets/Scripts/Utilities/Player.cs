using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public int coins = 0;
    public int diamonds = 0;
    public int nextLevelIndex = 1;
    public string playerName = "";
    public bool nameChanged = false;
    public bool playerCreated = false;
    public bool privacyPolicy = false;
    public List<int> allBalls = new List<int>() { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    public int currentBallIndex = 0;

    public int redKeyCount = 0;
    public int goldKeyCount = 0;
    public int silverKeyCount = 0;

    public int bulletCount = 0;
    public int lightningCount = 0;
    public int shieldCount = 0;
    public int speedCount = 0;

    public List<long> redChestBuys = new List<long>();
    public List<long> goldChestBuys = new List<long>();
    public List<long> silverChestBuys = new List<long>();

    public List<long> spinnerClicks = new List<long>();
    public List<long> spinnerCollects = new List<long>();
    public List<long> shopClicks = new List<long>();
    public List<long> leaderboardClicks = new List<long>();
    public List<long> chestClicks = new List<long>();

    void Awake()
    {
        transform.SetParent(transform.parent.parent);
        // Singleton
        int instances = FindObjectsOfType<Player>().Length;
        if (instances > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
    }

    public void ResetPlayer()
    {
        coins = 0;
        playerName = "";
        diamonds = 400;
        nextLevelIndex = 1;
        playerName = "";
        playerCreated = false;
        privacyPolicy = false;
        nameChanged = false;
        allBalls = new List<int>() { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        currentBallIndex = 0;

        redKeyCount = 2;
        goldKeyCount = 2;
        silverKeyCount = 2;

        bulletCount = 0;
        lightningCount = 0;
        shieldCount = 0;
        speedCount = 0;

        redChestBuys = new List<long>();
        goldChestBuys = new List<long>();
        silverChestBuys = new List<long>();

        spinnerClicks = new List<long>();
        spinnerCollects = new List<long>();
        shopClicks = new List<long>();
        leaderboardClicks = new List<long>();
        chestClicks = new List<long>();

        SaveSystem.SavePlayer(this);
    }

    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        if (data == null)
        {
            ResetPlayer();
            data = SaveSystem.LoadPlayer();
        }

        playerCreated = data.playerCreated;
        privacyPolicy = data.privacyPolicy;
        playerName = data.playerName;
        coins = data.coins;
        diamonds = data.diamonds;
        nameChanged = data.nameChanged;
        nextLevelIndex = data.nextLevelIndex;
        allBalls = data.allBalls;
        currentBallIndex = data.currentBallIndex;
        redKeyCount = data.redKeyCount;
        goldKeyCount = data.goldKeyCount;
        silverKeyCount = data.silverKeyCount;

        bulletCount = data.bulletCount;
        lightningCount = data.lightningCount;
        shieldCount = data.shieldCount;
        speedCount = data.speedCount;

        redChestBuys = data.redChestBuys;
        goldChestBuys = data.goldChestBuys;
        silverChestBuys = data.silverChestBuys;

        spinnerClicks = data.spinnerClicks;
        spinnerCollects = data.spinnerCollects;
        shopClicks = data.shopClicks;
        leaderboardClicks = data.leaderboardClicks;
        chestClicks = data.chestClicks;
    }
}
