using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GlobalVariables;

public class LevelStatus : MonoBehaviour
{
    Player player;
    Navigator navigator;

    [SerializeField] Text levelText;

    [SerializeField] GameObject coinPrefab;
    [SerializeField] GameObject diamondPrefab;

    [SerializeField] GameObject allKeys;
    // To set parent of coins and diamonds
    [SerializeField] GameObject platformItems;

    Ball ball;

    [SerializeField] Text coinCount;
    [SerializeField] Text subCoinCount;
    [SerializeField] Text diamondCount;
    [SerializeField] Text subDiamondCount;

    [SerializeField] GameObject coinsIcon;
    [SerializeField] GameObject diamondsIcon;

    [SerializeField] GameObject redKeyItem;
    [SerializeField] GameObject goldKeyItem;
    [SerializeField] GameObject silverKeyItem;

    [SerializeField] GameObject lightningButton;
    [SerializeField] GameObject shieldButton;
    [SerializeField] GameObject bulletButton;
    [SerializeField] GameObject speedButton;

    [SerializeField] GameObject doubleRewardWindow;
    [SerializeField] GameObject doubleRewardButton;
    [SerializeField] GameObject levelControlsWindow;

    [SerializeField] float lightningReloadTime;
    [SerializeField] float bulletReloadTime;
    [SerializeField] float shieldReloadTime;
    [SerializeField] float speedReloadTime;

    int coins;
    int diamonds;
    int redKeys;
    int silverKeys;
    int goldKeys;

    [SerializeField] GameObject giftWindow;
    [SerializeField] GameObject giftButton;
    [SerializeField] Image giftLoader;
    bool giftRelaoding;
    int giftReloadTime = 5;
    float time;
    
    void Awake()
    {
        ball = FindObjectOfType<Ball>();
        navigator = FindObjectOfType<Navigator>();
    }

    void Start()
    {
        player = FindObjectOfType<Player>();
        //player.ResetPlayer();
        player.LoadPlayer();

        SetScoreboardValues();
        levelText.text = player.nextLevelIndex.ToString();

        DisableGiftButton();
        ReloadGiftButton();
    }

    void Update()
    {
        if (giftRelaoding)
        {
            if (time > 0)
            {
                time -= Time.deltaTime;
                giftLoader.fillAmount = time / giftReloadTime;
            }
            else
            {
                EnableGiftButton();
            }
        }
    }

    #region Private Methods
    void EnableGiftButton()
    {
        giftButton.GetComponent<Button>().interactable = true;
        giftButton.transform.Find("notification").gameObject.SetActive(true);
        giftButton.GetComponent<Animator>().enabled = true;

        giftRelaoding = false;
    }

    void DisableGiftButton()
    {
        giftButton.GetComponent<Button>().interactable = false;
        giftButton.transform.Find("notification").gameObject.SetActive(false);
        giftButton.GetComponent<Animator>().enabled = false;
    }

    void ReloadGiftButton()
    {
        giftRelaoding = true;
        time = giftReloadTime;
    }

    void SetScoreboardValues()
    {
        coinCount.text = player.coins.ToString();
        diamondCount.text = player.diamonds.ToString();

        bulletButton.GetComponent<Skill>().SetSkillCount(player.bulletCount);
        lightningButton.GetComponent<Skill>().SetSkillCount(player.lightningCount);
        shieldButton.GetComponent<Skill>().SetSkillCount(player.shieldCount);
        speedButton.GetComponent<Skill>().SetSkillCount(player.speedCount);

        //shieldCount.text = player.shieldCount.ToString();

        if (coins > 0)
        {
            subCoinCount.text = "+" + coins.ToString();
        } else
        {
            subCoinCount.text = "";
        }
        if (diamonds > 0)
        {
            subDiamondCount.text = "+" + diamonds.ToString();
        } else
        {
            subDiamondCount.text = "";
        }
    }
    #endregion

    #region Public Methods
    // @access from gift window
    public void SetSpinnerGiftCounts(GameObject spinner)
    {
        switch (spinner.GetComponent<Spinner>().GetGiftName())
        {
            case "Bullet":
                int randomBulletsAmount = Random.Range(1, 3);
                spinner.GetComponent<Spinner>().SetGiftCount(randomBulletsAmount);
                break;
            case "Shield":
                int randomShieldsAmount = Random.Range(1, 2);
                spinner.GetComponent<Spinner>().SetGiftCount(randomShieldsAmount);
                break;
            case "Speed":
                int randomSpeedAmount = Random.Range(1, 2);
                spinner.GetComponent<Spinner>().SetGiftCount(randomSpeedAmount);
                break;
            case "Lightning":
                int randomLightningsAmount = Random.Range(1, 2);
                spinner.GetComponent<Spinner>().SetGiftCount(randomLightningsAmount);
                break;
            case "SilverKey":
                int randomSilverKeysAmount = Random.Range(1, 2);
                spinner.GetComponent<Spinner>().SetGiftCount(randomSilverKeysAmount);
                break;
            case "GoldKey":
                int randomGoldKeysAmount = Random.Range(1, 1);
                spinner.GetComponent<Spinner>().SetGiftCount(randomGoldKeysAmount);
                break;
            case "RedKey":
                int randomRedKeysAmount = Random.Range(1, 1);
                spinner.GetComponent<Spinner>().SetGiftCount(randomRedKeysAmount);
                break;
            case "Diamond":
                int randomDiamondsAmount = Random.Range(1, 5);
                spinner.GetComponent<Spinner>().SetGiftCount(randomDiamondsAmount);
                break;
            case "Coin":
                int randomCoinsAmount = Random.Range(1, 20);
                spinner.GetComponent<Spinner>().SetGiftCount(randomCoinsAmount);
                break;
        }
    }

    // @access from Ball script
    public void SetNextLevelMeter()
    {
        levelText.text = (player.nextLevelIndex + 1).ToString();
        levelControlsWindow.SetActive(false);
        doubleRewardWindow.SetActive(true);
        // Add reward for passing level
        coins += Random.Range(5, 15);
        SetScoreboardValues();
    }

    // @access from Collectable script
    public void CollectReward(Rewards reward)
    {
        switch (reward)
        {
            case Rewards.Coin:
                coins++;
                coinsIcon.GetComponent<AnimationTrigger>().Trigger("Start");
                break;
            case Rewards.Diamond:
                diamonds++;
                diamondsIcon.GetComponent<AnimationTrigger>().Trigger("Start");
                break;
            case Rewards.RedKey:
                redKeys++;
                GameObject redKey = Instantiate(redKeyItem, transform.position, Quaternion.identity);
                redKey.transform.SetParent(allKeys.transform);
                break;
            case Rewards.GoldKey:
                goldKeys++;
                GameObject goldKey = Instantiate(goldKeyItem, transform.position, Quaternion.identity);
                goldKey.transform.SetParent(allKeys.transform);
                break;
            case Rewards.SilverKey:
                silverKeys++;
                GameObject silverKey = Instantiate(silverKeyItem, transform.position, Quaternion.identity);
                silverKey.transform.SetParent(allKeys.transform);
                break;
        }
        SetScoreboardValues();
    }

    // @access from Box script
    public void OpenBox(Boxes box, int amount, Vector3 position)
    {
        switch (box)
        {
            case Boxes.Shield:
                player.shieldCount++;
                player.SavePlayer();
                break;
            case Boxes.Lightning:
                player.lightningCount++;
                player.SavePlayer();
                break;
            case Boxes.Bullet:
                player.bulletCount++;
                player.SavePlayer();
                break;
            case Boxes.Speed:
                player.speedCount++;
                player.SavePlayer();
                break;
            case Boxes.Coin:
                DropCoins(amount, position);
                break;
            case Boxes.Diamond:
                DropDiamonds(amount, position);
                break;
            case Boxes.Question:
                OpenBox((Boxes)Random.Range(0, 5), amount, position);
                break;
        }
        SetScoreboardValues();
    }

    // @access from box when it is hit by the ball
    public void DropCoins(int amount, Vector3 position)
    {
        for (int i = 0; i < amount; i++)
        {
            Vector3 coordShift = new Vector3(Random.Range(-30, 30), 0, Random.Range(-30, 30));
            GameObject coinInstance = Instantiate(coinPrefab, position, Quaternion.identity);
            coinInstance.transform.SetParent(platformItems.transform);
            coinInstance.transform.GetComponent<Collectible>().SetDropPosition(position + coordShift);
        }
    }

    // @access from box when it is hit by the ball
    public void DropDiamonds(int amount, Vector3 position)
    {
        for (int i = 0; i < amount; i++)
        {
            Vector3 coordShift = new Vector3(Random.Range(-30, 30), 0, Random.Range(-30, 30));
            GameObject diamondInstance = Instantiate(diamondPrefab, position, Quaternion.identity);
            diamondInstance.transform.SetParent(platformItems.transform);
            diamondInstance.transform.GetComponent<Collectible>().SetDropPosition(position + coordShift);
        }
    }

    public void CollectDroppableItem(Rewards reward)
    {

    }

    // @access from Jumper script
    public void CompleteLevel()
    {
        Camera.main.GetComponent<CameraResizer>().CameraFollowBall();
    }

    // @access from Ball script
    public void StartLevel()
    {
        Camera.main.GetComponent<CameraResizer>().CameraUnfollowBall();
    }

    // @access from Level canvas
    public void ClickHomeButton()
    {
        navigator.LoadMainScene();
    }

    // @access from Ball script
    // Only one place to save the level and load the next level to keep player data consistent
    public void LoadNextLevel()
    {
        navigator.LoadNextLevel(player.nextLevelIndex);
    }

    // @access from Level canvas
    public void ClickLightningSkill()
    {
        if (player.lightningCount > 0)
        {
            player.lightningCount--;
            player.SavePlayer();
            SetScoreboardValues();

            lightningButton.GetComponent<Skill>().ClickSkill(lightningReloadTime);

            ball.UseLightningSkill();
        }
    }

    // @access from Level canvas
    public void ClickBulletSkill()
    {
        if (player.bulletCount > 0)
        {
            player.bulletCount--;
            player.SavePlayer();
            SetScoreboardValues();

            ball.UseBulletSkill();
            bulletButton.GetComponent<Skill>().ClickSkill(bulletReloadTime);
        }
    }

    // @access from Level canvas
    public void ClickShieldSkill()
    {
        if (player.shieldCount > 0)
        {
            player.shieldCount--;
            player.SavePlayer();
            SetScoreboardValues();

            ball.UseShieldSkill();
            shieldButton.GetComponent<Skill>().ClickSkill(shieldReloadTime);
        }
    }

    // @access from Level canvas
    public void ClickSpeedSkill()
    {
        if (player.speedCount > 0)
        {
            player.speedCount--;
            player.SavePlayer();
            SetScoreboardValues();

            ball.UseSpeedSkill();
            speedButton.GetComponent<Skill>().ClickSkill(speedReloadTime);
        }
    }

    // @access from Level canvas
    public void ClickDoubleRewardButton()
    {
        coins *= 2;
        diamonds *= 2;

        doubleRewardButton.GetComponent<AnimationTrigger>().Trigger("Start");
        doubleRewardButton.GetComponent<Button>().interactable = false;

        subCoinCount.gameObject.GetComponent<AnimationTrigger>().Trigger("Start");
        subDiamondCount.gameObject.GetComponent<AnimationTrigger>().Trigger("Start");

        SetScoreboardValues();
    }

    // @access from Level canvas
    public void ClickNextLevelButton()
    {
        player.coins += coins;
        player.diamonds += diamonds;
        player.redKeyCount += redKeys;
        player.goldKeyCount += goldKeys;
        player.silverKeyCount += silverKeys;

        player.SavePlayer();

        LoadNextLevel();
    }

    // @access from Level canvas
    public void ClickGiftButton()
    {
        giftWindow.GetComponent<GiftWindow>().OpenGiftWindow();
        DisableGiftButton();
    }

    // @access from Gift Window
    public void CollectGifts()
    {
        List<GameObject> spinners = giftWindow.GetComponent<GiftWindow>().GetCompleteSpinners();

        for (int i = 0; i < spinners.Count; i++)
        {
            int count = spinners[i].GetComponent<Spinner>().GetGiftCount();
            switch (spinners[i].GetComponent<Spinner>().GetGiftName())
            {
                case "Bullet":
                    player.bulletCount += count;
                    break;
                case "Speed":
                    player.speedCount += count;
                    break;
                case "Shield":
                    player.shieldCount += count;
                    break;
                case "Lightning":
                    player.lightningCount += count;
                    break;
                case "SilverKey":
                    player.silverKeyCount += count;
                    break;
                case "GoldKey":
                    player.goldKeyCount += count;
                    break;
                case "RedKey":
                    player.redKeyCount += count;
                    break;
                case "Diamond":
                    player.diamonds += count;
                    break;
                case "Coin":
                    player.coins += count;
                    break;
            }
        }

        // Save click
        System.DateTimeOffset now = System.DateTimeOffset.UtcNow;
        long date = now.ToUnixTimeMilliseconds();
        player.spinnerCollects.Add(date);

        player.SavePlayer();
        SetScoreboardValues();

        CloseGiftWindow();
    }

    public void CloseGiftWindow()
    {
        giftWindow.GetComponent<GiftWindow>().CloseGiftWindow();
        ReloadGiftButton();
    }
    #endregion
}
