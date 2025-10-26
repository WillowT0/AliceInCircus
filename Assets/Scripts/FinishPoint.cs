using UnityEngine;

public class FinishPoint : MonoBehaviour
{
    [Header("Required Item")]
    [SerializeField] private string requiredItemID; // The ItemID that player must have

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            InventoryManager inventoryManager = FindObjectOfType<InventoryManager>();

            if (inventoryManager != null)
            {
                // Check if player has the required item
                if (inventoryManager.HasItem(requiredItemID))
                {
                    Debug.Log("Player has the item! Loading next level...");
                    SceneController.instance.NextLevel1();
                }
                else
                {
                    Debug.Log("Player does not have the required item!");
                }
            }
            else
            {
                Debug.LogWarning("No InventoryManager found in the scene!");
            }
        }
    }
}
