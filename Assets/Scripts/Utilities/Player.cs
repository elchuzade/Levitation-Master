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
        redKeyCount = 1;
        goldKeyCount = 1;
        silverKeyCount = 1;

        bulletCount = 4;
        lightningCount = 2;
        shieldCount = 2;
        speedCount = 2;

        levelSpinClick = new List<int>();
        homeSpinClick = new List<int>();
        shopClick = new List<int>();
        chestClick = new List<int>();

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

        levelSpinClick = data.levelSpinClick;
        homeSpinClick = data.homeSpinClick;
        shopClick = data.shopClick;
        chestClick = data.chestClick;
    }
}
