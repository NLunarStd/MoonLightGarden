using UnityEngine;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    public List<InventorySlot> inventorySlots = new List<InventorySlot>();
    public int inventorySlotAmount = 10;

    private void Start()
    {
        SetUpInventory();
    }

    void SetUpInventory()
    {
        for (int i = 0; i < inventorySlotAmount; i++)
        {
            var slot = new InventorySlot();
            inventorySlots.Add(slot);
        }
    }

    public bool AddItem(Item item, int amount)
    {
        
        InventorySlot existingSlot = inventorySlots.Find(slot => slot.currentItem == item && slot.currentAmount < item.MaxAmount);

        if (existingSlot != null)
        {
            
            int spaceAvailable = item.itemData.maxAmount - existingSlot.currentAmount;
            int amountToAdd = Mathf.Min(amount, spaceAvailable);
            existingSlot.currentAmount += amountToAdd;
            amount -= amountToAdd; 

            if (amount <= 0)
            {
                UIController.instance?.UpdateInventoryUI();
                return true;
            }
        }

        
        InventorySlot emptySlot = inventorySlots.Find(slot => slot.currentItem == null);

        if (emptySlot != null)
        {
            emptySlot.currentItem = item;
            emptySlot.currentAmount = amount;
            emptySlot.currentItem.itemData.itemImage = item.itemData.itemImage; // ใช้ itemImage จาก ScriptableObject
            UIController.instance?.UpdateInventoryUI();
            return true;
        }
        else
        {
            Debug.Log("Inventory is full.");
            return false;
        }
    }

    public bool RemoveItem(Item item)
    {
        InventorySlot foundSlot = inventorySlots.Find(slot => slot.currentItem == item); 

        if (foundSlot != null)
        {
            foundSlot.currentItem = null;
            foundSlot.currentAmount = 0;
            UIController.instance?.UpdateInventoryUI(); 
            return true;
        }
        else
        {
            Debug.Log("Item not found in inventory.");
            return false;
        }
    }
}