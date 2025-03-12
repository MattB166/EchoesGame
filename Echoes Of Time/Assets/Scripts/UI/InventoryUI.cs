using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    public float fadeDuration = 0.5f;
    public GameObject inventoryPanel;
    public InventorySlot centreSlot;
    public InventorySlot leftSlot;
    public InventorySlot rightSlot;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }


    public void ChangeItem(Component sender, object data) ///need to try and work out why any other event subscription doesnt work for this, as i want to pass 
        //in the whole inventory list and items, not just the current item.
    {
        Debug.Log("Change Item event received");
        if (data is object[] dataArray && dataArray.Length > 0)
        {
            data = dataArray[0];
            if (data is InventoryItem item)
            {
               StartCoroutine(ApplicationDelay(0.01f, item));
            }

        }
    }

    private IEnumerator ApplicationDelay(float delay, InventoryItem item)
    {
        yield return new WaitForSeconds(delay);
        centreSlot.UpdateSlot(item);
    }

    private void UpdatePanel(InventoryEventData data)
    {
       if(data.items != null && data.items.Count > 0)
        {
            centreSlot.UpdateSlot(data.items[data.currentIndex]);
            
            int prevIndex = (data.currentIndex - 1 + data.items.Count) % data.items.Count;
            leftSlot.UpdateSlot(data.items[prevIndex]);

            int nextIndex = (data.currentIndex + 1) % data.items.Count;
            rightSlot.UpdateSlot(data.items[nextIndex]);
        }

    }
}