using UnityEngine;

public class OpenPauseUIController : MonoBehaviour
{
    public PauseManager pauseManager;
   
    public void OpenPauseMenu()
    {
        pauseManager.Pause();
    }
}
