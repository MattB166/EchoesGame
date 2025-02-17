using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearItem : MeleeWeaponItem
{
    public bool isThrown;
    public int ThrowDamage; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(isThrown);
        if(TryGetComponent(out Rigidbody2D rb))
        {
           if(rb.velocityX > 0)
            {
                //Debug.Log("Spear is moving right");
            }
            if (rb.velocityX < 0)
            {
               // Debug.Log("Spear is moving left");
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
        //unsubscribe first
        inventory.player.GetComponent<Actions>().attackAnimFinishedCallback -= ThrowSpear;
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
            if(spear != null)
            {
                //Debug.Log("Spear thrown");
            }
            spear.AddComponent<Rigidbody2D>();
            Rigidbody2D rb = spear.GetComponent<Rigidbody2D>();
            Vector2 force = new Vector2(20, 0);
            rb.AddForce(force, ForceMode2D.Impulse);
            spear.GetComponent<SpearItem>().isThrown = true;
            rb.gravityScale = 0;
            inventory.player.GetComponent<Actions>().attackAnimFinishedCallback -= ThrowSpear;
            inventory.RemoveItem(this);
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       
        if (isThrown)
        {
            //Debug.Log("Spear hit something");
            if (collision.gameObject.TryGetComponent(out IDamageable damageable))
            {
                Debug.Log("Spear hit damageable");
                damageable.TakeDamage(ThrowDamage);
                isThrown = false;
                Rigidbody2D spearRb = GetComponent<Rigidbody2D>();
                Destroy(spearRb);


            }
            //check if interactable 
        }
    }



}
