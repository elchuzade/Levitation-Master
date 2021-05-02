using UnityEngine;

public class Barrier : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Bullet")
        {
            if (gameObject.tag == "Barrier")
            {
                Destroy(other.gameObject);
                AttemptDestroyProcess();
            } else
            {
                Destroy(other.gameObject);
            }
        }
    }

    public void AttemptDestroyProcess()
    {
        // Add barrier destroy particles
        Destroy(gameObject);
    }
}
