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
    public List<int> allBalls = new List<int>() { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    public int currentBallIndex = 0;
    public int redKeyCount = 0;
    public int goldKeyCount = 0;
    public int silverKeyCount = 0;

    public int bulletCount = 0;
    public int lightningCount = 0;
    public int shieldCount = 0;
    public int speedCount = 0;

    public List<int> levelSpinClick = new List<int>();
    public List<int> homeSpinClick = new List<int>();
    public List<int> shopClick = new List<int>();
    public List<int> chestClick = new List<int>();

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

        levelSpinClick = player.levelSpinClick;
        homeSpinClick = player.homeSpinClick;
        shopClick = player.shopClick;
        chestClick = player.chestClick;
    }
}
