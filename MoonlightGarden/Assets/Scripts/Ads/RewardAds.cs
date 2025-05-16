using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using TMPro;

public class RewardedAdsButton : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] Button _showAdButton;
    [SerializeField] string _androidAdUnitId = "Rewarded_Android";
    [SerializeField] string _iOSAdUnitId = "Rewarded_iOS";
    string _adUnitId = null; // This will remain null for unsupported platforms
    public LobbyUIController _lobbyUIController;

    int showAdsCount = 3;
    void Awake()
    {
        // Get the Ad Unit ID for the current platform:
#if UNITY_IOS
        _adUnitId = _iOSAdUnitId;
#elif UNITY_ANDROID
        _adUnitId = _androidAdUnitId;
#endif

        // Disable the button until the ad is ready to show:
        _showAdButton.interactable = false;
    }

    bool stopCoroutine = false;
    Coroutine runningCoroutine;
    void Start()
    {
        // Load the first ad when the script starts, if Unity Ads is initialized
        if (Advertisement.isInitialized)
        {
            LoadAd();
        }
        else
        {
            // Optionally, start a coroutine to wait for initialization and then load
            StartCoroutine(WaitForInitializationAndLoad());
        }
    }

    IEnumerator WaitForInitializationAndLoad()
    {
        yield return new WaitUntil(() => Advertisement.isInitialized);
        LoadAd();
    }

    public void LoadAd()
    {
        // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
        Debug.Log("Loading Ad: " + _adUnitId);
        _showAdButton.interactable = false;
        Advertisement.Load(_adUnitId, this);
        
    }

    // If the ad successfully loads, add a listener to the button and enable it:
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        Debug.Log("Ad Loaded: " + adUnitId);

        if (adUnitId.Equals(_adUnitId))
        {
            // Configure the button to call the ShowAd() method when clicked:
            _showAdButton.onClick.AddListener(ShowAd);
            // Enable the button for users to click:
            _showAdButton.interactable = true;
        }
    }
    public PauseManager pauseManager;
    // Implement a method to execute when the user clicks the button:
    public void ShowAd()
    {
        // Disable the button:
        _showAdButton.interactable = false;
        // Then show the ad:
        Advertisement.Show(_adUnitId, this);
        pauseManager.Pause();
        isGrantReward = false;
    }
    
    // Implement the Show Listener's OnUnityAdsShowComplete callback method to determine if the user gets a reward:
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            Debug.Log("Unity Ads Rewarded Ad Completed");
            // Grant a reward.
            if (!isGrantReward && showAdsCount > 0)
            {
                CurrencyManager.instance.UpdateMoonlightShard(30);
                _lobbyUIController.UpdateCurrency();
                isGrantReward = true;
                showAdsCount -= 1;
                _showAdButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Free 30 Shards\r\n( {showAdsCount} / 3 )\r\nWatch Ads.";
                stopCoroutine = false;
                pauseManager.Pause();
                _lobbyUIController.ShowRewardGrant();

                PlayerData data = SaveSystem.LoadPlayer();
                data.currentMoonlightShard = CurrencyManager.instance.GetMoonlightShard();
                SaveSystem.SavePlayer(data);

            }
            isGrantReward = false;
            LoadAd();
        }
    }
    bool isGrantReward = false;

    // Implement Load and Show Listener error callbacks:
    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // Use the error details to determine whether to try to load another ad.
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // Use the error details to determine whether to try to load another ad.
    }

    public void OnUnityAdsShowStart(string adUnitId) { }
    public void OnUnityAdsShowClick(string adUnitId) { }

    void OnDestroy()
    {
        // Clean up the button listeners:
        _showAdButton.onClick.RemoveAllListeners();
    }
}