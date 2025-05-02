using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour //needs derived classes for each specific item type. 
{
    public ItemData itemData;
    public Inventory inventory;
    public GameObject prefab;
    
    protected GameObject ItemOwner;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Init(ItemData itemData, Inventory inv, GameObject prefab)
    {
        this.itemData = itemData;
        this.inventory = inv;
        this.prefab = prefab;
        
    }

    public abstract void Use();

    public virtual void SecondaryUse() 
    {

    }

    public virtual void SetOwner(GameObject owner)
    {
        ItemOwner = owner; 
    }

    public void PlayUseSound()
    {
        
        if (itemData.useSound != null)
        {
            MusicManager.instance.PlaySFX(itemData.useSound,ItemOwner.transform.position);
            Debug.Log("Playing use for item: " + itemData.useSound.name + " at " + ItemOwner.transform.position);
        }
    }

    protected void PlaySecondaryUseSound(AudioClip clip)
    {
        
        if (clip != null)
        {
            MusicManager.instance.PlaySFX(clip, ItemOwner.transform.position);
        }

    }

}
