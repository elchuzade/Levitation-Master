using UnityEngine;
using static GlobalVariables;

public class Box : MonoBehaviour
{
    [SerializeField] Boxes box;
    [SerializeField] GameObject collectParticles;
    [SerializeField] GameObject components;
    [SerializeField] BoxCollider col;

    [Header ("Drop count for Coins")]
    [SerializeField] int minDropCountCoins;
    [SerializeField] int maxDropCountCoins;

    [Header("Drop count for Diamonds")]
    [SerializeField] int minDropCountDiamond;
    [SerializeField] int maxDropCountDiamond;

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
