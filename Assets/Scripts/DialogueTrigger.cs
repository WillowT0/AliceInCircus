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
    [SerializeField] private GameObject npcVisual;

    [Header("Finish Point Reference")]
    [SerializeField] private FinishPoint finishPoint; //  Assign in Inspector

    private bool playerInRange;
    private bool npcActive;
    private bool hasUnlockedFinish; //  Prevent double unlocks

    private IEnumerator Start()
    {
        yield return null;
        playerInRange = false;
        npcActive = true;

        if (visualCue != null)
            visualCue.SetActive(false);

        if (npcVisual != null)
            npcVisual.SetActive(true);
    }

    private void Update()
    {
        if (!npcActive) return;

        DialogueManager dialogueManager = DialogueManager.GetInstance();
        if (dialogueManager == null)
        {
            Debug.LogWarning("DialogueManager not found or not initialized yet!");
            return;
        }

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
                        // Hide visual cue
                        if (visualCue != null) visualCue.SetActive(false);

                        // Start dialogue for having the item
                        dialogueManager.EnterDialogueMode(dialogueIfHasItem);

                        // Wait for dialogue to finish, then unlock finish point
                        StartCoroutine(UnlockAfterDialogue());
                    }
                    else
                    {
                        // Dialogue when missing the item
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

    private IEnumerator UnlockAfterDialogue()
    {
        // Wait until the dialogue finishes
        while (DialogueManager.GetInstance() != null && DialogueManager.GetInstance().dialogueIsPlaying)
        {
            yield return null;
        }

        // Unlock the FinishPoint once dialogue ends
        if (!hasUnlockedFinish && finishPoint != null)
        {
            finishPoint.AllowNextLevel();
            hasUnlockedFinish = true;
            Debug.Log("[Rabbit] Player talked to me — FinishPoint unlocked!");
        }

        // Hide NPC
        if (npcVisual != null)
            npcVisual.SetActive(false);

        // hide the visual cue now
        if (visualCue != null)
            visualCue.SetActive(false);

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
