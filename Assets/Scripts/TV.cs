using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class TV : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    [SerializeField] GameObject lights;

    void Awake()
    {
        videoPlayer = transform.Find("VideoPlayer").GetComponent<VideoPlayer>();
    }

    public void SetAdLink(string url)
    {
        SetLightsGreen();
        videoPlayer.url = url;
        videoPlayer.Play();
    }

    public void SetAdButton(string url)
    {
        GetComponent<Button>().onClick.AddListener(() => Application.OpenURL(url));
    }

    public void SetLightsRed()
    {
        lights.GetComponent<Image>().color = new Color32(255, 0, 0, 255);
    }

    private void SetLightsGreen()
    {
        lights.GetComponent<Image>().color = new Color32(0, 255, 0, 255);
    }
}