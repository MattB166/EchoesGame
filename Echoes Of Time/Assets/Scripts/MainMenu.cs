using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// handles buttons, music , and other menu related things 
/// </summary>
public class MainMenu : MonoBehaviour
{
    private void Awake()
    {
        GameManager.instance.SetCurrentSceneType(SceneType.MainMenu);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void PlayGame()
    {
        GameManager.instance.SetCurrentSceneType(SceneType.Game);
        if(SavingSystem.SaveSlotExists(0))
        {
            GameManager.instance.LoadGame(0);
        }
        else
        {
            GameManager.instance.currentSaveSlot = 0;
            CheckPointSystem.instance.lastActiveLevel = "TestingLevel";
            SceneManager.LoadScene(CheckPointSystem.instance.lastActiveLevel, LoadSceneMode.Single);
        }
    }
}
