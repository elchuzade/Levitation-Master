using UnityEngine;

public class Barrier : MonoBehaviour
{
    [SerializeField] GameObject barrierDestroyParticles;
    [SerializeField] GameObject components;
    [SerializeField] BoxCollider col;

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
        barrierDestroyParticles.SetActive(true);
        components.SetActive(false);
        col.enabled = false;

        Destroy(gameObject, 1);
    }
}
