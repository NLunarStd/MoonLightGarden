using UnityEngine;

public class Structure : MonoBehaviour
{
    public StructureData structureData;

    private int currentHealth;

    protected virtual void Start()
    {
        currentHealth = structureData.health;
        BuildStructure();
    }

    protected virtual void Update()
    {
    }

    protected virtual void BuildStructure()
    {
        Debug.Log(structureData.structureName + " is being built.");
        Invoke("OnStructureBuilt", structureData.buildTime);
    }

    protected virtual void OnStructureBuilt()
    {
        Debug.Log(structureData.structureName + " has been built.");
    }

    public virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            DestroyStructure();
        }
    }

    public virtual void RepairStructure(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, structureData.maxHealth);
    }

    protected virtual void DestroyStructure()
    {
        Debug.Log(structureData.structureName + " has been destroyed.");
        Destroy(gameObject);
    }
}