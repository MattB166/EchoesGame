using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// handles buttons, music , and other menu related things 
/// </summary>
public class MainMenu : MonoBehaviour
{
    public AudioClip menuMusic;
    public GameObject mainMenuCanvas;
    public GameObject settingsMenuCanvas;

    private void Awake()
    {
        GameManager.instance.SetCurrentSceneType(SceneType.MainMenu);
    }
    // Start is called before the first frame update
    void Start()
    {
        MusicManager.instance.PlayMusic(menuMusic,true);
    }

    // Update is called once per frame
    void Update()
    {
       

    }

    public void PlayGame()
    {
        MusicManager.instance.StopMusic();

        GameManager.instance.SetCurrentSceneType(SceneType.Game);
        if(SavingSystem.SaveSlotExists(0))
        {
            Debug.Log("Loading game from slot 0 as it exists");
            GameManager.instance.LoadGameFromSlot(0);
        }
        else
        {
            GameManager.instance.currentSaveSlot = 0;
            CheckPointSystem.instance.lastActiveLevel = "TestingLevel";
            SceneManager.LoadScene(CheckPointSystem.instance.lastActiveLevel, LoadSceneMode.Single);
        }
    }

    public void SettingsMenu()
    {
        //enable settings canvas and disable main canvas
        mainMenuCanvas.SetActive(false);
        settingsMenuCanvas.SetActive(true);
    }
}
