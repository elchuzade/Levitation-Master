using UnityEngine;
using UnityEngine.UI;

public class HorizontalScroll : MonoBehaviour
{
    [SerializeField] Scrollbar scrollbar;

    void Start()
    {
        scrollbar.value = 0;
    }
}
