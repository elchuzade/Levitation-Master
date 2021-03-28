using UnityEngine;
using UnityEngine.UI;
using static GlobalVariables;

public class LevelStatus : MonoBehaviour
{
    Player player;
    Navigator navigator;

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
    }

    private void SetScoreboardValues()
    {
        coinsCount.text = (player.coins + coins).ToString();
        diamondsCount.text = (player.diamonds + diamonds).ToString();
    }

    // @access from Collectable script
    public void CollectReward(Rewards reward)
    {
        switch (reward)
        {
            case Rewards.Coin:
                coins++;
                coinsIcon.GetComponent<TriggerAnimation>().Trigger();
                break;
            case Rewards.Diamond:
                diamonds++;
                diamondsIcon.GetComponent<TriggerAnimation>().Trigger();
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
                } else
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

    private void ResetBuff(Buff buff)
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

    public void CompleteLevel()
    {
        Camera.main.GetComponent<CameraResizer>().CameraFollowBall();
    }

    public void ClickHomeButton()
    {
        navigator.LoadMainScene();
    }
}
