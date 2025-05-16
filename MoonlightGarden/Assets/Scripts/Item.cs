using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] public ItemData_SO itemData;
    [SerializeField] public Collider2D collider2D;

    public string ItemName => itemData.itemName;
    public string ItemDescription => itemData.itemDescription;
    public int CurrentAmount { get; set; } = 1; 
    public int MaxAmount => itemData.maxAmount;
    public GameObject prefabs => itemData.prefabs;
    public Sprite ItemImage => itemData.itemImage;

    private void Start()
    {
        CurrentAmount = Mathf.Clamp(CurrentAmount, 0, MaxAmount);
    }

    public virtual void UseItem()
    {
    }
}