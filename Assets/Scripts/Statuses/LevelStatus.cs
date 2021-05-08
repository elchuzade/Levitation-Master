using System.Collections;
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

    [SerializeField] GameObject lightningItem;
    [SerializeField] GameObject shieldItem;

    [SerializeField] GameObject allBuffs;
    [SerializeField] GameObject allKeys;
    // To set parent of coins and diamonds
    [SerializeField] GameObject platformItems;

    Ball ball;

    [SerializeField] Text coinCount;
    [SerializeField] Text subCoinCount;
    [SerializeField] Text diamondCount;
    [SerializeField] Text subDiamondCount;

    [SerializeField] Text bulletCount;
    [SerializeField] Text shieldCount;

    [SerializeField] GameObject coinsIcon;
    [SerializeField] GameObject diamondsIcon;

    [SerializeField] GameObject redKeyItem;
    [SerializeField] GameObject goldKeyItem;
    [SerializeField] GameObject silverKeyItem;

    [SerializeField] GameObject lightningButton;
    [SerializeField] GameObject shieldButton;
    [SerializeField] GameObject bulletButton;

    [SerializeField] GameObject doubleRewardWindow;
    [SerializeField] GameObject doubleRewardButton;
    [SerializeField] GameObject levelControlsWindow;

    [SerializeField] float lightningReloadTime;
    [SerializeField] float bulletReloadTime;
    [SerializeField] float shieldReloadTime;

    int coins;
    int diamonds;
    int redKeys;
    int silverKeys;
    int goldKeys;

    void Awake()
    {
        ball = FindObjectOfType<Ball>();
        navigator = FindObjectOfType<Navigator>();
    }

    void Start()
    {
        player = FindObjectOfType<Player>();
        player.ResetPlayer();
        player.LoadPlayer();

        SetScoreboardValues();
        levelText.text = player.nextLevelIndex.ToString();
    }

    #region Private Methods
    void SetScoreboardValues()
    {
        coinCount.text = player.coins.ToString();
        diamondCount.text = player.diamonds.ToString();

        bulletButton.GetComponent<Skill>().SetSkillCount(player.bulletCount);
        lightningButton.GetComponent<Skill>().SetSkillCount(player.lightningCount);
        shieldButton.GetComponent<Skill>().SetSkillCount(player.shieldCount);

        shieldCount.text = player.shieldCount.ToString();

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

    void ResetBuff(Buff buff)
    {
        for (int i = 0; i < allBuffs.transform.childCount; i++)
        {
            // Loop through all the buffs and find the one that needs to be reset and reset it
            if (allBuffs.transform.GetChild(i).GetComponent<BuffItem>().GetBuff() == buff)
            {
                allBuffs.transform.GetChild(i).GetComponent<BuffItem>().ResetBuff();
            }
        }
    }
    #endregion

    #region Public Methods
    // @access from Ball script
    public void SetNextLevelMeter()
    {
        levelText.text = (player.nextLevelIndex + 1).ToString();
        levelControlsWindow.SetActive(false);
        doubleRewardWindow.SetActive(true);
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
            case Boxes.Coin:
                DropCoins(amount, position);
                break;
            case Boxes.Diamond:
                DropDiamonds(amount, position);
                break;
            case Boxes.Shield:
                if (ball.GetShield())
                {
                    ResetBuff(Buff.Shield);
                    ball.DestroyBuff(Buff.Shield);
                    ball.SetBuff(Buff.Shield);
                }
                else
                {
                    GameObject shieldInstance = Instantiate(shieldItem, transform.position, Quaternion.identity);
                    shieldInstance.transform.SetParent(allBuffs.transform);
                    ball.SetBuff(Buff.Shield);
                }
                break;
            case Boxes.Lightning:
                if (ball.GetLightning())
                {
                    ResetBuff(Buff.Lightning);
                    ball.DestroyBuff(Buff.Lightning);
                    ball.SetBuff(Buff.Lightning);
                }
                else
                {
                    GameObject lightningInstance = Instantiate(lightningItem, transform.position, Quaternion.identity);
                    lightningInstance.transform.SetParent(allBuffs.transform);
                    ball.SetBuff(Buff.Lightning);
                }
                break;
            case Boxes.Question:
                OpenBox((Boxes)Random.Range(0, 4), amount, position);
                break;
        }
    }

    // @access from box when it is hit by the ball
    public void DropCoins(int amount, Vector3 position)
    {
        for (int i = 0; i < amount; i++)
        {
            Vector3 coordShift = new Vector3(Random.Range(-30, 30), 0, Random.Range(-30, 30));
            GameObject coinInstance = Instantiate(coinPrefab, position, Quaternion.identity);
            coinInstance.transform.SetParent(platformItems.transform);
            coinInstance.transform.GetComponent<Collectable>().SetDropPosition(position + coordShift);
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
            diamondInstance.transform.GetComponent<Collectable>().SetDropPosition(position + coordShift);
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
        navigator.LoadNextLevel(2);
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

            ball.UseLightingSkill();
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

        player.SavePlayer();

        LoadNextLevel();
    }
    #endregion

    #region Coroutines
    #endregion
}
