using UnityEngine;
using static GlobalVariables;

public class Box : MonoBehaviour
{
    [SerializeField] Boxes box;

    [Header ("For Diamonds and Coins")]
    [SerializeField] int minDropCount;
    [SerializeField] int maxDropCount;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ball")
        {
            LevelStatus levelStatus = FindObjectOfType<LevelStatus>();

            int amount = Random.Range(minDropCount, maxDropCount + 1);
            levelStatus.OpenBox(box, amount);

            Destroy(transform.parent.gameObject);
        }
    }
}
