using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CharacterSelector : MonoBehaviour
{
    public Image characterImage; // Image ที่จะแสดงตัวละคร
    private int currentIndex = 0; // Index ตัวละครปัจจุบัน
    public List<PlayableCharacter> characterList;
    public PlayableCharacter currentSelectedCharacter;
    public CharacterPanelManager characterPanelManager;


    public LobbyUISoundControl lobbyUISoundControl;
    public void Start()
    {
        UpdateSelectedCharacter();
        lobbyUISoundControl = GetComponent<LobbyUISoundControl>();
    }

    public void SelectPreviousCharacter()
    {
        currentIndex--;
        if (currentIndex < 0)
        {
            currentIndex = characterList.Count - 1;
        }
        UpdateSelectedCharacter();
        lobbyUISoundControl.uiAudioSource.PlayOneShot(lobbyUISoundControl.clickSound);
    }

    public void SelectNextCharacter()
    {
        currentIndex++;
        if (currentIndex >= characterList.Count)
        {
            currentIndex = 0;
        }
        UpdateSelectedCharacter();
        lobbyUISoundControl.uiAudioSource.PlayOneShot(lobbyUISoundControl.clickSound);
    }
    void UpdateSelectedCharacter()
    {
        currentSelectedCharacter = characterList[currentIndex];
        UpdateCharacterImage();
        characterPanelManager.UpdateChracter(currentSelectedCharacter);
    }
    private void UpdateCharacterImage()
    {
        characterImage.sprite = characterList[currentIndex].characterImage;
    }
}