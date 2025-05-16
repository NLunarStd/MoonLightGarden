using UnityEngine;

public class InventorySlot
{
    public Item currentItem;
    public int currentAmount;

    public InventorySlot()
    {
        currentItem = null;
        currentAmount = 0;
    }
}