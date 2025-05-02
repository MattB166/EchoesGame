using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProjectileCounterText : MonoBehaviour
{
    private TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetText(Component sender, object data)
    {
        if (data is object[] dataArray)
        {
            data = dataArray[0];
            if (data is Projectiles projectiles)
            {
                text.text = projectiles.ammoCount.ToString();
            }
        }
    }
}
