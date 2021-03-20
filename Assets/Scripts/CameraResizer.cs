using UnityEngine;
using UnityEngine.UI;

public class CameraResizer : MonoBehaviour
{
    [SerializeField] GameObject canvas;

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
}
