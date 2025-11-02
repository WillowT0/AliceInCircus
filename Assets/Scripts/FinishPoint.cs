using UnityEngine;

public class FinishPoint : MonoBehaviour
{
    [Header("Required Item (Optional)")]
    [Tooltip("Leave empty if no specific item is required to finish the level.")]
    [SerializeField] private string requiredItemID;

    [Header("UI")]
    [Tooltip("Assign the confirmation UI Canvas here.")]
    [SerializeField] private GameObject confirmCanvas;

    private bool playerInRange = false;
    private bool canFinishLevel = false; // Replaces npcCheckDone
    private InventoryManager inventoryManager;

    private void Awake()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
        if (inventoryManager == null)
            Debug.LogWarning("No InventoryManager found in the scene!");

        if (confirmCanvas != null)
            confirmCanvas.SetActive(false); // Ensure canvas starts inactive
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            TryShowConfirmUI();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Press F to move to the next level!");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
            HideConfirmUI();
        }
    }

    private void TryShowConfirmUI()
    {
        if (!canFinishLevel)
        {
            Debug.Log("You can't move to the next level yet!");
            return;
        }

        // If an item is required, check it; otherwise, just show the confirm canvas
        if (string.IsNullOrEmpty(requiredItemID) ||
            (inventoryManager != null && inventoryManager.HasItem(requiredItemID)))
        {
            if (confirmCanvas != null)
                confirmCanvas.SetActive(true);
        }
        else
        {
            Debug.Log($"Player does not have the required item: {requiredItemID}");
        }
    }

    private void HideConfirmUI()
    {
        if (confirmCanvas != null)
            confirmCanvas.SetActive(false);
    }

    /// once the player meets the condition to finish the level.
   
    public void AllowNextLevel()
    {
        canFinishLevel = true;
        Debug.Log(" Player is now allowed to finish the level!");
    }


    public void ConfirmAdvance()
    {
        SceneController.instance.NextLevel1();
        HideConfirmUI();
    }

   
   
    public void CancelAdvance()
    {
        HideConfirmUI();
    }
}
