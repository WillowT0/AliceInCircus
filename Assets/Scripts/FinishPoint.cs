using UnityEngine;

public class FinishPoint : MonoBehaviour
{
    [Header("Required Item")]
    [SerializeField] private string requiredItemID; // The ItemID player must have

    [Header("UI")]
    [SerializeField] private GameObject confirmCanvas; // Assign the confirmation UI Canvas here

    private bool playerInRange = false;
    private bool npcCheckDone = false; 

    private InventoryManager inventoryManager;

    private void Awake()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
        if (inventoryManager == null)
        {
            Debug.LogWarning("No InventoryManager found in the scene!");
        }

        if (confirmCanvas != null)
            confirmCanvas.SetActive(false); // Ensure canvas starts inactive
    }

    private void Update()
    {
        if (playerInRange && inventoryManager != null)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                TryShowConfirmUI();
            }
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
        if (!npcCheckDone)
        {
            Debug.Log("You must talk to the rabbit before moving to the next level!");
            return;
        }

        if (inventoryManager.HasItem(requiredItemID))
        {
            if (confirmCanvas != null)
                confirmCanvas.SetActive(true);
        }
        else
        {
            Debug.Log("Player does not have the required item!");
        }
    }

    private void HideConfirmUI()
    {
        if (confirmCanvas != null)
            confirmCanvas.SetActive(false);
    }

    // Called by NPC_CheckItem when player passes the check
    public void AllowNextLevel()
    {
        npcCheckDone = true;
        Debug.Log("Rabbit check complete — player can now finish the level!");
    }

    // Called from Yes button
    public void ConfirmAdvance()
    {
        SceneController.instance.NextLevel1();
        HideConfirmUI();
    }

    // Called from No button
    public void CancelAdvance()
    {
        HideConfirmUI();
    }
}
