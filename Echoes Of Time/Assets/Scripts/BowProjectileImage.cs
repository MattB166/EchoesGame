using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BowProjectileImage : MonoBehaviour
{
    private Image image;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if(image.sprite == null)
        {
            image.enabled = false;
        }
        else
        {
            image.enabled = true;
        }
    }

    public void SetImage(Component sender, object data)
    {
        if (data is object[] dataArray)
        {
            data = dataArray[0];
            if (data is Projectiles projectiles)
            {
                image.sprite = projectiles.projectile.projectileData.itemSprite;
            }
        }
    }
}
