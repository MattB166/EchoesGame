using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour //needs derived classes for each specific item type. 
{
    public ItemData itemData;
    public Inventory inventory;
    public GameObject prefab;
    private AudioSource audioSource;
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
        audioSource = inventory.gameObject.AddComponent<AudioSource>();
        if (audioSource != null)
        {
            //Debug.Log("Audio source added to inventory game object");
            audioSource.clip = itemData.useSound;
            audioSource.playOnAwake = false;
        }
        //Debug.Log("Item initialized with " + itemData.name);
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
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }

    protected void PlaySecondaryUseSound(AudioClip clip)
    {
        if (audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

}
