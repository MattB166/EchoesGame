

using System.Collections.Generic;

[System.Serializable]
public class InventoryEventData
{
    public InventoryItem currentItem;
    public int currentIndex;
    public List<InventoryItem> items;

    public InventoryEventData(InventoryItem currentItem, int currentIndex, List<InventoryItem> items)
    {
        this.currentItem = currentItem;
        this.currentIndex = currentIndex;
        this.items = items;
    }

}
