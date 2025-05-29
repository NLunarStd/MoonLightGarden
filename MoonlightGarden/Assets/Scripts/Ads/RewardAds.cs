using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using TMPro;
using System; // Required for DateTime

public class RewardedAdsButton : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] Button _showAdButton;
    [SerializeField] string _androidAdUnitId = "Rewarded_Android";
    [SerializeField] string _iOSAdUnitId = "Rewarded_iOS";
    string _adUnitId = null;
    public LobbyUIController _lobbyUIController;
    public PauseManager pauseManager;

    private int showAdsCount;
    private const string LastAdResetKey = "LastAdResetTime";
    private const string AdCountKey = "RewardedAdCount";
    private const int MaxDailyAds = 3;

    void Awake()
    {
#if UNITY_IOS
        _adUnitId = _iOSAdUnitId;
#elif UNITY_ANDROID
        _adUnitId = _androidAdUnitId;
#endif

        _showAdButton.interactable = false;
    }

    void Start()
    {
        InitializeAdCount();
        UpdateAdButtonText();

        if (Advertisement.isInitialized)
        {
            LoadAd();
        }
        else
        {
            StartCoroutine(WaitForInitializationAndLoad());
        }
    }

    private void InitializeAdCount()
    {
        long lastResetTicks = long.Parse(PlayerPrefs.GetString(LastAdResetKey, "0"));
        DateTime lastResetTime = new DateTime(lastResetTicks);
        DateTime currentTime = DateTime.Now;

        DateTime sixAMToday = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, 6, 0, 0);
        DateTime sixAMYesterday = sixAMToday.AddDays(-1);

        bool shouldReset = false;

        if (currentTime >= sixAMToday)
        {
            if (lastResetTime < sixAMToday)
            {
                shouldReset = true;
            }
        }
        else 
        {
            if (lastResetTime < sixAMYesterday || lastResetTicks == 0) 
            {
                shouldReset = true;
            }
        }

        if (shouldReset)
        {
            showAdsCount = MaxDailyAds;
            PlayerPrefs.SetString(LastAdResetKey, currentTime.Date.AddHours(6).Ticks.ToString());
            PlayerPrefs.SetInt(AdCountKey, showAdsCount);
        }
        else
        {
            showAdsCount = PlayerPrefs.GetInt(AdCountKey, MaxDailyAds);
        }
        PlayerPrefs.Save();
    }

    private void UpdateAdButtonText()
    {
        if (_showAdButton.GetComponentInChildren<TextMeshProUGUI>() != null)
        {
            _showAdButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Free 30 Shards\r\n( {showAdsCount} / {MaxDailyAds} )\r\nWatch Ads.";
            _showAdButton.interactable = showAdsCount > 0;
        }
    }

    IEnumerator WaitForInitializationAndLoad()
    {
        yield return new WaitUntil(() => Advertisement.isInitialized);
        LoadAd();
    }

    public void LoadAd()
    {
        Debug.Log("Loading Ad: " + _adUnitId);
        _showAdButton.interactable = false;
        Advertisement.Load(_adUnitId, this);
    }

    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        Debug.Log("Ad Loaded: " + adUnitId);

        if (adUnitId.Equals(_adUnitId))
        {
            _showAdButton.onClick.RemoveAllListeners();
            _showAdButton.onClick.AddListener(ShowAd);
            _showAdButton.interactable = showAdsCount > 0;
        }
    }

    public void ShowAd()
    {
        if (showAdsCount > 0)
        {
            _showAdButton.interactable = false;
            Advertisement.Show(_adUnitId, this);
            pauseManager.Pause();
        }
        else
        {
            Debug.Log("No more rewarded ads available today.");
            UpdateAdButtonText();
        }
    }

    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            Debug.Log("Unity Ads Rewarded Ad Completed");

            if (showAdsCount > 0)
            {
                CurrencyManager.instance.UpdateMoonlightShard(30);
                _lobbyUIController.UpdateCurrency();
                showAdsCount -= 1;
                PlayerPrefs.SetInt(AdCountKey, showAdsCount);
                PlayerPrefs.Save();

                _lobbyUIController.ShowRewardGrant();

                PlayerData data = SaveSystem.LoadPlayer();
                data.currentMoonlightShard = CurrencyManager.instance.GetMoonlightShard();
                SaveSystem.SavePlayer(data);
            }
            UpdateAdButtonText();
            LoadAd();
        }
    }

    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
        _showAdButton.interactable = showAdsCount > 0;
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        _showAdButton.interactable = showAdsCount > 0;
    }

    public void OnUnityAdsShowStart(string adUnitId) { }
    public void OnUnityAdsShowClick(string adUnitId) { }

    void OnDestroy()
    {
        _showAdButton.onClick.RemoveAllListeners();
    }
}