using System.Collections;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    [Header("Conditional Dialogue")]
    [SerializeField] private TextAsset dialogueIfHasItem;
    [SerializeField] private TextAsset dialogueIfMissingItem;

    [Header("Item Check")]
    [SerializeField] private string itemToCheck;

    [Header("NPC Visual")]
    [SerializeField] private GameObject npcVisual; // Assign the sprite or model here

    private bool playerInRange;
    private bool npcActive; // tracks if NPC can be interacted with

    private IEnumerator Start()
    {
        // Wait one frame to ensure DialogueManager exists
        yield return null;

        // Reset NPC state on scene load
        playerInRange = false;
        npcActive = true;

        if (visualCue != null)
            visualCue.SetActive(false);

        if (npcVisual != null)
            npcVisual.SetActive(true);
    }

    private void Update()
    {
        // Only allow interaction if NPC is active
        if (!npcActive) return;

        // Ensure DialogueManager exists
        DialogueManager dialogueManager = DialogueManager.GetInstance();
        if (dialogueManager == null)
        {
            Debug.LogWarning("DialogueManager not found or not initialized yet!");
            return;
        }

        // Handle player input when in range
        if (playerInRange && !dialogueManager.dialogueIsPlaying)
        {
            if (visualCue != null) visualCue.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                InventoryManager inventoryManager = FindObjectOfType<InventoryManager>();
                if (inventoryManager != null)
                {
                    if (inventoryManager.HasItem(itemToCheck))
                    {
                        // Hide visual cue immediately
                        if (visualCue != null) visualCue.SetActive(false);

                        // Start dialogue for having the item
                        dialogueManager.EnterDialogueMode(dialogueIfHasItem);

                        // Hide NPC visual and cue after dialogue finishes
                        StartCoroutine(RemoveNPCAfterDialogue());
                    }
                    else
                    {
                        // Start dialogue for missing item
                        dialogueManager.EnterDialogueMode(dialogueIfMissingItem);
                    }
                }
                else
                {
                    Debug.LogWarning("InventoryManager not found in scene!");
                }
            }
        }
        else
        {
            if (visualCue != null) visualCue.SetActive(false);
        }
    }

    private IEnumerator RemoveNPCAfterDialogue()
    {
        // Wait until the dialogue finishes
        while (DialogueManager.GetInstance() != null && DialogueManager.GetInstance().dialogueIsPlaying)
        {
            yield return null;
        }

        // Hide NPC visual
        if (npcVisual != null)
            npcVisual.SetActive(false);

        // Ensure visual cue is hidden
        if (visualCue != null)
            visualCue.SetActive(false);

        // Disable further interactions
        npcActive = false;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!npcActive) return;

        if (collider.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (!npcActive) return;

        if (collider.CompareTag("Player"))
            playerInRange = false;
    }
}
