using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class InputPickupItem : BasePickupItem
{
    private bool playerInRange = false;
    private InputPickupItem nearestItem;
    private static List<InputPickupItem> itemsInRange = new();
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            playerInRange = true;
            itemsInRange.Add(this);
            Debug.Log("Player in range of " + itemData.name);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
            itemsInRange.Remove(this);
            Debug.Log("Player out of range of " + itemData.name);
        }
    }

    private void Update()
    {
        nearestItem = GetNearestItem();
        Debug.Log("Nearest item is" + nearestItem.itemData.name);
       
    }
    ///show UI of nearest item to pickup 
    ///

    private InputPickupItem GetNearestItem()
    {
        float nearestDistance = float.MaxValue;
        InputPickupItem nearestItem = null;
        Transform player = GameManager.instance.playerPos;
        foreach (var item in itemsInRange)
        {
            float distance = Vector2.Distance(player.position, item.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestItem = item;
            }
        }
        return nearestItem;
    }

}
