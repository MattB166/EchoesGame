using NUnit;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Projectiles
{
    public BaseProjectile projectile;
    public int ammoCount = 0;

    public Projectiles(BaseProjectile projectile, int ammoCount)
    {
        this.projectile = projectile;
        this.ammoCount = ammoCount;
    }
}
public class BowItem : WeaponItem
{
    public List<Projectiles> projectiles = new List<Projectiles>();
    public int currentProjectileIndex = 0;
    public Projectiles currentProjectile;

    //private List<Projectiles> projectilesToRemove = new List<Projectiles>();


    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        currentProjectileIndex = 0;
        if (currentProjectile == null && projectiles.Count > 0)
        {
            currentProjectile = projectiles[currentProjectileIndex];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void Use()
    {
        //fire the current projectile if there is any. 
        inventory.player.GetComponent<Actions>().attackAnimFinishedCallback -= Shoot;
        inventory.player.GetComponent<Actions>().attackAnimFinishedCallback += Shoot;

    }

    public override void Init(ItemData itemData, Inventory inv, GameObject prefab)
    {
        base.Init(itemData, inv, prefab);
        weaponData = itemData as WeaponData;
        Debug.Log("Bow initialized with " + weaponData.name);
        projectiles.Clear(); //need to actually add in the saved projectiles

        foreach (var p in inv.storedProjectiles)
        {
            ProjectileData projectileData = p.Key;
            int amount = p.Value;
            BaseProjectile projectile = projectileData.projectilePrefab.GetComponent<BaseProjectile>();
            AddProjectile(projectile, amount);
            //Debug.Log("Projectile added from inventory: " + projectileData.name + " with amount: " + amount);
        }

        inv.storedProjectiles.Clear();
    }

    public void Shoot(Actions.Weapons weapon)
    {
        if(weapon == Actions.Weapons.Bow)
        {
            projectiles.RemoveAll(p => p.ammoCount <= 0);

            if (currentProjectile == null || currentProjectile.ammoCount <= 0)
            {
                //Debug.Log("No projectiles left to fire.");
                return;
            }

            float direction = inventory.player.GetComponent<Movement>().direction;
            float xOffSet = 1.4f * direction;
            float xPos = inventory.player.transform.position.x + xOffSet;
            float yPos = inventory.player.transform.position.y;
            Vector2 pos = new Vector2(xPos, yPos);
            Quaternion rot = direction == 1 ? Quaternion.identity : Quaternion.Euler(0, 180, 0);
            GameObject projectile = Instantiate(currentProjectile.projectile.gameObject, pos, rot);
            BaseProjectile bp = projectile.GetComponent<BaseProjectile>();
            bp.InitialiseData(currentProjectile.projectile.projectileData);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            rb.gravityScale = 0;
            rb.velocity = new Vector2(currentProjectile.projectile.projectileData.projectileSpeed * direction, 0);




            bp.Fire(pos);
            currentProjectile.ammoCount--;
            //Debug.Log("Fired projectile: " + currentProjectile.projectile.projectileData.name + " Projectile now has: " + currentProjectile.ammoCount + " ammo.");
            //Debug.Log(projectiles.Count + " projectiles left.");
            if (currentProjectile.ammoCount <= 0)
            {
                RemoveProjectile(currentProjectile);
            }

        }



    }

    private void RemoveProjectile(Projectiles p)
    {
        int index = projectiles.IndexOf(p);
        if (index != -1)
        {
            projectiles.RemoveAt(index);

            if (projectiles.Count > 0)
            {
                currentProjectileIndex = 0;
                currentProjectile = projectiles[currentProjectileIndex];
                Debug.Log("Switched projectile due to previous one emptying.");
            }
            else
            {
                currentProjectile = null;
                Debug.Log("No more projectiles left.");
            }
        }
    }


    public override void SecondaryUse()
    {
        CycleProjectiles();
    }

    public void AddProjectile(BaseProjectile projectile, int ammoCount)
    {
        if (projectile == null || projectile.projectileData == null)
        {
            ///Debug.LogError("Projectile or projectileData is null! Cannot add to BowItem.");
            return;
        }

        for (int i = 0; i < projectiles.Count; i++)
        {
            if (projectiles[i].projectile.projectileData.projectileType == projectile.projectileData.projectileType)
            {
                projectiles[i].ammoCount += ammoCount;
                int newCount = projectiles[i].ammoCount;
                //Debug.Log("Added " + ammoCount + " ammo to existing projectile: " + projectile.projectileData.name + " now has: " + newCount + " ammo.");
                return;
            }
        }
        BaseProjectile bp = projectile;
        int count = ammoCount;
        Projectiles p = new Projectiles(bp, count);
        //Debug.Log("Added new projectile: " + projectile.projectileData.name + " with " + ammoCount + " ammo.");
        projectiles.Add(p);
        currentProjectile = p;
    }

    public void CycleProjectiles()
    {
        if (projectiles.Count > 1)
        {
            currentProjectileIndex = (currentProjectileIndex + 1) % projectiles.Count;
            currentProjectile = projectiles[currentProjectileIndex];
            Debug.Log("Switched to projectile: " + currentProjectile.projectile.projectileData.name);
            if (currentProjectile.ammoCount <= 0)
            {
                RemoveProjectile(currentProjectile);
            }
        }
        else
        {
            currentProjectileIndex = 0;
            currentProjectile = projectiles[currentProjectileIndex];
        }
    }


    public void ClearUpEmptyProjectiles()
    {
        //Debug.Log("Checking for empty projectiles");
        for (int i = projectiles.Count - 1; i >= 0; i--)
        {
            if (projectiles[i].ammoCount <= 0)
            {
                Debug.Log("Removing projectile: " + projectiles[i].projectile.projectileData.name);
                projectiles.RemoveAt(i);
            }
        }
    }
}
