using System.Collections;
using System.Xml;
using TMPro;
using UnityEngine;

public class GameOverUIController : MonoBehaviour
{
    public Transform conitunePanel;
    public Transform resultPanel;
    public ContinueGaneOverControl continueGaneOverControl;

    public float scoreMultiplier = 100;
    public int scoreAdding = 0;
    public void OpenContinuePanel()
    {
        conitunePanel.gameObject.SetActive(true);
    }
    public void CloseContinuePanel()
    {
        conitunePanel.gameObject.SetActive(false);
    }
    public void ShowResult()
    {
        resultPanel.gameObject.SetActive(true);
        CalculateScore();
    }

    public void CalculateScore()
    {
        int totalDayScore = GameManager.instance.sessionData.totalDay * 100;
        dayScore.text = totalDayScore.ToString();
        int totalKillScore = GameManager.instance.sessionData.totalKill * 5;
        killScore.text = totalKillScore.ToString();
        int totalBuiltScore = GameManager.instance.sessionData.totalBuilt * 10;
        builtScore.text = totalBuiltScore.ToString();
        int FinalScore = (int)((totalDayScore + totalKillScore + totalBuiltScore) * scoreMultiplier / 100) + scoreAdding;
        finalScore.text = FinalScore.ToString();
        StartCoroutine(UpdateScoreText());
    }

    public TextMeshProUGUI dayScore;
    public TextMeshProUGUI killScore; 
    public TextMeshProUGUI builtScore;

    public TextMeshProUGUI finalScore;
    public bool isCompleteShowScore = false;
    public IEnumerator UpdateScoreText()
    {
        GameManager.instance.soundManager.playerSource.Pause();
        GameManager.instance.soundManager.playerSource.PlayOneShot(GameManager.instance.soundManager.calculatedSound);
        yield return new WaitForSeconds(1);
        StartCoroutine(ShowScore(dayScore));
        StartCoroutine(ShowScore(killScore));
        StartCoroutine(ShowScore(builtScore));
        yield return new WaitForSeconds(1);
        StartCoroutine(ShowScore(finalScore));
        yield return new WaitForSeconds(1);
        isCompleteShowScore = true;
    }
    public float fadeSpeed = 0.01f;
    IEnumerator ShowScore(TextMeshProUGUI TMP)
    {
        bool isCompleteFade = false;
        
        if (!isCompleteFade)
        {
            while (TMP.color.a > 0)
            {
                TMP.color = new Color(TMP.color.r, TMP.color.g, TMP.color.b, TMP.color.a - fadeSpeed);
                yield return null;
            }
        }
        TMP.color = new Color(TMP.color.r, TMP.color.g, TMP.color.b, 0f);
        TMP.color = new Color(TMP.color.r, TMP.color.g, TMP.color.b, 1f);
        isCompleteFade = true;
        
        yield return null;
        yield return new WaitForSeconds(3);
        StartCoroutine(ShowEXPIncrease());
    }
    public Transform expGainPanel;
    public TextMeshProUGUI currentExptxt;
    public TextMeshProUGUI currentLeveltxt;
    public TextMeshProUGUI totalMoonLightShardReward;
    PlayerData data;
    int currentExp = 0;
    int currentLevel = 1;
    int currentExpRequire = 100;
    int totalAccumulatedExp = 1;
    int totalReward = 0;
    public IEnumerator ShowEXPIncrease()
    {
        expGainPanel.gameObject.SetActive(true);
        data = SaveSystem.LoadPlayer();
        currentExp = data.currentExp;
        currentLevel = data.currentLevel;
        if(currentLevel <= 1) currentLevel = 1;
        currentExpRequire = currentLevel * 100;
        totalAccumulatedExp = currentExp + int.Parse(finalScore.text);

        currentExptxt.text = $"{currentExp.ToString()}  / {currentExpRequire.ToString()}";
        currentLeveltxt.text = "Level " + currentLevel.ToString();
        yield return new WaitForSeconds(1);
        StartCoroutine(ChanageTextValue(currentExptxt));
    }
    public SceneLoader sceneLoader;
    public IEnumerator ChanageTextValue(TextMeshProUGUI text)
    {
        GameManager.instance.soundManager.playerSource.Pause();
        GameManager.instance.soundManager.playerSource.PlayOneShot(GameManager.instance.soundManager.getShardSound);
        
        while (currentExp < totalAccumulatedExp)
        {
            currentExp += 1;
            currentExptxt.text = $"{currentExp.ToString()}  / {currentExpRequire.ToString()}";

            if (currentExp >= currentExpRequire)
            {
                currentExp = 0;
                totalAccumulatedExp -= currentExpRequire;
                currentLevel += 1;
                currentExpRequire = currentLevel * 100;
                currentExptxt.text = $"{currentExp.ToString()}  / {currentExpRequire.ToString()}";
                currentLeveltxt.text = "Level " + currentLevel.ToString();
                totalReward += 10;
                totalMoonLightShardReward.text = totalReward.ToString();
            }
            yield return new WaitForSeconds(0.1f);
        }

        data.currentLevel = currentLevel;
        data.currentExp = currentExp;
        data.currentMoonlightShard += totalReward; 
        SaveSystem.SavePlayer(data);
        yield return new WaitForSeconds(1);
        sceneLoader.LoadSceneAsync();
    }
    
}
