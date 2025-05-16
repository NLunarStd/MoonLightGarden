using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Item Data")]
public class ItemData_SO : ScriptableObject
{
    public string itemName;
    public string itemDescription;
    public int maxAmount = 1;
    public Sprite itemImage;
    public GameObject prefabs;
}