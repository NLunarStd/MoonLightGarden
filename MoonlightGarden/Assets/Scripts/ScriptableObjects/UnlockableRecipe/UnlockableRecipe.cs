using NUnit.Framework;
using UnityEngine;


[CreateAssetMenu(fileName = "UnlockableRecipe", menuName = "Unlockable Recipe")]
public class UnlockableRecipe : ScriptableObject
{
    public string name = "";
    public bool isUnklocked = false;
    public bool isRepeatUnlockable = false;
    public int stacked = 0;
    public int maxStacked = 1;
    public int itemTier = 1;
    public RecipeType type = RecipeType.item;

    public UnlockableRecipe priorUnlokcedNeed;
    public bool isDefaultUnlocked = false;
}
public enum RecipeType
{
    item, feature, character, monster
}
