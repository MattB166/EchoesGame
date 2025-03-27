using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelNameScript : MonoBehaviour
{
    public TextMeshProUGUI levelNameText;
    private string levelNameTextString;

    // Start is called before the first frame update
    void Start()
    {
        levelNameText = GetComponent<TextMeshProUGUI>();
        levelNameTextString = SceneManager.GetActiveScene().name;
        levelNameText.text = levelNameTextString;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
