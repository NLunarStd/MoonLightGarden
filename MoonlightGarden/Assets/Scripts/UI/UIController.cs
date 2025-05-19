using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UIController : MonoBehaviour
{
    public static UIController instance;
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
    public TextMeshProUGUI dayText;
    public TextMeshProUGUI pauseDatText;
    public TextMeshProUGUI playerName;
    public TextMeshProUGUI pausePlayerName;

    public Image hpFill;
    public Image flowerHPFill;

    public TextMeshProUGUI flowerAlertText;

    public Transform itemBarPanel;
    public Transform[] uiItemSlot;
    public Transform selectingItemSlot;
    public Color textColor;
    public Color selectedTextColor = Color.red;

    [Header("Image")]
    public Transform[] itemSlotImage;
    [Header("Slot")]
    public TextMeshProUGUI[] itemSlotIndexText;
    [Header("Amount")]
    public TextMeshProUGUI[] itemSlotAmountText;

    public Inventory playerInventory;
    private void Start()
    {
        SetUpUIInventorySlots();
        SetUPInDisplayColorColor();
        UpdateInventoryUI();
    }

    private void SetUpUIInventorySlots()
    {

        // Debug.Log(itemBarPanel.childCount);
        uiItemSlot = new Transform[itemBarPanel.childCount];
        for (int i = 0; i < itemBarPanel.childCount; i++)
        {
            uiItemSlot[i] = itemBarPanel?.GetChild(i);
        }
        itemSlotImage = new Transform[uiItemSlot.Length];
        itemSlotIndexText = new TextMeshProUGUI[uiItemSlot.Length];
        itemSlotAmountText = new TextMeshProUGUI[uiItemSlot.Length];
        for (int i = 0; i < uiItemSlot.Length; i++)
        {
            itemSlotImage[i] = uiItemSlot[i].GetChild(0).GetComponent<Transform>();
            itemSlotIndexText[i] = uiItemSlot[i].GetChild(1).GetComponent<TextMeshProUGUI>();
            itemSlotAmountText[i] = uiItemSlot[i].GetChild(2).GetComponent<TextMeshProUGUI>();
        }
        SetSlotToEmpty();
        if (uiItemSlot.Length > 0) 
        {
            selectingItemSlot = uiItemSlot[0];
            uiItemSlot[0].GetComponent<Toggle>().isOn = true;
            uiItemSlot[0].GetComponent<Toggle>().interactable = false;
        }
        CheckForSelected();
    }

    private void SetSlotToEmpty()
    {
        foreach (Transform itemSlot in itemSlotImage)
        {
            itemSlot.GetComponent<Image>().color = new Color(1,1,1,0);
        }
        foreach (TextMeshProUGUI itemAmountInSlot in itemSlotAmountText)
        {
            itemAmountInSlot.text = " ";
        }
    }

    public void CheckForSelected()
    {
        Transform newSelectingItemSlot = null;

        foreach (Transform itemSlot in uiItemSlot)
        {
            TextMeshProUGUI indexText = itemSlot.GetChild(1).GetComponent<TextMeshProUGUI>();
            
            if (itemSlot.GetComponent<Toggle>().isOn)
            {
                newSelectingItemSlot = itemSlot;
                if (newSelectingItemSlot != selectingItemSlot && selectingItemSlot != null) 
                {
                    selectingItemSlot.GetComponent<Toggle>().interactable = true;
                    selectingItemSlot.GetComponent<Toggle>().isOn = false;
                    selectingItemSlot.GetChild(1).GetComponent<TextMeshProUGUI>().color = textColor;
                    selectingItemSlot = newSelectingItemSlot;
                }
                indexText.color = selectedTextColor;
            }
            else
            {
                indexText.color = textColor;
            }
        }
    }
    public void UpdateInventoryUI()
    {
        if (playerInventory == null) return;

        for (int i = 0; i < playerInventory.inventorySlots.Count; i++)
        {
            if (playerInventory.inventorySlots[i].currentItem != null)
            {
                itemSlotImage[i].GetComponent<Image>().sprite = playerInventory.inventorySlots[i].currentItem.itemData.itemImage;
                itemSlotImage[i].GetComponent<Image>().color = Color.white;
                itemSlotAmountText[i].text = playerInventory.inventorySlots[i].currentAmount.ToString();
            }
            else
            {
                itemSlotImage[i].GetComponent<Image>().color = new Color(1, 1, 1, 0);
                itemSlotAmountText[i].text = "";
            }
        }
    }

    public void UpdateDayText(int day)
    {
        dayText.text = $"Day {day}";
        pauseDatText.text = $"Current Day: {day}";
    }
    public void UpdatePlayerName()
    {
        playerName.text = GameManager.instance.playerName;
        pausePlayerName.text = GameManager.instance.playerName;
    }

    public void PlaceBuilding()
    {
        InventorySlot selectedSlot = playerInventory.inventorySlots[GetSelectedIndex()];
        if (selectedSlot.currentItem != null && selectedSlot.currentItem.GetComponent<BuildingItem>() != null)
        {
            BuildingItem buildingItem = selectedSlot.currentItem.GetComponent<BuildingItem>();
            if (buildingItem.buildingPrefab != null)
            {
                // ส่ง interactor.transform จาก Player
                BuildingManager.instance.PlaceBuilding(buildingItem.buildingPrefab, PlayerController.instance.interactor.transform);
                Debug.Log("Place Building");
                playerInventory.RemoveItem(selectedSlot.currentItem);
            }
        }
    }


    public int GetSelectedIndex()
    {
        for (int i = 0; i < uiItemSlot.Length; i++)
        {
            if (uiItemSlot[i] == selectingItemSlot)
            {
                return i;
            }
        }
        return -1;
    }

    private Color normalInDisplayColor = Color.white;
    private Color damagedDisplayColor = Color.red;
    public Gradient damageColorGradient = new Gradient();
    public Image inDisplayPortraitImage;
    void SetUPInDisplayColorColor()
    {
        GradientColorKey[] colorKeys = new GradientColorKey[2];
        colorKeys[0] = new GradientColorKey(normalInDisplayColor, 1f);
        colorKeys[1] = new GradientColorKey(damagedDisplayColor, 0f);

        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
        alphaKeys[0] = new GradientAlphaKey(1f, 1f);
        alphaKeys[1] = new GradientAlphaKey(1f, 0f);
        damageColorGradient.SetKeys(colorKeys, alphaKeys);

    }
    public void UpdateHP(float amount)
    { 
        hpFill.fillAmount = amount;
        Color targetColor = damageColorGradient.Evaluate(amount);
        inDisplayPortraitImage.color = targetColor;
    }
    public void UpdateFlowerHP(float amount)
    {
        flowerHPFill.fillAmount = amount;
    }
    public Transform respawningPanel;
    public void ToggleRespawningScreen()
    {
        if (respawningPanel.gameObject.activeSelf == true)
        {
            respawningPanel.gameObject.SetActive(false);
        }
        else
        {
            respawningPanel.gameObject.SetActive(true);
        }
    }
    public TextMeshProUGUI respawnCoolDownText;
    public void UpdateRespawningCooldown(string text)
    {
        respawnCoolDownText.text = text;
    }


    public void DisplayAlertText()
    {
        StartCoroutine(FadeOutText(flowerAlertText));
        isCompleteFade = false;
    }
    public float fadeSpeed = 0.01f;
    private bool isCompleteFade = false;
    IEnumerator FadeOutText(TextMeshProUGUI text)
    {
        text.transform.gameObject.SetActive(true);
        if (!isCompleteFade)
        {
            while (text.color.a > 0)
            {
                text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - fadeSpeed);
                yield return null;
            }
        }
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0f);
        text.transform.gameObject.SetActive(false);
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1f);
        isCompleteFade = true;
    }
}
