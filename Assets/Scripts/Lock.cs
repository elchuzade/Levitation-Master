using UnityEngine;

public class Lock : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OpenLock()
    {
        transform.parent.GetComponent<ChestUnlock>().OpenLock();
    }
}
