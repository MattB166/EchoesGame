using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectiveDescription : MonoBehaviour
{

    public TextMeshProUGUI objectiveText;
    private string objectiveTextString;
    public GameEvent frozenTutorialEventComplete;
    public bool isFrozen = false;
    public bool hasBeenFrozen = false;

    // Start is called before the first frame update
    void Start()
    {
        objectiveText = GetComponent<TextMeshProUGUI>();
        objectiveTextString = "";
    }

    // Update is called once per frame
    void Update()
    {
        objectiveText.text = objectiveTextString;
        if(Input.anyKeyDown && isFrozen && !PauseMenu.isPaused)
        {
            UnfreezeObjectiveDescription(this, null);
        }
    }

    public void SetObjectiveDescriptionText(Component sender, object data)
    {
        if (data is object[] dataArray)
        {
            data = dataArray[0];
            if (data is BaseObjective)
            {
                //Debug.Log("Setting Objective Description Text");
                //write the description of the objective
                objectiveTextString = ((BaseObjective)data).objectiveData.objectiveDescription;
                //freeze time to allow player to read the description, and when any input is detected, unfreeze time and complete the objective. 
                if(!hasBeenFrozen)
                {
                    Time.timeScale = 0f;
                    isFrozen = true;
                    hasBeenFrozen = true;
                }
               

            }
        }

    }

    public void UnfreezeObjectiveDescription(Component sender, object data)
    {
        //Debug.Log("Unfreezing Objective Description");
        Time.timeScale = 1;
        isFrozen = false;
        //ClearObjectiveText(this, null);
    }

    public void ClearObjectiveText(Component sender, object data)
    {
        objectiveTextString = "";
        frozenTutorialEventComplete.Announce(this,null);
        hasBeenFrozen = false;
        //Time.timeScale = 1;
    }
}
