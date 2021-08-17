using UnityEngine;

public class Bullet : MonoBehaviour
{
    DirectionArrow directionArrow;
    int speed = 3;

    Vector3 direction;

    void Awake()
    {
        directionArrow = FindObjectOfType<DirectionArrow>();
    }

    void Start()
    {
        direction = directionArrow.direction;

        if (direction == Vector3.zero)
        {
            direction = Vector3.up;
        }
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(90, 0, angle - 90);
        Destroy(gameObject, 4);
    }

    void Update()
    {
        transform.localPosition += new Vector3(direction.normalized.x, 0, direction.normalized.y) * speed;
    }
}
