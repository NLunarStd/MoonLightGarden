using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUIController : MonoBehaviour
{

    public Transform ShopPanel;
    public TextMeshProUGUI MoonlightShard;
    public TextMeshProUGUI BloodMoonShard;
    public Transform rewardGrantPanel;
    public TMP_InputField inputFieldForName;
    public string userNameValue;
    public TextMeshProUGUI userName;
    public Button playSurviveButton;
    public CharacterSelector characterSelector;
    public SceneLoader sceneLoader;
    public TextMeshProUGUI playerLevel;

    public void OpenShopPanel()
    {
        ShopPanel.gameObject.SetActive(true);
    }

    public void CloseShopPanel()
    {
        ShopPanel.gameObject.SetActive(false);
    }

    public void UpdateCurrency()
    {
        int moonlighShard = CurrencyManager.instance.GetMoonlightShard();
        MoonlightShard.text = string.Format("{0:D5}",moonlighShard);
        int bloodmoonShard = CurrencyManager.instance.GetBloodMoonShard();
        BloodMoonShard.text = string.Format("{0:D5}",bloodmoonShard);
    }

    public void ShowRewardGrant()
    {
        rewardGrantPanel.gameObject.SetActive(true);
    }


    public void OpenInputField()
    {
        inputFieldForName.gameObject.SetActive(true);
    }

    public Transform userPanel;
    public Transform confirmPanel;
    public TextMeshProUGUI userLevel;
    public TextMeshProUGUI totalDay;
    public TextMeshProUGUI currentExp;

    public void TogglePlayInfo()
    {
        if (userPanel.gameObject.activeSelf)
        {
            userPanel.gameObject.SetActive(false);
        }
        else
        {
            userPanel.gameObject.SetActive(true);
        }
    }
    public void ToggleConfirmation()
    {
        if (confirmPanel.gameObject.activeSelf)
        {
            confirmPanel.gameObject.SetActive(false);
        }
        else
        {
            confirmPanel.gameObject.SetActive(true);
        }
    }
    public void CloseInputField()
    {
        if (inputFieldForName != null)
        {
            userName.text = inputFieldForName.text;
        }
        inputFieldForName.gameObject.SetActive(false);
        PlayerData data = new PlayerData();
        data.userName = userName.text;
        SaveSystem.SavePlayer(data);
    }
    void SavePlayerData()
    {
        PlayerData data = new PlayerData();
        SaveSystem.SavePlayer(data);
        LoadPlayerData();
    }
    void LoadPlayerData()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        userName.text = data.userName;
        CurrencyManager.instance.MoonlightShard = data.currentMoonlightShard;
        CurrencyManager.instance.BloodMoonShard = data.currentBloodMoonShard;
        playerLevel.text = data.currentLevel.ToString();
        UpdateCurrency();

        userLevel.text = data.currentLevel.ToString();
        int currentExpRequire = data.currentLevel * 100;
        currentExp.text = $"{data.currentExp.ToString()} / {currentExpRequire.ToString()}";
        totalDay.text = data .totalDay.ToString();
        Debug.Log("Load complete");
    }

    public void ResetData()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        PlayerData emptyData = new PlayerData();
        data = emptyData;
        SaveSystem.SavePlayer(data);
        LoadPlayerData() ;

    }

    public void PlaySurviveMode()
    {
        if (characterSelector.currentSelectedCharacter.isUnlokced)
        {
            PlayerData data = SaveSystem.LoadPlayer();
            data.currentSelectedCharacterName = characterSelector.currentSelectedCharacter.name;
            SaveSystem.SavePlayer(data);
            sceneLoader.LoadSceneAsync();
        }
        else
        {
            return;
        }
    }
    private void Start()
    {
        //SavePlayerData();
        LoadPlayerData();
    }
}
