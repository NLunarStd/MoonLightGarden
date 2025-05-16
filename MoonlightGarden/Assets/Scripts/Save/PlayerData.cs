using UnityEngine;

[System.Serializable]
public class PlayerData 
{
    public string userName;
    public int currentDay;
    public int totalDay;
    public int currentMoonlightShard;
    public int currentBloodMoonShard;

    public int currentExp;
    public int currentLevel;
    public string currentSelectedCharacterName;

    //public Vector3 playerPosition;
    public float[] position;
    
    public PlayerData(GameManager gameManager) 
    {
        currentDay = gameManager.currentDay;

     
        
        position = new float[3];
        position[0] = gameManager.playerController.transform.position.x;
        position[1] = gameManager.playerController.transform.position.y;
        position[2] = gameManager.playerController.transform.position.z;
    }
    public PlayerData()
    {
        userName = "USERNAME";
        currentDay = 1;
        totalDay = 0;
        currentMoonlightShard = 0;
        currentBloodMoonShard = 0;
        currentExp = 0;
        currentLevel = 1;

    }

}
