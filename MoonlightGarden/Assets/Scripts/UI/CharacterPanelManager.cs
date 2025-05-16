using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPanelManager : MonoBehaviour
{
    public Transform characterName;
    public Transform characterImage;
    public Transform skillPoint;
    public PlayableCharacter character;


    public void UpdateChracter(PlayableCharacter playableCharacter)
    {
        characterName.GetComponent<TextMeshProUGUI>().text = playableCharacter.characterName;
        characterImage.GetComponent<Image>().sprite = playableCharacter.characterImage;
        skillPoint.GetComponent<TextMeshProUGUI>().text = $"Skill point : {playableCharacter.skillPoint.ToString()}";

    }

    public void UpdateSkillPoint()
    {

    }

}
