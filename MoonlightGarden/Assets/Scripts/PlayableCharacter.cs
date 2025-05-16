using UnityEngine;

[CreateAssetMenu (fileName = "PlayableCharacter",menuName = "Character")]
public class PlayableCharacter : ScriptableObject
{
    public string characterName = "character_Name";
    public int skillPoint = 0;
    public int currentMaxSkillPoint = 0;
    [Header ("Combat")]
    public int totalCombat_1_allocated = 0;
    public int totalCombat_1_Maximum = 20;
    public int totalCombat_2_allocated = 0;
    public int totalCombat_2_Maximum = 20;
    public int totalCombat_3_allocated = 0;
    public int totalCombat_3_Maximum = 20;
    [Header("ResourceGathering")]
    public int totalResourceGathering_1_allocated = 0;
    public int totalResourceGathering_1_Maximum = 20;
    public int totalResourceGathering_2_allocated = 0;
    public int totalResourceGathering_2_Maximum = 20;
    public int totalResourceGathering_3_allocated = 0;
    public int totalResourceGathering_3_Maximum = 20;
    [Header("Exploration")]
    public int totalExploration_1_allocated = 0;
    public int totalExploration_1_Maximum = 20;
    public int totalExploration_2_allocated = 0;
    public int totalExploration_2_Maximum = 20;
    public int totalExploration_3_allocated = 0;
    public int totalExploration_3_Maximum = 20;
    [Header("LifeCycle")]
    public int totalLifeCycle_1_allocated = 0;
    public int totalLifeCycle_1_Maximum = 20;
    public int totalLifeCycle_2_allocated = 0;
    public int totalLifeCycle_2_Maximum = 20;
    public int totalLifeCycle_3_allocated = 0;
    public int totalLifeCycle_3_Maximum = 20;

    public Sprite characterImage;
    public Sprite characterPortrait;

    public int healthPoint = 100;
    public int attack = 10;
    public int defense = 0;
    public int moveSpeed = 5;
    public float scoreMultiplier = 100;
    public float ScoreFlatAdd;

    public bool isUnlokced = false;
}
