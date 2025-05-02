using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearItem : MeleeWeaponItem
{
    public bool isThrown;
    public int ThrowDamage;
    public int maxPierces;
    public int pierceCount; 
    private bool returnToPlayer;
    private Vector2 startPos;
    public float throwDistance;
    private float currentDistance;
    public AudioClip throwSound;
    //private Collider2D spearCol; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(inventory != null)
        {
            startPos = inventory.player.transform.position;
        }
        
        
        CheckDistance();
        if (returnToPlayer)
        {
            ReturnToPlayer();
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
            float direction = inventory.player.GetComponent<Movement>().direction;
            
            float xOffSet = 2.0f * direction;
            float xPos = inventory.player.transform.position.x + xOffSet;
            float yPos = inventory.player.transform.position.y;
            Vector2 pos = new Vector2(xPos, yPos);
            Quaternion rot = direction == 1 ? Quaternion.identity : Quaternion.Euler(0, 180, 0);
            GameObject spear = Instantiate(prefab, pos, rot);
            foreach (Collider2D col in spear.GetComponents<Collider2D>())
            {
                if(!col.isTrigger)
                {
                    col.enabled = false;          /////stops spear stopping when hitting objects on travel. 
                }
            }
            
            
            spear.AddComponent<Rigidbody2D>();
            Rigidbody2D rb = spear.GetComponent<Rigidbody2D>();
            spear.GetComponent<SpearItem>().pierceCount = 0;
            Vector2 force = new Vector2(20 * direction, 0);
            rb.AddForce(force, ForceMode2D.Impulse);
            spear.GetComponent<SpearItem>().isThrown = true;
            rb.gravityScale = 0;
            inventory.player.GetComponent<Actions>().attackAnimFinishedCallback -= ThrowSpear;
            inventory.RemoveItem(this);
            PlaySecondaryUseSound(throwSound);
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        
    }


    private void OnTriggerEnter2D(Collider2D collision)   //seems to work better as it pierces through objects. 
    {
        if (isThrown)
        {
            if (collision.TryGetComponent(out IDamageable damageable))
            {
                
                damageable.TakeDamage(ThrowDamage);
                GameObject impact = Instantiate(meleeWeaponData.impactAnimation, collision.transform.position, Quaternion.identity);
                Destroy(impact, 1.0f);
                pierceCount++;
                if (pierceCount >= maxPierces)
                {
                    Invoke("DelayedReturn", 0.1f);
                    return;
                    
                }
                if (damageable.HitPoints > 0)
                {
                    Rigidbody2D rb = GetComponent<Rigidbody2D>();
                    rb.velocity = Vector2.zero;
                    DisableCollidersForReturn();
                    returnToPlayer = true;
                    isThrown = false;
                    pierceCount = 0;
                    return; 
                    
                }

            }
        }
    }

    private void DelayedReturn()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        DisableCollidersForReturn();
        returnToPlayer = true;
        isThrown = false;
        pierceCount = 0;
    }

    private void DisableCollidersForReturn()
    {
        Collider2D[] cols = GetComponents<Collider2D>();
        foreach (Collider2D col in cols)
        {
            col.enabled = false;
        }
    }

    public void CheckDistance()
    {

        currentDistance = Vector2.Distance(startPos, transform.position);
        

        if (currentDistance > throwDistance && !returnToPlayer)
        {
            
            if(TryGetComponent(out Rigidbody2D rb))
            {
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0;
                Invoke("DisableCollidersForReturn", 0.2f);
                returnToPlayer = true;
            }
            
            
        }

    }

    public void ReturnToPlayer()
    {
        //move towards player
       
        Vector2 playerPos = inventory.player.transform.position;
        Vector2 spearPos = transform.position;
        Vector2 direction = playerPos - spearPos;
        float speed = 30.0f;
        transform.position = Vector2.MoveTowards(spearPos, playerPos, speed * Time.deltaTime);
        
        
        if (Vector2.Distance(playerPos, spearPos) < 0.4f)
        {
            returnToPlayer = false;
            GetComponent<WeaponPickupItem>().HandlePickup(inventory.player.GetComponent<Actions>(), inventory);
            Destroy(gameObject);
        }
    }

}
