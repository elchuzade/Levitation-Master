using System;
using System.Collections.Generic;

[Serializable]
public class PlayerData
{
    public int coins = 0;
    public int diamonds = 0;
    public int nextLevelIndex = 1;
    public string playerName = "";
    public bool playerCreated = false;
    public bool privacyPolicy = false;
    public bool nameChanged = false;
    public List<int> allBalls = new List<int>() { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
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

    public bool maxLevelReached = false;

    public PlayerData (Player player)
    {
        coins = player.coins;
        diamonds = player.diamonds;
        nextLevelIndex = player.nextLevelIndex;
        playerName = player.playerName;
        playerCreated = player.playerCreated;
        privacyPolicy = player.privacyPolicy;
        nameChanged = player.nameChanged;
        allBalls = player.allBalls;
        currentBallIndex = player.currentBallIndex;

        redKeyCount = player.redKeyCount;
        goldKeyCount = player.goldKeyCount;
        silverKeyCount = player.silverKeyCount;

        bulletCount = player.bulletCount;
        lightningCount = player.lightningCount;
        shieldCount = player.shieldCount;
        speedCount = player.speedCount;

        redChestBuys = player.redChestBuys;
        goldChestBuys = player.goldChestBuys;
        silverChestBuys = player.silverChestBuys;

        spinnerClicks = player.spinnerClicks;
        spinnerCollects = player.spinnerCollects;
        shopClicks = player.shopClicks;
        leaderboardClicks = player.leaderboardClicks;
        chestClicks = player.chestClicks;
        maxLevelReached = player.maxLevelReached;
    }
}
