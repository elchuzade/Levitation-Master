using System.Collections;
using UnityEngine;

public class ChestPlatform : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
        if (other.gameObject.tag != "Ball")
        {
            StartCoroutine(MoveUp(other.gameObject));
        }
    }

    IEnumerator MoveUp(GameObject coin)
    {
        yield return new WaitForSeconds(1);

        coin.GetComponent<ChestDropItem>().MoveUp();
    }
}
