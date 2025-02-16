using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour //needs derived classes for each specific item type. 
{
    public ItemData itemData;
    public Inventory inventory;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Init(ItemData itemData, Inventory inv)
    {
        this.itemData = itemData;
        this.inventory = inv;
        //Debug.Log("Item initialized with " + itemData.name);
    }

    public abstract void Use();

}
