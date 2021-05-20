using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using static Utilities;

public class Server : MonoBehaviour
{
    public class Header
    {
        public string deviceId;
        public string deviceOS;
    }

    // Change player name
    public class PlayerName
    {
        public string playerName;
    }
    // Each row of leaderboard
    public class LeaderboardItem
    {
        public string playerName;
        public int rank;
    }

    // Video Link
    public class VideoJson
    {
        public string id; // link id to measure clicks
        public string video; // link to video
        public string name; // product title
        public string website; // link to follow on click
        public string playMarket; // link to follow on click
        public string appStore; // link to follow on click
    }

    public class PlayerData
    {
        public int coins;
        public int diamonds;
        public int nextLevelIndex;
        public int currentBallIndex;
        public bool maxLevelReached;

        public List<int> allBalls;
        public List<int> levelsAfterMaxReached;

        public int redKeyCount;
        public int goldKeyCount;
        public int silverKeyCount;

        public int bulletCount;
        public int lightningCount;
        public int shieldCount;
        public int speedCount;

        public List<long> redChestBuys;
        public List<long> goldChestBuys;
        public List<long> silverChestBuys;

        public List<long> spinnerClicks;
        public List<long> spinnerCollects;
        public List<long> shopClicks;
        public List<long> leaderboardClicks;
        public List<long> chestClicks;
    }

    // LOCAL TESTING
    //string abboxAdsApi = "http://localhost:5002";
    //string levitationMasterApi = "http://localhost:5001/v1/levitationMaster";

    // STAGING
    //string abboxAdsApi = "https://staging.ads.abbox.com";
    //string levitationMasterApi = "https://staging.api.abboxgames.com/levitationMaster";

    // PRODUCTION
    string abboxAdsApi = "https://ads.abbox.com";
    string levitationMasterApi = "https://api.abboxgames.com/v1/levitationMaster";

    List<LeaderboardItem> top = new List<LeaderboardItem>();
    List<LeaderboardItem> before = new List<LeaderboardItem>();
    List<LeaderboardItem> after = new List<LeaderboardItem>();
    LeaderboardItem you = new LeaderboardItem();

    // To send response to corresponding files
    [SerializeField] MainStatus mainStatus;
    // This is to call the functions in leaderboard scene
    [SerializeField] LeaderboardStatus leaderboardStatus;

    Header header = new Header();

    void Awake()
    {
        header.deviceId = SystemInfo.deviceUniqueIdentifier;
        header.deviceOS = SystemInfo.operatingSystem;
    }

    /* ---------- LOAD SCENE ---------- */

    // CREATE NEW PLAYER
    public void CreatePlayer(Player player)
    {
        string playerUrl = levitationMasterApi + "/player";

        PlayerData playerData = new PlayerData();
        playerData.coins = player.coins;
        playerData.diamonds = player.diamonds;
        playerData.nextLevelIndex = player.nextLevelIndex;
        playerData.currentBallIndex = player.currentBallIndex;

        playerData.allBalls = new List<int>();
        player.allBalls.ForEach(l => { playerData.allBalls.Add(l); });

        playerData.redKeyCount = player.redKeyCount;
        playerData.goldKeyCount = player.goldKeyCount;
        playerData.silverKeyCount = player.silverKeyCount;

        playerData.bulletCount = player.bulletCount;
        playerData.lightningCount = player.lightningCount;
        playerData.shieldCount = player.shieldCount;
        playerData.speedCount = player.speedCount;

        // Clicks
        playerData.redChestBuys = new List<long>();
        player.redChestBuys.ForEach(l => { playerData.redChestBuys.Add(l); });

        playerData.goldChestBuys = new List<long>();
        player.goldChestBuys.ForEach(l => { playerData.goldChestBuys.Add(l); });

        playerData.silverChestBuys = new List<long>();
        player.silverChestBuys.ForEach(l => { playerData.silverChestBuys.Add(l); });

        playerData.spinnerClicks = new List<long>();
        player.spinnerClicks.ForEach(l => { playerData.spinnerClicks.Add(l); });

        playerData.spinnerCollects = new List<long>();
        player.spinnerCollects.ForEach(l => { playerData.spinnerCollects.Add(l); });

        playerData.shopClicks = new List<long>();
        player.shopClicks.ForEach(l => { playerData.shopClicks.Add(l); });

        playerData.leaderboardClicks = new List<long>();
        player.leaderboardClicks.ForEach(l => { playerData.leaderboardClicks.Add(l); });

        playerData.chestClicks = new List<long>();
        player.chestClicks.ForEach(l => { playerData.chestClicks.Add(l); });

        string playerDataJson = JsonUtility.ToJson(playerData);

        StartCoroutine(CreatePlayerCoroutine(playerUrl, playerDataJson));
    }

    // This one is called when the game is just launched
    // Either create a new player or move on
    private IEnumerator CreatePlayerCoroutine(string url, string playerData)
    {
        var jsonBinary = System.Text.Encoding.UTF8.GetBytes(playerData);
        DownloadHandlerBuffer downloadHandlerBuffer = new DownloadHandlerBuffer();
        UploadHandlerRaw uploadHandlerRaw = new UploadHandlerRaw(jsonBinary);
        uploadHandlerRaw.contentType = "application/json";

        UnityWebRequest webRequest =
            new UnityWebRequest(url, "POST", downloadHandlerBuffer, uploadHandlerRaw);

        string message = JsonUtility.ToJson(header);
        string headerMessage = BuildHeaders(message);
        webRequest.SetRequestHeader("token", headerMessage);

        yield return webRequest.SendWebRequest();
        if (webRequest.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(webRequest.downloadHandler.text);
            // Set the error received from creating a player
        }
        else
        {
            Debug.Log(webRequest.downloadHandler.text);
            // Make the success actions received from creating a player
            mainStatus.CreatePlayerSuccess();
        }
    }

    /* ---------- MAIN SCENE ---------- */

    // SAVE PLAYER DATA
    public void SavePlayerData(Player player)
    {
        string playerDataUrl = levitationMasterApi + "/data";

        PlayerData playerData = new PlayerData();
        playerData.coins = player.coins;
        playerData.diamonds = player.diamonds;
        playerData.nextLevelIndex = player.nextLevelIndex;
        playerData.currentBallIndex = player.currentBallIndex;

        playerData.allBalls = new List<int>();
        player.allBalls.ForEach(l => { playerData.allBalls.Add(l); });

        playerData.redKeyCount = player.redKeyCount;
        playerData.goldKeyCount = player.goldKeyCount;
        playerData.silverKeyCount = player.silverKeyCount;

        playerData.bulletCount = player.bulletCount;
        playerData.lightningCount = player.lightningCount;
        playerData.shieldCount = player.shieldCount;
        playerData.speedCount = player.speedCount;

        // Clicks
        playerData.redChestBuys = new List<long>();
        player.redChestBuys.ForEach(l => { playerData.redChestBuys.Add(l); });

        playerData.goldChestBuys = new List<long>();
        player.goldChestBuys.ForEach(l => { playerData.goldChestBuys.Add(l); });

        playerData.silverChestBuys = new List<long>();
        player.silverChestBuys.ForEach(l => { playerData.silverChestBuys.Add(l); });

        playerData.spinnerClicks = new List<long>();
        player.spinnerClicks.ForEach(l => { playerData.spinnerClicks.Add(l); });

        playerData.spinnerCollects = new List<long>();
        player.spinnerCollects.ForEach(l => { playerData.spinnerCollects.Add(l); });

        playerData.shopClicks = new List<long>();
        player.shopClicks.ForEach(l => { playerData.shopClicks.Add(l); });

        playerData.leaderboardClicks = new List<long>();
        player.leaderboardClicks.ForEach(l => { playerData.leaderboardClicks.Add(l); });

        playerData.chestClicks = new List<long>();
        player.chestClicks.ForEach(l => { playerData.chestClicks.Add(l); });

        string playerDataJson = JsonUtility.ToJson(playerData);

        StartCoroutine(SavePlayerDataCoroutine(playerDataUrl, playerDataJson));
    }

    private IEnumerator SavePlayerDataCoroutine(string url, string playerData)
    {
        var jsonBinary = System.Text.Encoding.UTF8.GetBytes(playerData);
        DownloadHandlerBuffer downloadHandlerBuffer = new DownloadHandlerBuffer();
        UploadHandlerRaw uploadHandlerRaw = new UploadHandlerRaw(jsonBinary);
        uploadHandlerRaw.contentType = "application/json";

        UnityWebRequest webRequest =
            new UnityWebRequest(url, "POST", downloadHandlerBuffer, uploadHandlerRaw);

        string message = JsonUtility.ToJson(header);
        string headerMessage = BuildHeaders(message);
        webRequest.SetRequestHeader("token", headerMessage);

        yield return webRequest.SendWebRequest();
        //if (webRequest.result == UnityWebRequest.Result.ConnectionError)
        //{
        //    Debug.Log(webRequest.downloadHandler.text);
        //    // Set the error received from creating a player
        //}
        //else
        //{
        //    Debug.Log(webRequest.downloadHandler.text);
        //    // Make the success actions received from creating a player
        //}
    }

    public void SendVideoClick(bool privacy, string videoId, string link)
    {
        string videoUrl = abboxAdsApi + "/api/v1/clicks/" + videoId;
        StartCoroutine(SendClickCoroutine(videoUrl, privacy, link));
    }

    private IEnumerator SendClickCoroutine(string url, bool privacy, string link)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            string message = JsonUtility.ToJson(header);
            string headerMessage = BuildHeaders(message);

            if (privacy)
            {
                webRequest.SetRequestHeader("token", headerMessage);
            }
            webRequest.SetRequestHeader("link", link);

            // Send request and wait for the desired response.
            yield return webRequest.SendWebRequest();
        }
    }

    public void GetVideoLink()
    {
        string videoUrl = abboxAdsApi + "/api/v1/videos";
        StartCoroutine(GetAdLinkCoroutine(videoUrl));
    }

    // This one is for TV in main scene
    // Get the latest video link, for now in general, in future personal based on the DeviceId
    private IEnumerator GetAdLinkCoroutine(string url)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            string message = JsonUtility.ToJson(header);
            string headerMessage = BuildHeaders(message);
            webRequest.SetRequestHeader("token", headerMessage);

            // Send request and wait for the desired response.
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(webRequest.downloadHandler.text);
                // Set the error of video link received from the server
                //mainStatus.SetVideoLinkError(webRequest.error);
            }
            else
            {
                Debug.Log(webRequest.downloadHandler.text);
                // Parse the response from server to retrieve all data fields
                VideoJson videoInfo = JsonUtility.FromJson<VideoJson>(webRequest.downloadHandler.text);

                // Set the video link received from the server
                mainStatus.SetVideoLinkSuccess(videoInfo);
            }
        }
    }

    /* ---------- LEADERBOARD SCENE ---------- */

    // CHANGE PLAYER NAME
    public void ChangePlayerName(string playerName)
    {
        string nameUrl = levitationMasterApi + "/name";

        PlayerName nameObject = new PlayerName();
        nameObject.playerName = playerName;

        string nameJson = JsonUtility.ToJson(nameObject);

        StartCoroutine(ChangeNameCoroutine(nameUrl, nameJson));
    }

    // CHANGE PLAYER NAME
    private IEnumerator ChangeNameCoroutine(string url, string playerName)
    {
        var jsonBinary = System.Text.Encoding.UTF8.GetBytes(playerName);
        DownloadHandlerBuffer downloadHandlerBuffer = new DownloadHandlerBuffer();
        UploadHandlerRaw uploadHandlerRaw = new UploadHandlerRaw(jsonBinary);
        uploadHandlerRaw.contentType = "application/json";

        UnityWebRequest webRequest =
            new UnityWebRequest(url, "POST", downloadHandlerBuffer, uploadHandlerRaw);

        string message = JsonUtility.ToJson(header);
        string headerMessage = BuildHeaders(message);
        webRequest.SetRequestHeader("token", headerMessage);

        yield return webRequest.SendWebRequest();

        if (webRequest.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(webRequest.downloadHandler.text);
            // Set the error received from creating a player
            leaderboardStatus.ChangeNameError();
        }
        else
        {
            Debug.Log(webRequest.downloadHandler.text);
            // Make the success actions received from creating a player
            leaderboardStatus.ChangeNameSuccess();
        }
    }


    // GET LEADERBOARD LIST
    public void GetLeaderboard()
    {
        string leaderboardUrl = levitationMasterApi + "/leaderboard";
        StartCoroutine(LeaderboardCoroutine(leaderboardUrl));
    }

    // Get leaderboard data and populate it into the scroll list
    private IEnumerator LeaderboardCoroutine(string url)
    {
        Debug.Log(url);
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            string message = JsonUtility.ToJson(header);
            string headerMessage = BuildHeaders(message);
            webRequest.SetRequestHeader("token", headerMessage);

            // Send request and wait for the desired response.
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                // Set the error of leaderboard data received from the server
                Debug.Log(webRequest.downloadHandler.text);
            }
            else
            {
                Debug.Log(webRequest.downloadHandler.text);
                // Parse the response from server to retrieve all data fields
                PopulateLeaderboardData(webRequest.downloadHandler.text);
            }
        }
    }

    private void PopulateLeaderboardData(string jsonData)
    {
        // Clear the lists incase they already had data in them
        top.Clear();
        before.Clear();
        after.Clear();
        // Extract string arrays of top, before, after and stirng of you data
        string[] topData = JsonHelper.GetJsonObjectArray(jsonData, "top");
        string[] beforeData = JsonHelper.GetJsonObjectArray(jsonData, "before");
        string youData = JsonHelper.GetJsonObject(jsonData, "you");
        string[] afterData = JsonHelper.GetJsonObjectArray(jsonData, "after");

        if (topData != null)
        {
            // Parse top data to leaderboard item to populate the list
            for (int i = 0; i < topData.Length; i++)
            {
                LeaderboardItem item = JsonUtility.FromJson<LeaderboardItem>(topData[i]);
                top.Add(item);
            }
        }

        if (beforeData != null)
        {
            // Parse before data
            for (int i = 0; i < beforeData.Length; i++)
            {
                LeaderboardItem item = JsonUtility.FromJson<LeaderboardItem>(beforeData[i]);
                before.Add(item);
            }
        }

        // Parse you data
        you = JsonUtility.FromJson<LeaderboardItem>(youData);

        if (afterData != null)
        {
            // Parse after data
            for (int i = 0; i < afterData.Length; i++)
            {
                LeaderboardItem item = JsonUtility.FromJson<LeaderboardItem>(afterData[i]);
                after.Add(item);
            }
        }

        // Send leaderboard data to leaderboard scene
        leaderboardStatus.SetLeaderboardData(top, before, you, after);
    }
}

