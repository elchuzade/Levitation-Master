using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class TV : MonoBehaviour
{
    private VideoPlayer videoPlayer;

    void Awake()
    {
        videoPlayer = transform.Find("VideoPlayer").GetComponent<VideoPlayer>();
    }

    // @access from Server script
    public void SetAdLink(string url)
    {
        videoPlayer.url = url;
        videoPlayer.Play();
    }

    // @access from Server script
    public void SetAdButton(string url)
    {
        GetComponent<Button>().onClick.AddListener(() => Application.OpenURL(url));
    }
}
