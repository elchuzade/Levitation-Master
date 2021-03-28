using UnityEngine;
using UnityEngine.UI;

public class CameraResizer : MonoBehaviour
{
    [SerializeField] GameObject canvas;

    bool levelComplete;

    void Start()
    {
        //Camera.main.orthographicSize = Screen.height / 2;
        //transform.position = new Vector3((float)Screen.width / 2, (float)Screen.height / 2, -10);

        //Change the camera zoom based on the screen ratio, for very tall or very wide screens
        if ((float)Screen.height / Screen.width > 2)
        {
            Camera.main.orthographicSize = 800;
        }
        else
        {
            Camera.main.orthographicSize = 667;
        }

        if ((float)Screen.width / Screen.height > 0.7)
        {
            canvas.GetComponent<CanvasScaler>().matchWidthOrHeight = 1;
        }
    }

    void Update()
    {
        if (levelComplete && Camera.main.fieldOfView > 45)
        {
            // Zoom in the camera from 102 to 45 degrees so the ball is close enough
            Camera.main.fieldOfView--;
        }
    }

    public void CameraFollowBall()
    {
        GameObject ball = FindObjectOfType<Ball>().gameObject;

        // Set the camera focus to the ball
        Camera.main.transform.LookAt(ball.transform);
        levelComplete = true;
        transform.SetParent(ball.transform);
    }
}
