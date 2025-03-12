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


    public void ChangeItem(Component sender, object data)
    {
        Debug.Log("Change Item event received");
        
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