﻿using UnityEngine;
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
    public List<int> allBalls = new List<int>() { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    public int currentBallIndex = 0;
    public int redChestCount = 0;
    public int goldChestCount = 0;
    public int silverChestCount = 0;

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
        coins = 44444;
        playerName = "";
        diamonds = 555;
        nextLevelIndex = 1;
        playerName = "";
        playerCreated = false;
        privacyPolicy = false;
        nameChanged = false;
        allBalls = new List<int>() { 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        currentBallIndex = 0;
        redChestCount = 1;
        goldChestCount = 1;
        silverChestCount = 1;

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
        redChestCount = data.redChestCount;
        goldChestCount = data.goldChestCount;
        silverChestCount = data.silverChestCount;
    }
}
