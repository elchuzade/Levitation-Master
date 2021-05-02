using UnityEngine;
using UnityEngine.UI;
using static GlobalVariables;

public class ShopStatus : MonoBehaviour
{
    Player player;
    Navigator navigator;

    [SerializeField] Text diamondsText;
    [SerializeField] Text coinsText;

    [SerializeField] GameObject leftArrowButton;
    [SerializeField] GameObject rightArrowButton;

    [SerializeField] GameObject scrollbar;

    [SerializeField] GameObject[] allBalls;
    [SerializeField] GameObject[] allBallsRising;

    int ballIndex;

    GameObject ballRising;

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

        scrollbar.GetComponent<Scrollbar>().value = (float)player.currentBallIndex / 8;
        scrollbar.GetComponent<Scrollbar>().onValueChanged.AddListener(value => SwipeBall(value));

        SetBallValues();
    }

    #region Public Methods
    // @access from Shop canvas
    public void SwipeBall(float value)
    {
        ballIndex = (int)(value * 8);
        SetBallValues();
    }

    // @access from Shop canvas
    public void ClickSelectShopItem(int index)
    {
        for (int i = 0; i < allBalls.Length; i++)
        {
            if (allBalls[i].GetComponent<ShopItem>().GetIndex() == index)
            {
                allBalls[i].GetComponent<ShopItem>().SelectItem();
            }
            else if (allBalls[i].GetComponent<ShopItem>().GetIndex() == ballIndex)
            {
                allBalls[i].GetComponent<ShopItem>().DeselectItem();
            }
        }

        ballIndex = index;
        player.currentBallIndex = ballIndex;
        player.SavePlayer();
    }

    // @access from Shop canvas
    public void ClickBuyShopItem(int index)
    {
        if (player.allBalls[index] == 1)
        {
            allBalls[player.currentBallIndex].GetComponent<ShopItem>().DeselectItem();
            // Ball is already bought, just select it
            allBalls[index].GetComponent<ShopItem>().SelectItem();
            player.currentBallIndex = index;
            player.SavePlayer();
            SetScoreboardValues();
        }
        else
        {
            // Ball is not bought, try to buy it
            if (allBalls[index].GetComponent<ShopItem>().GetCurrency() == Currency.Coin &&
                player.coins >= allBalls[index].GetComponent<ShopItem>().GetPrice())
            {
                // Buy the ball abd charge player
                player.coins -= allBalls[index].GetComponent<ShopItem>().GetPrice();
                allBalls[index].GetComponent<ShopItem>().UnlockItem();
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
                allBalls[index].GetComponent<ShopItem>().UnlockItem();
                allBalls[player.currentBallIndex].GetComponent<ShopItem>().DeselectItem();
                allBalls[index].GetComponent<ShopItem>().SelectItem();
                player.currentBallIndex = index;
                player.allBalls[index] = 1;
                player.SavePlayer();
                SetScoreboardValues();
            }
        }
    }

    // @access from Shop canvas
    public void ClickBackButton()
    {
        navigator.LoadMainScene();
    }

    // @access from Shop canvas
    public void ClickLeftArrowButton()
    {
        leftArrowButton.GetComponent<AnimationTrigger>().Trigger("Start");
        if (ballIndex > 0)
        {
            ballIndex--;
            scrollbar.GetComponent<Scrollbar>().value = (float)ballIndex / 8;
            SetBallValues();
        }
    }

    // @access from Shop canvas
    public void ClickRightArrowButton()
    {
        rightArrowButton.GetComponent<AnimationTrigger>().Trigger("Start");
        if (ballIndex < allBalls.Length - 1)
        {
            ballIndex++;
            scrollbar.GetComponent<Scrollbar>().value = (float)ballIndex / 8;
            SetBallValues();
        }
    }
    #endregion

    #region Private Methods
    void SetBallValues()
    {
        if (ballRising)
        {
            Destroy(ballRising);
        }
        ballRising = Instantiate(allBallsRising[ballIndex], new Vector3(375, 907, 600
            ), Quaternion.identity);

        // Set arrows
        leftArrowButton.GetComponent<Button>().interactable = true;
        rightArrowButton.GetComponent<Button>().interactable = true;

        leftArrowButton.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        rightArrowButton.GetComponent<Image>().color = new Color32(255, 255, 255, 255);

        if (ballIndex == 0)
        {
            leftArrowButton.GetComponent<Button>().interactable = false;
            leftArrowButton.GetComponent<Image>().color = new Color32(100, 100, 100, 255);
        }
        else if (ballIndex == allBalls.Length - 1)
        {
            rightArrowButton.GetComponent<Button>().interactable = false;
            rightArrowButton.GetComponent<Image>().color = new Color32(100, 100, 100, 255);
        }
        for (int i = 0; i < allBalls.Length; i++)
        {
            if (player.allBalls[i] == 1)
            {
                allBalls[i].GetComponent<ShopItem>().UnlockItem();
            }
            if (player.currentBallIndex == i)
            {
                allBalls[i].GetComponent<ShopItem>().SelectItem();
            }
        }
    }

    void SetPlayerBalls()
    {
        for (int i = 0; i < player.allBalls.Count; i++)
        {
            if (player.allBalls[i] == 1)
            {
                allBalls[i].GetComponent<ShopItem>().UnlockItem();
            }
        }
        allBalls[player.currentBallIndex].GetComponent<ShopItem>().SelectItem();
    }

    void SetScoreboardValues()
    {
        diamondsText.text = player.diamonds.ToString();
        coinsText.text = player.coins.ToString();
    }
    #endregion
}
