using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    public Item itemData; // Reference to your ScriptableObject
    public int amount = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check that the player triggered this pickup
        if (other.CompareTag("Player"))
        {
            InventoryManager inventoryManager = FindObjectOfType<InventoryManager>();

            if (inventoryManager != null)
            {
                AddToInventory(inventoryManager);
                Destroy(gameObject); // Remove pickup object
            }
        }
    }

    private void AddToInventory(InventoryManager inventoryManager)
    {
        UISlotHandler[] slots = FindObjectsOfType<UISlotHandler>();

        foreach (UISlotHandler slot in slots)
        {
            // Stack if same item exists and it's stackable
            if (slot.item != null && slot.item.ItemID == itemData.ItemID && slot.item.isStackable)
            {
                inventoryManager.StackInInventory(slot, itemData);
                return;
            }
        }

        // Otherwise, find an empty slot
        foreach (UISlotHandler slot in slots)
        {
            if (slot.item == null)
            {
                Item newItem = itemData.Clone();
                newItem.itemCount = amount;
                inventoryManager.PlaceInInventory(slot, newItem);
                return;
            }
        }

        Debug.Log("Inventory is full!");
    }
}
