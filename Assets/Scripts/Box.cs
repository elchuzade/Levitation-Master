using UnityEngine;
using static GlobalVariables;

public class Box : MonoBehaviour
{
    [SerializeField] Boxes box;
    [SerializeField] GameObject collectParticles;
    [SerializeField] GameObject components;
    [SerializeField] BoxCollider col;

    [Header ("For Diamonds and Coins")]
    [SerializeField] int minDropCount;
    [SerializeField] int maxDropCount;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ball")
        {
            LevelStatus levelStatus = FindObjectOfType<LevelStatus>();

            int amount = Random.Range(minDropCount, maxDropCount + 1);
            levelStatus.OpenBox(box, amount, transform.position);

            AttempDestroyBox();
        }
    }

    void AttempDestroyBox()
    {
        col.enabled = false;
        collectParticles.SetActive(true);
        components.SetActive(false);

        Destroy(gameObject, 1);
    }
}
