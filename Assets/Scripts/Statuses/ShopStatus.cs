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
    [SerializeField] GameObject scrollContent;

    [SerializeField] GameObject canvas;

    int ballIndex;

    //GameObject ballRising;

    void Awake()
    {
        navigator = FindObjectOfType<Navigator>();
    }

    void Start()
    {
        for (int i = 0; i < scrollContent.transform.childCount; i++)
        {
            RectTransform rt = (RectTransform)scrollContent.transform.GetChild(i).transform;
            float height = rt.rect.height;

            RectTransform canvasRt = (RectTransform)canvas.transform;
            float width = canvasRt.rect.width;

            scrollContent.transform.GetChild(i).GetComponent<RectTransform>().sizeDelta = new Vector2(width - 200, height);
        }

        player = FindObjectOfType<Player>();
        //player.ResetPlayer();
        player.LoadPlayer();

        SetScoreboardValues();
        SetPlayerBalls();

        scrollbar.GetComponent<Scrollbar>().numberOfSteps = 25;
        scrollbar.GetComponent<Scrollbar>().value = (float)player.currentBallIndex / 24;

        //scrollbar.GetComponent<Scrollbar>().onValueChanged.AddListener(value => SwipeBall(value));

        SetBallValues(player.currentBallIndex);

        // 0.125 is the step size based on 0 - 1 and number of transitions between balls 1 / 24
        SwipeBall((float)1 / 24 * player.currentBallIndex);

        // Save click
        System.DateTimeOffset now = System.DateTimeOffset.UtcNow;
        long date = now.ToUnixTimeMilliseconds();
        player.shopClicks.Add(date);
        player.SavePlayer();
    }

    #region Public Methods
    // @access from Shop canvas
    public void SwipeBall(float value)
    {
        ballIndex = (int)(value * 24);
        SetBallValues(ballIndex);
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
    public void ClickUnlockShopItem(int index)
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
            //Debug.Log(ballIndex);
            scrollbar.GetComponent<Scrollbar>().value = (float)ballIndex / 24;
            SetBallValues(ballIndex);
        }
    }

    // @access from Shop canvas
    public void ClickRightArrowButton()
    {
        rightArrowButton.GetComponent<AnimationTrigger>().Trigger("Start");
        if (ballIndex < allBalls.Length - 1)
        {
            ballIndex++;
            //Debug.Log(ballIndex);
            scrollbar.GetComponent<Scrollbar>().value = (float)ballIndex / 24;
            SetBallValues(ballIndex);
        }
    }
    #endregion

    #region Private Methods
    void SetBallValues(int ballIndex)
    {
        //if (ballRising)
        //{
        //    Destroy(ballRising);
        //}
        ////Debug.Log("instantiating " + ballIndex);
        //ballRising = Instantiate(allBallsRising[ballIndex], new Vector3(375, 907, 600
        //    ), Quaternion.identity);

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
