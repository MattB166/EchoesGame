using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour //needs derived classes for each specific item type. 
{
    public ItemData itemData;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Init(ItemData itemData)
    {
        this.itemData = itemData;
        //Debug.Log("Item initialized with " + itemData.name);
    }
}
