using UnityEngine;

[CreateAssetMenu(fileName = "New Structure Data", menuName = "Structure Data", order = 51)]
public class StructureData : ScriptableObject
{
    public string structureName = "Structure";
    public int health = 100;
    public int maxHealth = 100;
    public GameObject visualPrefab;
    public float buildTime = 5f;
    public float repairTime = 3f;
}