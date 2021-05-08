using UnityEngine;

[CreateAssetMenu(fileName = "New Spinner Item", menuName = "Spinner Item")]
public class Item : ScriptableObject
{
    public GameObject itemPrefab;
    [Header("* Must Be Unique")]
    public string itemName;
    public int chanceCount;
}
