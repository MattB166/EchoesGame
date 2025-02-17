using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearItem : MeleeWeaponItem
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(TryGetComponent(out Rigidbody2D rb))
        {
           if(rb.velocityX > 0)
            {
                Debug.Log("Spear is moving right");
            }
            if (rb.velocityX < 0)
            {
                Debug.Log("Spear is moving left");
            }
            else if (rb.velocityX == 0 && rb.velocityY == 0)
            {
                Rigidbody2D spearRb = GetComponent<Rigidbody2D>();
                Destroy(spearRb);
            }
        }
    }
    public override void Use()
    {

    }

    public override void SecondaryUse()
    {
        Debug.Log("Secondary use of spear");
        //on second use spear is dropped until recollected. 
        
        // inventory.RemoveItem(this); do after the spear has been thrown, otherwise the item will be removed from the inventory before it is thrown and so animation will not be played.
        inventory.player.GetComponent<Actions>().attackAnimFinishedCallback += ThrowSpear;
    }

    public override void Init(ItemData itemData,Inventory inv, GameObject prefab)
    {
        base.Init(itemData, inv,prefab);
        meleeWeaponData = itemData as MeleeWeaponData;
       
    }

    public void ThrowSpear(Actions.Weapons weapon)
    {
        if(weapon == Actions.Weapons.Spear) //probably a better way to do this. 
        {
            float xPos = inventory.player.transform.position.x + 2.0f;
            float yPos = inventory.player.transform.position.y;
            Vector2 pos = new Vector2(xPos, yPos);
            GameObject spear = Instantiate(prefab, pos, inventory.player.transform.rotation);
            spear.AddComponent<Rigidbody2D>();
            Rigidbody2D rb = spear.GetComponent<Rigidbody2D>();
            Vector2 force = new Vector2(10, 4);
            rb.AddForce(force, ForceMode2D.Impulse);
            inventory.player.GetComponent<Actions>().attackAnimFinishedCallback -= ThrowSpear;
            inventory.RemoveItem(this);
        }
        
    }

   

}
