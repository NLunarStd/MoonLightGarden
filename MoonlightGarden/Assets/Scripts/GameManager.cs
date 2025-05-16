using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
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

        LoadCanvas.gameObject.SetActive(true);
    }

    public LightCycleController lightCycleController;
    public PlayerController playerController;
    public UIController uIController;
    public ResourceManager resourceManager;
    public Inventory inventory;
    public TimeBaseEventSystem timeBaseEventSystem;
    public SoundManager soundManager;
    public GameOverUIController gameOverUIController;
    public SessionData sessionData;
    public string playerName;

    public int currentDay = 1;

    public Transform respawnPoint;
    public Transform flowerTransform;

    public bool isSetUpComplete = false;
    public Transform loadingUI;
    public Transform mainUI;

    public bool isGameOver = false;

    public Transform LoadCanvas;
    public enum GameState
    {
        Play, Pause, GameOver
    }
    private void Start()
    {
        lightCycleController = GetComponent<LightCycleController>();
        uIController = GetComponent<UIController>();
        resourceManager = GetComponent<ResourceManager>();
        inventory = GetComponent<Inventory>();
        timeBaseEventSystem = GetComponent<TimeBaseEventSystem>();
        soundManager = GetComponent<SoundManager>();
        sessionData = GetComponent<SessionData>();


        uIController.UpdateDayText(currentDay);
        uIController.UpdatePlayerName();
    }
    private void Update()
    {
        ManagePlayerLantern();
          
    }
    public void UpdateGameState(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.Play:
                isGameOver = false;
                break;
            case GameState.Pause: break;
            case GameState.GameOver:
                isGameOver = true;
                gameOverUIController.gameObject.SetActive(true);
                break;
        }
    }
    void ManagePlayerLantern()
    {
        if (lightCycleController.normalizedTime > 0.65f)
        {
            playerController.OpenLatern();
        }
        else
        {
            playerController.CloseLatern();
        }
    }

    public void UpdateResorce(ResourceManager.ResourceType resourceType,int amount)
    {
        resourceManager.UpdateResource(resourceType, amount);
    }
    public void UpdateJewelState(ResourceManager.JewelType jewelType, bool stage)
    {
        resourceManager.UpdateJewelState(jewelType, stage);
    }
    public bool CheckJewelState(ResourceManager.JewelType jewelType)
    {
        return resourceManager.CheckJewelState(jewelType);
    }
    public void UpdateDay()
    {
        currentDay += 1;
        uIController.UpdateDayText(currentDay);
        sessionData.totalDay += 1;
    }

    public void AddItem(Item item, int amount)
    {
        inventory.AddItem(item, amount);
    }

    public void SetUpComplete()
    {
        isSetUpComplete = true;
        loadingUI.gameObject.SetActive(false);
        mainUI.gameObject.SetActive(true);

    }
}
