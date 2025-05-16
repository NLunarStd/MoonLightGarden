using UnityEngine;

[CreateAssetMenu(fileName = "CraftingRecipe", menuName = "Crafting Recipe")]
public class CraftingRecipe : ScriptableObject
{
    public ResourceManager.ResourceType[] requiredResources;
    public int[] requiredAmounts;
    public GameObject resultMonsterPrefab;
    public GameObject resultBuildingItemPrefab;
}