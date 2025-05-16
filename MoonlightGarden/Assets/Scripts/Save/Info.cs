using UnityEngine;

public class Info : MonoBehaviour
{
    
    void Start()
    {
        SavePlayer();
    }

    public int coin;

    public int day;
    public void SavePlayer()
    {
        SaveSystem.SavePlayer(GameManager.instance);
    }

    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        coin = data.currentMoonlightShard;
        day = data.currentDay;

        /* Vector3 position
         * position.x = data.positiom[0]
         * position.y = data.positiom[1]
         * position.z = data.positiom[2]
         * playerPosition = position
        */
    }

}
