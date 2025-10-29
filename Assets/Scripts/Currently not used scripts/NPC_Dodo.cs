using UnityEngine;

public class NPC_Dodo : MonoBehaviour
{
    [Header("NPC Setup")]
    public string requiredItemID;                 // Item player must have
    public TextAsset dialogueJSON;                // Dialogue for this NPC
    public GameObject memoryMiniGameUI;          // Minigame Canvas

    private bool playerInRange = false;
    private bool dialogueTriggered = false;

    private void Awake()
    {
        // Subscribe early to avoid missing the event
        DialogueManager.GetInstance().OnDialogueComplete += OnDialogueFinished;
    }

    private void OnDestroy()
    {
        // Unsubscribe to avoid memory leaks
        if (DialogueManager.GetInstance() != null)
            DialogueManager.GetInstance().OnDialogueComplete -= OnDialogueFinished;
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            TryInteract();
        }
    }

    private void TryInteract()
    {
        if (dialogueTriggered) return;

        InventoryManager inventoryManager = FindObjectOfType<InventoryManager>();
        bool hasItem = inventoryManager != null && inventoryManager.HasItem(requiredItemID);

        Debug.Log("NPC Interaction: Checking for item '" + requiredItemID + "' => " + hasItem);

        if (hasItem)
        {
            dialogueTriggered = true;
            DialogueManager.GetInstance().EnterDialogueMode(dialogueJSON);
        }
        else
        {
            Debug.Log("Player doesn�t have the required item.");
        }
    }

    private void OnDialogueFinished()
    {
        if (dialogueTriggered)
        {
            StartMinigame();
        }
    }

    private void StartMinigame()
    {
        if (memoryMiniGameUI != null)
        {
            // Activate the UI
            memoryMiniGameUI.SetActive(true);

            // Try to find the CardController on the Canvas or its children
           // CardController cardController = memoryMiniGameUI.GetComponent<CardController>();
         //   if (cardController == null)
        //    {
        //        cardController = memoryMiniGameUI.GetComponentInChildren<CardController>();
//}
//
       //     if (cardController != null)
        //    {
                // Initialize the memory minigame
         //       cardController.InitializeMinigame();
        //    }
        //    else
         //   {
       //         Debug.LogWarning("No CardController found on Memory MiniGame UI or its children!");
     //       }
        }
        else
        {
            Debug.LogWarning("No Memory MiniGame UI assigned to NPC!");
        }
    }


    public void OnMinigameComplete(bool success)
    {
        if (memoryMiniGameUI != null)
        {
            memoryMiniGameUI.SetActive(false);
        }

        if (success)
        {
            Debug.Log("Minigame complete! Player wins.");
            // TODO: Reward player, trigger next dialogue, etc.
        }

        dialogueTriggered = false; // Allow the NPC to be triggered again if needed
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }
}
