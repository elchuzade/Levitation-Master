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

    [SerializeField] Text coinsCount;
    [SerializeField] Text diamondsCount;
    [SerializeField] GameObject coinsIcon;
    [SerializeField] GameObject diamondsIcon;
    [SerializeField] GameObject redKeyItem;
    [SerializeField] GameObject goldKeyItem;
    [SerializeField] GameObject silverKeyItem;

    [SerializeField] GameObject lightningSkill;
    [SerializeField] GameObject lightningButton;

    [SerializeField] GameObject shootingButton;
    [SerializeField] GameObject bulletPrefab;

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
        //player.ResetPlayer();
        player.LoadPlayer();

        SetScoreboardValues();
        levelText.text = player.nextLevelIndex.ToString();
    }

    #region Private Methods
    void SetScoreboardValues()
    {
        coinsCount.text = (player.coins + coins).ToString();
        diamondsCount.text = (player.diamonds + diamonds).ToString();
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
    public void OpenBox(Boxes box, int amount)
    {
        switch (box)
        {
            case Boxes.Coin:
                DropCoins(amount, transform.position);
                break;
            case Boxes.Diamond:
                DropDiamonds(amount, transform.position);
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
                OpenBox((Boxes)Random.Range(0, 4), amount);
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
        // Shoot lightning effect on the ball and stop the ball for a second
        lightningSkill.transform.position = ball.transform.position;
        lightningSkill.SetActive(true);
        lightningButton.GetComponent<AnimationTrigger>().Trigger("Start");
        ball.StrikeLighting();
        StartCoroutine(StopLightningSkill());
    }

    // @access from Level canvas
    public void ClickShootingSkill()
    {
        // Shoot lightning effect on the ball and stop the ball for a second
        GameObject bulletInstance = Instantiate(bulletPrefab, ball.transform.position, Quaternion.identity);
        bulletInstance.transform.SetParent(platformItems.transform);

        shootingButton.GetComponent<AnimationTrigger>().Trigger("Start");
    }
    #endregion

    #region Coroutines
    IEnumerator StopLightningSkill()
    {
        yield return new WaitForSeconds(0.5f);
        lightningSkill.SetActive(false);
    }
    #endregion
}
