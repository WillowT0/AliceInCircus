using UnityEngine;

public class FinishPoint : MonoBehaviour
{
    [Header("Required Item")]
    [SerializeField] private string requiredItemID; // The ItemID that player must have

    private bool playerInRange = false;
    private InventoryManager inventoryManager;

    private void Awake()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
        if (inventoryManager == null)
        {
            Debug.LogWarning("No InventoryManager found in the scene!");
        }
    }

    private void Update()
    {
        if (playerInRange && inventoryManager != null)
        {
            // Player presses the confirm key (U)
            if (Input.GetKeyDown(KeyCode.U))
            {
                TryAdvanceLevel();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Press U to move to the next level!");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    private void TryAdvanceLevel()
    {
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
}
