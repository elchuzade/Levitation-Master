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
    public int redChestCount = 0;
    public int goldChestCount = 0;
    public int silverChestCount = 0;

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
        redChestCount = player.redChestCount;
        goldChestCount = player.goldChestCount;
        silverChestCount = player.silverChestCount;
    }
}
