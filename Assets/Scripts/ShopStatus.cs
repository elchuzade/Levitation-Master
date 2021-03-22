using UnityEngine;
using UnityEngine.UI;
using static GlobalVariables;

public class ShopStatus : MonoBehaviour
{
    Player player;
    Navigator navigator;

    [SerializeField] Text diamondsText;
    [SerializeField] Text coinsText;

    [SerializeField] GameObject[] allBalls;

    void Awake()
    {
        navigator = FindObjectOfType<Navigator>();
    }

    void Start()
    {
        player = FindObjectOfType<Player>();
        //player.ResetPlayer();
        player.LoadPlayer();

        SetScoreboardValues();
        SetPlayerBalls();
    }

    private void SetPlayerBalls()
    {
        for (int i = 0; i < player.allBalls.Count; i++)
        {
            if (player.allBalls[i] == 1)
            {
                allBalls[i].GetComponent<ShopItem>().BuyItem();
            }
        }
        allBalls[player.currentBallIndex].GetComponent<ShopItem>().SelectItem();
    }

    private void SetScoreboardValues()
    {
        diamondsText.text = player.diamonds.ToString();
        coinsText.text = player.coins.ToString();
    }

    public void ClickShopItem(int index)
    {
        if (player.allBalls[index] == 1)
        {
            allBalls[player.currentBallIndex].GetComponent<ShopItem>().DeselectItem();
            // Ball is already bought, just select it
            allBalls[index].GetComponent<ShopItem>().SelectItem();
            player.currentBallIndex = index;
            player.SavePlayer();
            SetScoreboardValues();
        } else
        {
            // Ball is not bought, try to buy it
            if (allBalls[index].GetComponent<ShopItem>().GetCurrency() == Currency.Coin &&
                player.coins >= allBalls[index].GetComponent<ShopItem>().GetPrice())
            {
                // Buy the ball abd charge player
                player.coins -= allBalls[index].GetComponent<ShopItem>().GetPrice();
                allBalls[index].GetComponent<ShopItem>().BuyItem();
                allBalls[player.currentBallIndex].GetComponent<ShopItem>().DeselectItem();
                allBalls[index].GetComponent<ShopItem>().SelectItem();
                player.currentBallIndex = index;
                player.allBalls[index] = 1;
                player.SavePlayer();
                SetScoreboardValues();
            }
            else if (allBalls[index].GetComponent<ShopItem>().GetCurrency() == Currency.Diamond &&
                player.diamonds >= allBalls[index].GetComponent<ShopItem>().GetPrice())
            {
                // Buy the ball abd charge player
                player.diamonds -= allBalls[index].GetComponent<ShopItem>().GetPrice();
                allBalls[index].GetComponent<ShopItem>().BuyItem();
                allBalls[player.currentBallIndex].GetComponent<ShopItem>().DeselectItem();
                allBalls[index].GetComponent<ShopItem>().SelectItem();
                player.currentBallIndex = index;
                player.allBalls[index] = 1;
                player.SavePlayer();
                SetScoreboardValues();
            }
        }
    }

    public void ClickBackButton()
    {
        navigator.LoadMainScene();
    }
}
