using Mono.CSharp;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class GiftCodeController : MonoBehaviour
{
    public void TogglePanel()
    {
        if(transform.gameObject.activeSelf)
        {
            transform.gameObject.SetActive(false);
            lobbyUISoundControl.uiAudioSource.PlayOneShot(lobbyUISoundControl.popSound);
        }
        else
        {
            transform.gameObject.SetActive(true);
            lobbyUISoundControl.uiAudioSource.PlayOneShot(lobbyUISoundControl.popSound);
        }
    }
    string textInput;
    public void EndInputText()
    {
        textInput = inputField.text;
        switch (textInput)
        {
            case "Motherload":
                CurrencyManager.instance.UpdateMoonlightShard(300);
                lobbyUISoundControl.uiAudioSource.PlayOneShot(lobbyUISoundControl.popSound);
                LobbyUIController.UpdateCurrency();
                break;
        }
        PlayerData playerData = SaveSystem.LoadPlayer();
        playerData.currentMoonlightShard = CurrencyManager.instance.GetMoonlightShard();
        SaveSystem.SavePlayer(playerData);
        inputField.text = "Enter Code...";
        TogglePanel();
    }

    public TMP_InputField inputField;
    public LobbyUISoundControl lobbyUISoundControl;
    public LobbyUIController LobbyUIController;
}
