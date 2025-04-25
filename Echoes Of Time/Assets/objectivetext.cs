using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class objectivetext : MonoBehaviour
{
    public TextMeshProUGUI objectiveText;
    private string objectiveTextString;

    // Start is called before the first frame update
    void Start()
    {
        objectiveText = GetComponent<TextMeshProUGUI>();
        objectiveTextString = " ";
        //Debug.Log("Objective Text: " + objectiveTextString);
    }

    // Update is called once per frame
    void Update()
    {
        objectiveText.text = objectiveTextString;
    }

    public void SetObjectiveText(Component sender, object data)
    {
        if(data is object[] dataArray)
        {
            data = dataArray[0];
            if(data is BaseObjective)
            {
                //Debug.Log("Data is base objective");
                float progress = ((BaseObjective)data).currentProgress * 100;

                if(float.IsInfinity(progress) || float.IsNaN(progress))
                {
                    progress = 0f;
                }
                objectiveTextString = ((BaseObjective)data).objectiveData.objectiveName + " " + progress.ToString("F0") + "%"; 
            }
        }
    }

    public void ClearObjectiveText(Component sender, object data)
    {
        objectiveTextString = "";
    }
}
