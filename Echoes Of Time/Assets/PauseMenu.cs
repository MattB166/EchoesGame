using System.Collections;
using System.Collections.Generic;
//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
/// <summary>
/// manages the control of the pause menu, so that it can be paused and unpaused and go back to menu, and quit. 
/// </summary>
public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance;
    public static bool isPaused = false;

    public GameObject pauseMenuUI;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    public void TogglePause(InputAction.CallbackContext context)
    {
        //Debug.Log("TogglePause called");
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
        GameManager.instance.SetCurrentSceneType(SceneType.PauseMenu);
    }

    public void Unpause()
    {
        pauseMenuUI.SetActive(false);
        if(Time.timeScale != 1f)
        {
            Time.timeScale = 1f;
        }
        
        isPaused = false;
        GameManager.instance.SetCurrentSceneType(SceneType.Game);
    }

    public void LoadMenu()
    {
        Unpause();
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadSettings()
    {
        Debug.Log("Loading settings");
    }

    public void QuitGame()
    { 
        Application.Quit();

        
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
