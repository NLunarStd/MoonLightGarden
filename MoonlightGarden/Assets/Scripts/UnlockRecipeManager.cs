using UnityEngine;
using System.Collections.Generic;
using TMPro;
public class UnlockRecipeManager : MonoBehaviour
{
    public List<UnlockableRecipe> recipeList = new List<UnlockableRecipe>();
    private PlayerData currentPlayerData;
    Dictionary<string, UnlockableRecipe> allRecipesByName = new Dictionary<string, UnlockableRecipe>();


    public LobbyUIController lobbyUIController;
    private void Awake()
    {
        
        foreach (var recipe in recipeList)
        {
            if (!allRecipesByName.ContainsKey(recipe.name))
            {
                allRecipesByName.Add(recipe.name, recipe);
            }
            else
            {
                Debug.LogWarning($"Duplicate recipe name found: {recipe.name}. Only the first instance will be used for lookup.");
            }
        }
    }
    public void UnlockRecipe(UnlockableRecipe recipe)
    {
        DisplayUnlockRecipe(recipe);
        recipe.isUnklocked = true;
        if(recipe.isRepeatUnlockable)
        {
            recipe.stacked += 1;
        }
        SetUpUnlockablePool();
        SaveRecipeProgress();
        CurrencyManager.instance.UpdateMoonlightShard(-30);
        lobbyUIController.UpdateCurrency();
        UpdateUnlockedRecipesUI();
    }
    public Transform recipeUnlockMessage;
    public TextMeshProUGUI recipeUnlockText;
    public void DisplayUnlockRecipe(UnlockableRecipe recipe)
    {
        recipeUnlockText.text = $"{recipe.name} is unlocked!";
        recipeUnlockMessage.gameObject.SetActive(true);
        Invoke("HideUnlockMessage", 3f);
    }
    private void HideUnlockMessage()
    {
         recipeUnlockMessage.gameObject.SetActive(false);
    }
    public void UnlockWithUIButton()
    {
        UnlockableRecipe recipeToUnlock = TryUnlockRecipe();

        if (recipeToUnlock != null)
        {
            UnlockRecipe(recipeToUnlock); 
        }
        else
        {
            Debug.LogWarning("No unlockable recipes available!");
            recipeUnlockText.text = "No new recipes available to unlock!";
            recipeUnlockMessage.gameObject.SetActive(true);
            Invoke("HideUnlockMessage", 3f);
        }
    }
    public UnlockableRecipe TryUnlockRecipe()
    {
        List<UnlockableRecipe> tier1 = new List<UnlockableRecipe>();
        List<UnlockableRecipe> tier2 = new List<UnlockableRecipe>();
        List<UnlockableRecipe> tier3 = new List<UnlockableRecipe>();
        List<UnlockableRecipe> tier4 = new List<UnlockableRecipe>();
        foreach (var recipe in toUnlockableRecipeList)
        {
            switch (recipe.itemTier) 
            {
                case 1:
                    tier1.Add(recipe);
                    break;
                case 2:
                    tier2.Add(recipe);
                    break;
                case 3:
                    tier3.Add(recipe);
                    break;
                case 4:
                    tier4.Add(recipe);
                    break;
            }
        }
        List<(List<UnlockableRecipe> tierList, int originalWeight)> availableTiers = new List<(List<UnlockableRecipe> tierList, int originalWeight)>();

        if (tier1.Count > 0) availableTiers.Add((tier1, 50));
        if (tier2.Count > 0) availableTiers.Add((tier2, 30));
        if (tier3.Count > 0) availableTiers.Add((tier3, 15));
        if (tier4.Count > 0) availableTiers.Add((tier4, 5));

        if (availableTiers.Count == 0)
        {
            return null;
        }

        // Calculate the total weight
        int totalWeight = 0;
        foreach (var tierInfo in availableTiers)
        {
            totalWeight += tierInfo.originalWeight;
        }
        if (totalWeight == 0) 
        {
            return null;
        }

        // Generate a random value within the total weight
        int randomWeightValue = Random.Range(0, totalWeight);
        int currentWeightSum = 0;

        // Select a tier based on the random weight value
        foreach (var (tierList, originalWeight) in availableTiers)
        {
            currentWeightSum += originalWeight;
            if (randomWeightValue < currentWeightSum)
            {
                
                int randomIndex = Random.Range(0, tierList.Count);
                return tierList[randomIndex];
            }
        }

        return null;
    }


    public List<UnlockableRecipe> toUnlockableRecipeList = new List<UnlockableRecipe>();
    public List<UnlockableRecipe> alreadyUnlockedRecipeList = new List<UnlockableRecipe>();
    public void SetUpUnlockablePool()
    {
        toUnlockableRecipeList.Clear(); 
        alreadyUnlockedRecipeList.Clear(); 

        foreach (UnlockableRecipe recipe in recipeList)
        {
            SortToRecipeList(recipe);
        }
        UpdateUnlockedRecipesUI();
    }
    public void SortToRecipeList(UnlockableRecipe recipe)
    {
        if (!recipe.isUnklocked)
        {
            toUnlockableRecipeList.Add(recipe);
        }
        else
        {
            if (recipe.isRepeatUnlockable && recipe.stacked < recipe.maxStacked)
            {
                toUnlockableRecipeList.Add(recipe);
            }
            else
            {
                alreadyUnlockedRecipeList.Add(recipe);
            }
        }
    }

    private void Start()
    {
        LoadRecipeProgress();
        if (unlockedRecipesUIPanel != null)
        {
            unlockedRecipesUIPanel.SetActive(false);
        }
    }

    public void LoadRecipeProgress()
    {
        currentPlayerData = SaveSystem.LoadPlayer();
        if (currentPlayerData == null)
        {
            currentPlayerData = new PlayerData(); // Initialize if LoadPlayer returned null
        }
        if (currentPlayerData.unlockedRecipeNames == null)
        {
            currentPlayerData.unlockedRecipeNames = new List<string>();
        }
        if (currentPlayerData.toUnlockRecipeNames == null) // If you save this list as well
        {
            currentPlayerData.toUnlockRecipeNames = new List<string>();
        }
        ResetAllRecipesToDefault();

        // Apply unlocked states from saved data
        if (currentPlayerData.unlockedRecipeNames.Count > 0) 
        {
            foreach (string recipeName in currentPlayerData.unlockedRecipeNames)
            {
                if (allRecipesByName.TryGetValue(recipeName, out UnlockableRecipe recipe))
                {
                    recipe.isUnklocked = true;

                }
                else
                {
                    Debug.LogWarning($"Recipe with name '{recipeName}' not found");
                }
            }
        }
        

        SetUpUnlockablePool();
    }

    public void SaveRecipeProgress()
    {
        if (currentPlayerData == null)
        {
            Debug.LogError("Player Data is null! Cannot save recipe progress.");
            return;
        }

       
        currentPlayerData.unlockedRecipeNames.Clear();
        currentPlayerData.toUnlockRecipeNames.Clear();


        foreach (var recipe in recipeList) // Iterate through the master list
        {
            if (recipe.isUnklocked)
            {
                currentPlayerData.unlockedRecipeNames.Add(recipe.name);
               
            }
            
        }

        SaveSystem.SavePlayer(currentPlayerData); // Use the SavePlayer overload that takes PlayerData
        Debug.Log("Recipe progress saved!");
    }

    
    private void ResetAllRecipesToDefault()
    {
        foreach (var recipe in recipeList)
        {
            if (recipe.isDefaultUnlocked)
            {
                recipe.isUnklocked = true;
                recipe.stacked = 1; // Assuming 1 stack for default unlocked
            }
            else // Otherwise, it's locked and not stacked
            {
                recipe.isUnklocked = false;
                recipe.stacked = 0; // Or 1 if that's your base for all recipes
            }
        }
    }

    public GameObject unlockedRecipesUIPanel; 
    public Transform unlockedRecipesContentParent; 
    public GameObject recipeItemUIPrefab;
    private List<GameObject> currentRecipeUIItems = new List<GameObject>();

    public void UpdateUnlockedRecipesUI()
    {
        // Clear any existing items first
        foreach (GameObject item in currentRecipeUIItems)
        {
            Destroy(item);
        }
        currentRecipeUIItems.Clear();

        alreadyUnlockedRecipeList.Sort((r1, r2) => r1.name.CompareTo(r2.name));

        // Iterate through unlocked recipes and create UI elements
        foreach (UnlockableRecipe recipe in alreadyUnlockedRecipeList)
        {
            // Instantiate the prefab
            GameObject recipeItem = Instantiate(recipeItemUIPrefab, unlockedRecipesContentParent);
            currentRecipeUIItems.Add(recipeItem);

            // Get the TextMeshProUGUI component from the instantiated item
            TextMeshProUGUI recipeNameText = recipeItem.GetComponentInChildren<TextMeshProUGUI>();
            if (recipeNameText != null)
            {
                // Display recipe name and stack count if applicable
                if (recipe.isRepeatUnlockable && recipe.maxStacked > 1)
                {
                    recipeNameText.text = $"{recipe.name} (x{recipe.stacked}/{recipe.maxStacked})";
                }
                else
                {
                    recipeNameText.text = recipe.name;
                }
            }
            else
            {
                Debug.LogWarning("RecipeItemUIPrefab is missing a TextMeshProUGUI");
            }

            // Image recipeIcon = recipeItem.GetComponentInChildren<Image>();
            // if (recipeIcon != null && recipe.icon != null) {
            //     recipeIcon.sprite = recipe.icon;
            // }
        }
    }

    public void ToggleUnlockedRecipesUIPanel()
    {
        if (unlockedRecipesUIPanel != null)
        {
            unlockedRecipesUIPanel.SetActive(!unlockedRecipesUIPanel.activeSelf);
            // Optionally, refresh the UI every time it's shown, just in case
            if (unlockedRecipesUIPanel.activeSelf)
            {
                UpdateUnlockedRecipesUI();
            }
        }
    }
}
