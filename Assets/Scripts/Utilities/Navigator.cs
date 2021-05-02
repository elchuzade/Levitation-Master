using UnityEngine;
using UnityEngine.SceneManagement;

public class Navigator : MonoBehaviour
{
    void Awake()
    {
        transform.SetParent(transform.parent.parent);
        // Singleton
        int instances = FindObjectsOfType<Navigator>().Length;
        if (instances > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void LoadShop()
    {
        SceneManager.LoadScene("ShopScene");
    }

    public void LoadLeaderboard()
    {
        SceneManager.LoadScene("LeaderboardScene");
    }

    public void LoadChests()
    {
        SceneManager.LoadScene("ChestsScene");
    }

    public void LoadNextLevel(int nextLevelIndex)
    {
        SceneManager.LoadScene("Level-" + nextLevelIndex);
    }

    public void LoadLeaderboardScene()
    {
        SceneManager.LoadScene("LeaderboardScene");
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }
}
