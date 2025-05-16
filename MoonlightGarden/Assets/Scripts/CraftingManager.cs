using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    public static CraftingManager instance;
    public List<CraftingRecipe> craftingRecipes;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool CanCraft(CraftingRecipe recipe)
    {
        for (int i = 0; i < recipe.requiredResources.Length; i++)
        {
            if (ResourceManager.instance.GetResourceAmount(recipe.requiredResources[i]) < recipe.requiredAmounts[i])
            {
                return false;
            }
        }
        return true;
    }

    public void Craft(CraftingRecipe recipe, Vector3 spawnPosition)
    {
        if (CanCraft(recipe))
        {
            for (int i = 0; i < recipe.requiredResources.Length; i++)
            {
                ResourceManager.instance.UpdateResource(recipe.requiredResources[i], -recipe.requiredAmounts[i]);
            }

            if (recipe.resultMonsterPrefab != null)
            {
                Instantiate(recipe.resultMonsterPrefab, spawnPosition, Quaternion.identity);
            }
            else if (recipe.resultBuildingItemPrefab != null)
            {
                Instantiate(recipe.resultBuildingItemPrefab, spawnPosition, Quaternion.identity);
            }
            else
            {
                
            }
        }
        else
        {
            Debug.Log("Not enough resource!");
        }
    }
}