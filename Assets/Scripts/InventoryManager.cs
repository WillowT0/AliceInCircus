using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public void StackInInventory(UISlotHandler currentSlot, Item item)
    {
        currentSlot.item.itemCount += item.itemCount;

        currentSlot.itemCountText.text = currentSlot.item.itemCount.ToString();
    }
    public void PlaceInInventory(UISlotHandler currentSlot, Item item)
    {
        currentSlot.item = item;
        currentSlot.icon.sprite = item.itemIcon;
        currentSlot.itemCountText.text = item.itemCount.ToString();
        currentSlot.icon.gameObject.SetActive(true);
    }
    public void ClearItemSlot(UISlotHandler currentSlot)
    {
        currentSlot.item = null;
        currentSlot.icon.sprite = null;
        currentSlot.itemCountText.text = string.Empty;
        currentSlot.icon.gameObject.SetActive(false);
    }

    public bool HasItem(string itemID)
    {
        UISlotHandler[] slots = FindObjectsOfType<UISlotHandler>();
        foreach (UISlotHandler slot in slots)
        {
            if (slot.item != null && slot.item.ItemID == itemID)
            {
                return true;
            }
        }
        return false;
    }

}
