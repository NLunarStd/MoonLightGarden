using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager instance;

    public Dictionary<ResourceType, int> resourceAmounts = new Dictionary<ResourceType, int>();
    public Dictionary<ResourceType, int> resourceMaxAmounts = new Dictionary<ResourceType, int>();
    public Dictionary<JewelType, bool> jewelStates = new Dictionary<JewelType, bool>();

    public enum ResourceType
    {
        MonsterCorpse_A,
        MonsterCorpse_B,
        MonsterCorpse_C,
        MonsterCorpse_D
    }

    public enum JewelType
    {
        First,
        Second,
        Third
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            InitializeResources();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeResources()
    {
        resourceAmounts[ResourceType.MonsterCorpse_A] = 0;
        resourceMaxAmounts[ResourceType.MonsterCorpse_A] = 99;
        resourceAmounts[ResourceType.MonsterCorpse_B] = 0;
        resourceMaxAmounts[ResourceType.MonsterCorpse_B] = 99;
        resourceAmounts[ResourceType.MonsterCorpse_C] = 0;
        resourceMaxAmounts[ResourceType.MonsterCorpse_C] = 99;
        resourceAmounts[ResourceType.MonsterCorpse_D] = 0;
        resourceMaxAmounts[ResourceType.MonsterCorpse_D] = 99;

        jewelStates[JewelType.First] = false;
        jewelStates[JewelType.Second] = false;
        jewelStates[JewelType.Third] = false;
    }

    public void UpdateResource(ResourceType resourceType, int amount)
    {
        if (resourceAmounts.ContainsKey(resourceType) && resourceMaxAmounts.ContainsKey(resourceType))
        {
            resourceAmounts[resourceType] = Mathf.Clamp(resourceAmounts[resourceType] + amount, 0, resourceMaxAmounts[resourceType]);
        }
        else
        {
            Debug.LogError("Resource type not found: " + resourceType);
        }
    }

    public void UpdateJewelState(JewelType jewelType, bool status)
    {
        if (jewelStates.ContainsKey(jewelType))
        {
            jewelStates[jewelType] = status;
        }
        else
        {
            Debug.LogError("Jewel type not found: " + jewelType);
        }
    }

    public bool CheckJewelState(JewelType type)
    {
        if (jewelStates.ContainsKey(type))
        {
            return jewelStates[type];
        }
        else
        {
            Debug.LogError("Jewel type not found: " + type);
            return false;
        }
    }

    public int GetResourceAmount(ResourceType type)
    {
        if (resourceAmounts.ContainsKey(type))
        {
            return resourceAmounts[type];
        }
        else
        {
            Debug.LogError("Resource Type not found: " + type);
            return 0;
        }
    }
}