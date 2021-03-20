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
    public int purpleChestCount = 0;
    public int blueChestCount = 0;
    // Upgrade logic
    public int nextUpgradePrice = 110; // What you will have next
    public int nextUpgradePower = 12; // What you will have next
    public int upgradeStepPriceMin = 10; // How much price will increase at least
    public int upgradeStepPriceMax = 50; // How much price will increase at most
    public int upgradeStepPowerMin = 2; // How much power will increase at least
    public int upgradeStepPowerMax = 5; // How much power will increase at most

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
        purpleChestCount = player.purpleChestCount;
        blueChestCount = player.blueChestCount;
    }
}
