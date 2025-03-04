using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerSaveData
{
    ///Movement and actions data
    public Actions.Weapons currentWeaponIndex;
    public List<Actions.Weapons> availableWeaponsList;

    //inventory saving
    public List<InventoryItem> inventoryItems;
    public int currentInventoryItemIndex;

    //projectile saving
    public List<Projectiles> availableProjectiles;
    public int currentProjectileIndex;

    //bank saving. saves coin amount

    public PlayerSaveData(Actions.Weapons currentWeapon, List<Actions.Weapons> availableWeaponsList, List<InventoryItem> inventoryItems, int currentInventoryItemIndex, List<Projectiles> availableProjectiles, int currentProjectileIndex)
    {
        this.currentWeaponIndex = currentWeapon;
        this.availableWeaponsList = availableWeaponsList;
        this.inventoryItems = inventoryItems;
        this.currentInventoryItemIndex = currentInventoryItemIndex;
        this.availableProjectiles = availableProjectiles;
        this.currentProjectileIndex = currentProjectileIndex;
    }

}
