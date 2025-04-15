using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// manages the control of the pause menu, so that it can be paused and unpaused and go back to menu, and quit. 
/// </summary>
public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;

    public GameObject pauseMenuUI;

    public void TogglePause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (isPaused)
            {
                Unpause();
            }
            else
            {
                Pause();
            }
        }
    }


    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        if(Time.timeScale != 0f)
        {
            Time.timeScale = 0f;
        }
        
        isPaused = true;
    }

    public void Unpause()
    {
        pauseMenuUI.SetActive(false);
        if(Time.timeScale != 1f)
        {
            Time.timeScale = 1f;
        }
        
        isPaused = false;
    }

    public void LoadMenu()
    {
        Debug.Log("Loading Menu...");
    }

    public void QuitGame()
    { 
        Application.Quit();

        
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
