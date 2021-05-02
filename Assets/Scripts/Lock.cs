using UnityEngine;

public class Lock : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    // @access from ChestWindow canvas
    public void OpenLock()
    {
        transform.parent.GetComponent<ChestUnlock>().OpenLock();
    }
}
