using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CameraResizer : MonoBehaviour
{
    [SerializeField] GameObject canvas;
    [SerializeField] GameObject ball;

    Vector3 cameraInitPosition = new Vector3(375, 1380, -125);
    Vector3 cameraInitRotation = new Vector3(50, 0, 0);

    [SerializeField] bool shopScene;

    bool levelComplete;
    bool levelStarted;
    float cameraRotateSpeed = 1;

    int cameraCloseDistance = 45;
    int cameraFarDistance = 80;

    void Start()
    {
        //Camera.main.orthographicSize = Screen.height / 2;
        //transform.position = new Vector3((float)Screen.width / 2, (float)Screen.height / 2, -10);

        //Change the camera zoom based on the screen ratio for very tall or very wide screens
        if ((float)Screen.height / Screen.width >= 2)
        {
            // In the level scene the camera will zoom out more, so that all disc is visible
            cameraFarDistance = 90;
            // Tall phone
            Camera.main.orthographicSize = 800;
        }
        else
        {
            // In the level scene the camera will zoom out less, so that all disc fits perfectly the screen width
            cameraFarDistance = 80;
            // Wide phone (+tablet)
            if (shopScene)
            {
                Camera.main.orthographicSize = Screen.width / 2;
            } else
            {
                Camera.main.orthographicSize = 667;
            }
        }

        // Tablet screens
        if ((float)Screen.width / Screen.height > 0.6)
        {
            canvas.GetComponent<CanvasScaler>().matchWidthOrHeight = 1;
        } else
        {
            // Phone screens
            canvas.GetComponent<CanvasScaler>().matchWidthOrHeight = 0;
        }

        // Place a camera in the center of the shop scene so the ball is rendered correctly
        if (shopScene)
        {
            transform.position = new Vector3(Screen.width / 2, Screen.height / 2 - 100, -600);
        }
    }

    void Update()
    {
        if (levelComplete && Camera.main.fieldOfView > cameraCloseDistance)
        {
            // Zoom in the camera from 80 to 45 degrees so the ball is close enough
            Camera.main.fieldOfView--;
        } else if (levelStarted && Camera.main.fieldOfView < cameraFarDistance)
        {
            // Zoom out the camera from 45 to 102 degrees so the ball is far enough
            Camera.main.fieldOfView++;
        }
        if (levelStarted)
        {
            Camera.main.transform.position = Vector3.Lerp(transform.position, cameraInitPosition, Time.deltaTime);

            Camera.main.transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(cameraInitRotation), Time.deltaTime * cameraRotateSpeed);
            cameraRotateSpeed += 0.05f;
        }
        else if (levelComplete)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(ball.transform.position.x, ball.transform.position.y, ball.transform.position.z - 725), Time.deltaTime);
            Camera.main.transform.LookAt(ball.transform);
        }
    }

    IEnumerator LinkCameraToBall()
    {
        yield return new WaitForSeconds(1);
        transform.SetParent(ball.transform);
    }

    // @access from LevelStatus script
    public void CameraFollowBall()
    {
        // Set the camera focus to the ball
        levelComplete = true;
        levelStarted = false;
        StartCoroutine(LinkCameraToBall());
    }

    // @access from LevelStatus script
    public void CameraUnfollowBall()
    {
        // Set the camera focus its initial position and rotation
        levelComplete = false;
        levelStarted = true;
        transform.SetParent(transform.parent.parent);
    }
}
