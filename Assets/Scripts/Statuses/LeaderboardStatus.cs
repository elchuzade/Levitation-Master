using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Server;

public class LeaderboardStatus : MonoBehaviour
{
    // To send player data to server
    private class PlayerJson
    {
        public string playerId;
        public PlayerData playerData;
    }

    Player player;
    Navigator navigator;
    Server server;

    [SerializeField] Text diamondsText;
    [SerializeField] Text coinsText;
    [SerializeField] GameObject diamondIcon;

    // Single line of leadersboard
    [SerializeField] GameObject leaderboardItemPrefab;
    [SerializeField] GameObject leaderboardItemTrippleDots;

    // This is for saving the name before it has been changed, so receive diamonds
    [SerializeField] GameObject changeNameGetDiamondsButton;
    // This is for saving the name after it has been changed
    [SerializeField] GameObject changeNameSaveButton;
    // This is the window where you change your name
    [SerializeField] GameObject changeNameWindow;
    // To set parent for leaderboard items
    [SerializeField] GameObject leaderboardScrollContent;
    // To scroll down to your position
    [SerializeField] GameObject leaderboardScrollbar;
    // To point at change name button
    [SerializeField] GameObject arrow;
    // To extract value of input field when save or get 3 diamonds is clicked
    [SerializeField] InputField nameInput;

    List<LeaderboardItem> before = new List<LeaderboardItem>();
    List<LeaderboardItem> after = new List<LeaderboardItem>();
    List<LeaderboardItem> top = new List<LeaderboardItem>();
    LeaderboardItem you = new LeaderboardItem();

    Color32 goldColor = new Color32(255, 215, 0, 255);
    Color32 silverColor = new Color32(192, 192, 192, 255);
    Color32 bronzeColor = new Color32(205, 127, 50, 255);

    void Awake()
    {
        server = FindObjectOfType<Server>();
        navigator = FindObjectOfType<Navigator>();
    }

    void Start()
    {
        player = FindObjectOfType<Player>();
        //player.ResetPlayer();
        player.LoadPlayer();

        SetPlayerValues();
        SwapSaveButton();

        server.GetLeaderboard();

        // Save click
        System.DateTimeOffset now = System.DateTimeOffset.UtcNow;
        long date = now.ToUnixTimeMilliseconds();
        player.leaderboardClicks.Add(date);
        player.SavePlayer();
    }

    #region Private Methods
    void SetPlayerValues()
    {
        diamondsText.text = player.diamonds.ToString();
        coinsText.text = player.coins.ToString();
    }

    void SwapSaveButton()
    {
        // Fetch data from server and choose the button to show
        if (player.nameChanged)
        {
            // The name sent from the server
            arrow.SetActive(false);
            changeNameSaveButton.SetActive(true);
            changeNameGetDiamondsButton.SetActive(false);
        }
        else
        {
            arrow.SetActive(true);
            changeNameSaveButton.SetActive(false);
            changeNameGetDiamondsButton.SetActive(true);
        }
    }

    void ScrollListToPlayer()
    {
        // Combine all the values from all three lists top, before after
        int total = top.Count + before.Count + after.Count;
        // Increase by one for your rank if it is outside of top ten
        if (you.rank != 0)
        {
            total++;
        }
        // based on total find where to place the scroll
        if (total > 10)
        {
            // If total is greater than 10, it is safe to show the bottom 5 players to make sure you are also visible
            leaderboardScrollbar.GetComponent<Scrollbar>().value = 0.001f;
        }
        else
        {
            // If you are in the top ten, then if you are in top 5, show first 5 players to make sure you are also visible
            if (you.rank < 6)
            {
                leaderboardScrollbar.GetComponent<Scrollbar>().value = 0.999f;
            }
            else
            {
                // Otherwise your rank is in the range of 5-10, so it is safe to show middle 5 players to make sure you are also visible
                leaderboardScrollbar.GetComponent<Scrollbar>().value = 0.5f;
            }
        }
    }

    // Loop through top ten, 3 before and 3 after lists to find if give data exists not to repeat
    bool CheckIfExists(int rank)
    {
        if (CheckIfExistInTop(rank) ||
            CheckIfExistInBefore(rank) ||
            CheckIfExistInAfter(rank))
        {
            return true;
        }
        return false;
    }

    // Loop through the list of players ranked in top ten and see if iven data exists
    bool CheckIfExistInTop(int rank)
    {
        for (int i = 0; i < top.Count; i++)
        {
            if (top[i].rank == rank)
            {
                return true;
            }
        }
        return false;
    }

    // Loop through the list of players ranked before you and see if iven data exists
    bool CheckIfExistInBefore(int rank)
    {
        for (int i = 0; i < before.Count; i++)
        {
            if (before[i].rank == rank)
            {
                return true;
            }
        }
        return false;
    }

    GameObject InstantiateLeaderboardItem(int rank)
    {
        GameObject leaderboardItem = Instantiate(leaderboardItemPrefab, transform.position, Quaternion.identity);
        return leaderboardItem;
    }

    void BuildUpList()
    {
        // Loop through top ten players and instantiate an item object
        top.ForEach(item =>
        {
            // Check for revert leaderboard items
            GameObject leaderboardItem = InstantiateLeaderboardItem(item.rank);
            
            // Set its parent to be scroll content, for scroll functionality to work properly
            leaderboardItem.transform.SetParent(leaderboardScrollContent.transform);
            leaderboardItem.transform.localScale = Vector3.one;

            if (item.rank == 1)
            {
                leaderboardItem.transform.Find("Background").GetComponent<Image>().color = goldColor;
            }
            else if (item.rank == 2)
            {
                leaderboardItem.transform.Find("Background").GetComponent<Image>().color = silverColor;
            }
            else if (item.rank == 3)
            {
                leaderboardItem.transform.Find("Background").GetComponent<Image>().color = bronzeColor;
            }

            // Compare item from top ten with your rank incase you are in top ten
            if (item.rank == you.rank)
            {
                // Show frame around your entry
                ShowYourEntryFrame(leaderboardItem);
            }

            // Set its name component text mesh pro value to name from top list
            leaderboardItem.transform.Find("Name").GetComponent<Text>().text = item.playerName;
            // Set its rank component text to rank from top list converted to string
            leaderboardItem.transform.Find("Rank").GetComponent<Text>().text = item.rank.ToString();
        });

        // Add tripple dots after top ten only if your rank is > 14,
        // since at 14 the the top ten and 3 before you become continuous, so no need for dots in between
        if (you.rank > 14)
        {
            CreateTrippleDotsEntry();
        }

        // Loop through before players and instantiate an item object
        before.ForEach(item =>
        {
            // Check for revert leaderboard items
            GameObject leaderboardItem = InstantiateLeaderboardItem(item.rank);
            
            // Set its parent to be scroll content, for scroll functionality to work properly
            leaderboardItem.transform.SetParent(leaderboardScrollContent.transform);
            leaderboardItem.transform.localScale = Vector3.one;

            SetItemEntry(leaderboardItem, item);
        });

        // Create your entry item only if your rank is not in top ten
        // 0 is assigned by default if there is no value

        if (you.rank > 10)
        {
            CreateYourEntry();
        }

        // Loop through after players and instantiate an item object
        after.ForEach(item =>
        {
            // Check for revert leaderboard items
            GameObject leaderboardItem = InstantiateLeaderboardItem(item.rank);
            
            // Set its parent to be scroll content, for scroll functionality to work properly
            leaderboardItem.transform.SetParent(leaderboardScrollContent.transform);
            leaderboardItem.transform.localScale = Vector3.one;

            SetItemEntry(leaderboardItem, item);
        });

        CreateTrippleDotsEntry();

        // Add the scroll value after all the data is populated
        ScrollListToPlayer();
    }

    void CreateTrippleDotsEntry()
    {
        // Create tripple dots to separate different lists
        GameObject leaderboardItem = Instantiate(leaderboardItemTrippleDots, transform.position, Quaternion.identity);
        // Set its parent to be scroll content, for scroll functionality to work properly
        leaderboardItem.transform.SetParent(leaderboardScrollContent.transform);
        leaderboardItem.transform.localScale = Vector3.one;
    }

    void CreateYourEntry()
    {
        // Create tripple dots to separate different lists
        GameObject leaderboardItem = Instantiate(leaderboardItemPrefab, transform.position, Quaternion.identity);
        
        // Set its parent to be scroll content, for scroll functionality to work properly
        leaderboardItem.transform.SetParent(leaderboardScrollContent.transform);
        // Show frame around your entry
        ShowYourEntryFrame(leaderboardItem);
        SetItemEntry(leaderboardItem, you);
    }

    void SetItemEntry(GameObject item, LeaderboardItem value)
    {
        // Set its name component text mesh pro value to your name
        item.transform.Find("Name").GetComponent<Text>().text = value.playerName;
        // Set its rank component text mesh pro value to your rank
        item.transform.Find("Rank").GetComponent<Text>().text = value.rank.ToString();
    }

    void ShowYourEntryFrame(GameObject item)
    {
        // Show leaderboard frame for your entry
        item.transform.Find("Frame").gameObject.SetActive(true);
    }

    // Loop through the list of players ranked after you and see if iven data exists
    bool CheckIfExistInAfter(int rank)
    {
        for (int i = 0; i < after.Count; i++)
        {
            if (after[i].rank == rank)
            {
                return true;
            }
        }
        return false;
    }
    #endregion

    #region Public Methods
    // @access from Server script
    public void ChangeNameError()
    {
        // Repopulate leaderboard data
        server.GetLeaderboard();
    }

    // @access from Server script
    public void ChangeNameSuccess()
    {
        // Repopulate leaderboard data
        server.GetLeaderboard();
    }

    // @access from Server script
    public void SetLeaderboardData(List<LeaderboardItem> topData, List<LeaderboardItem> beforeData, LeaderboardItem youData, List<LeaderboardItem> afterData)
    {
        // Clear the lists incase they already had data in them
        foreach (Transform child in leaderboardScrollContent.transform)
        {
            Destroy(child.gameObject);
        }
        top.Clear();
        before.Clear();
        after.Clear();
        if (topData != null)
        {
            // Loop though top ten list provided by the server and add them to local list
            for (int i = 0; i < topData.Count; i++)
            {
                top.Add(topData[i]);
            }
        }
        if (beforeData != null)
        {
            // Loop though up to 3 players before you list provided by the server
            // and if they have not yet been added to the list add them
            for (int i = 0; i < beforeData.Count; i++)
            {
                if (!CheckIfExists(beforeData[i].rank))
                {
                    before.Add(beforeData[i]);
                }
            }
        }
        if (youData != null)
        {
            you.rank = youData.rank;
            // Check if your rank has already been added to the list if not add it
            if (!CheckIfExists(youData.rank))
            {
                you = youData;
                // Set the name in the change name field to prepopulate
                nameInput.text = youData.playerName;
            }
        }
        if (afterData != null)
        {
            // Loop though up to 3 players after you list provided by the server
            // and if they have not yet been added to the list add them
            for (int i = 0; i < afterData.Count; i++)
            {
                if (!CheckIfExists(afterData[i].rank))
                {
                    after.Add(afterData[i]);
                }
            }
        }

        BuildUpList();
    }

    // @access from Leaderboard canvas
    public void ClickBackButton()
    {
        navigator.LoadMainScene();
    }

    // @access from Leaderboard canvas
    public void ClickEditButton()
    {
        changeNameWindow.SetActive(true);
    }

    // @access from Leaderboard canvas
    // This is for invisible button that covers the rest of the screen when modal is open
    public void CloseChangeName()
    {
        changeNameWindow.SetActive(false);
        SwapSaveButton();
    }

    // @access from Leaderboard canvas
    public void ClickSaveName()
    {
        server.ChangePlayerName(nameInput.text);
        if (player.playerName.Length < 2)
        {
            player.playerName = nameInput.text;
            player.diamonds += 3;
            player.nameChanged = true;
            player.SavePlayer();
            player.LoadPlayer();
            SetPlayerValues();
        }

        CloseChangeName();
    }
    #endregion
}
