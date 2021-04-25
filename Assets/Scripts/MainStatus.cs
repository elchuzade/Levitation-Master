using UnityEngine;
using UnityEngine.UI;
using static Server;

public class MainStatus : MonoBehaviour
{
    Navigator navigator;
    Player player;
    Server server;
    TV tv;

    [SerializeField] Text diamondsText;
    [SerializeField] Text coinsText;

    // To run animation and to show notification sign
    [SerializeField] GameObject chestButton;
    [SerializeField] GameObject chestNotification;
    [SerializeField] GameObject hapticsButton;
    [SerializeField] GameObject privacyWindow;
    [SerializeField] GameObject quitGameWindow;
    [SerializeField] GameObject leaderboardButton;

    void Awake()
    {
        navigator = FindObjectOfType<Navigator>();
        server = FindObjectOfType<Server>();
        tv = FindObjectOfType<TV>();

        hapticsButton = GameObject.Find("HapticsButton");
    }

    void Start()
    {
        player = FindObjectOfType<Player>();
        player.ResetPlayer();
        player.LoadPlayer();

        server.GetVideoLink();

        if (player.privacyPolicy)
        {
            privacyWindow.SetActive(false);
            leaderboardButton.GetComponent<Button>().onClick.AddListener(() => ClickLeaderboardButton());

            if (!player.playerCreated)
            {
                server.CreatePlayer(player);
            }
            else
            {
                server.SavePlayerData(player);
            }
        }
        else
        {
            privacyWindow.SetActive(true);
            leaderboardButton.GetComponent<Button>().onClick.AddListener(() => ShowPrivacyPolicy());
            leaderboardButton.GetComponent<Image>().color = new Color32(255, 197, 158, 100);
        }

        SetScoreboardValues();
        SetButtonInitialState();
        SetChestButton();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            quitGameWindow.SetActive(true);
        }
    }

    // @access from server
    public void CreatePlayerSuccess()
    {
        player.playerCreated = true;
        player.SavePlayer();
    }

    // Set video link from server file
    public void SetVideoLinkSuccess(VideoJson response)
    {
        tv.SetAdLink(response.video);
        tv.SetAdButton(response.website);
    }

    private void SetChestButton()
    {
        if (player.redChestCount > 0 || player.goldChestCount > 0 || player.silverChestCount > 0)
        {
            chestNotification.SetActive(true);
            chestButton.GetComponent<Animator>().Play("ChestShake");
        }
        else
        {
            chestNotification.SetActive(false);
            chestButton.GetComponent<Animator>().enabled = false;
        }
    }

    private void SetScoreboardValues()
    {
        diamondsText.text = player.diamonds.ToString();
        coinsText.text = player.coins.ToString();
    }

    public void ClickPlayButton()
    {
        navigator.LoadNextLevel(player.nextLevelIndex);
    }

    public void ClickChestButton()
    {
        navigator.LoadChests();
    }

    public void ClickShopButton()
    {
        navigator.LoadShop();
    }

    public void ClickLeaderboardButton()
    {
        navigator.LoadLeaderboard();
    }

    public void ClickHapticsButton()
    {
        if (PlayerPrefs.GetInt("Haptics") == 1)
        {
            // Set button state to disabled
            hapticsButton.transform.Find("Disabled").gameObject.SetActive(true);
            // If haptics are turned on => turn them off
            PlayerPrefs.SetInt("Haptics", 0);
        }
        else
        {
            // Set button state to enabled
            hapticsButton.transform.Find("Disabled").gameObject.SetActive(false);
            // If haptics are turned off => turn them on
            PlayerPrefs.SetInt("Haptics", 1);
        }
    }

    // Set initial states of haptics button based on player prefs
    private void SetButtonInitialState()
    {
        if (PlayerPrefs.GetInt("Haptics") == 1)
        {
            hapticsButton.transform.Find("Disabled").gameObject.SetActive(false);
        }
        else
        {
            hapticsButton.transform.Find("Disabled").gameObject.SetActive(true);
        }
    }

    public void ShowPrivacyPolicy()
    {
        privacyWindow.SetActive(true);
    }

    public void ClickDeclinePrivacyPolicy()
    {
        leaderboardButton.GetComponent<Button>().onClick.AddListener(() => privacyWindow.SetActive(true));
        privacyWindow.SetActive(false);
    }

    public void ClickAcceptPrivacyPolicy()
    {
        leaderboardButton.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        leaderboardButton.GetComponent<Button>().onClick.AddListener(() => ClickLeaderboardButton());

        privacyWindow.transform.localScale = new Vector3(0, 1, 1);
        privacyWindow.SetActive(false);
        player.privacyPolicy = true;
        player.SavePlayer();

        server.CreatePlayer(player);
    }

    public void ClickQuitGame()
    {
        Application.Quit();
    }

    public void CancelQuitGame()
    {
        quitGameWindow.SetActive(false);
    }

    public void ClickTermsOfUse()
    {
        Application.OpenURL("https://abboxgames.com/terms-of-use");
    }

    public void ClickPrivacyPolicy()
    {
        Application.OpenURL("https://abboxgames.com/privacy-policy");
    }
}