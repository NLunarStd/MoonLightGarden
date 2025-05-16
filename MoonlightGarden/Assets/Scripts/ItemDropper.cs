using UnityEngine;

public class ItemDropper : MonoBehaviour
{
    public Item itemToDrop;
    public int amountToDrop = 1;
    public int dropChance = 50; 

    public GameObject spawnDropEffect;

    public void TryDropItem()
    {
        if (Random.Range(0, 100) < dropChance)
        {
            DropItem(itemToDrop, amountToDrop, transform.position); 
            SpawnDropEffect(transform.position); 
        }
    }

    void DropItem(Item item, int amount, Vector2 dropLocation)
    {
        if (item != null && item.prefabs != null)
        {
            for (int i = 0; i < amount; i++)
            {
                Instantiate(item.prefabs, dropLocation, Quaternion.identity);
                Debug.Log("Drop Item");
            }
        }
    }

    void SpawnDropEffect(Vector2 spawnLocation)
    {
        if (spawnDropEffect != null)
        {
            Instantiate(spawnDropEffect, spawnLocation, Quaternion.identity);
        }
    }
}