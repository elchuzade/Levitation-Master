using UnityEngine;
using static GlobalVariables;

public class Rotate : MonoBehaviour
{
    [SerializeField] float rotateSpeed;
    [SerializeField] RotateDirection direction;

    int angularDirection;

    void Start()
    {
        if (direction == RotateDirection.Clockwise)
        {
            angularDirection = 1;
        } else
        {
            angularDirection = -1;
        }
    }

    void Update()
    {
        transform.Rotate(Vector3.up, rotateSpeed * angularDirection * Time.deltaTime);
    }
}
