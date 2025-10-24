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
    private bool npcActive = true; // Flag to track if NPC is still active

    private void Awake()
    {
        playerInRange = false;
        if (visualCue != null)
            visualCue.SetActive(false);
    }

    private void Update()
    {
        // Only allow interaction if NPC is still active
        if (!npcActive) return;

        if (playerInRange && !DialogueManager.GetInstance().dialogueIsPlaying)
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
                        DialogueManager.GetInstance().EnterDialogueMode(dialogueIfHasItem);

                        // Hide NPC visual and visual cue after dialogue finishes
                        StartCoroutine(RemoveNPCAfterDialogue());
                    }
                    else
                    {
                        // Start dialogue for missing item
                        DialogueManager.GetInstance().EnterDialogueMode(dialogueIfMissingItem);
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
        while (DialogueManager.GetInstance().dialogueIsPlaying)
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
