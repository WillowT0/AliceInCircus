using System.Collections;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    [Header("Dialogue Files (Has Item)")]
    [SerializeField] private TextAsset firstDialogueHasItem;
    [SerializeField] private TextAsset repeatDialogueHasItem;

    [Header("Dialogue Files (Missing Item)")]
    [SerializeField] private TextAsset firstDialogueMissingItem;
    [SerializeField] private TextAsset repeatDialogueMissingItem;

    [Header("Item Check")]
    [SerializeField] private string itemToCheck;

    [Header("NPC Visual")]
    [SerializeField] private GameObject npcVisual;

    [Header("Finish Point Reference")]
    [SerializeField] private FinishPoint finishPoint; // Assign in Inspector

    private bool hasTalkedWithItem = false;
    private bool hasTalkedWithoutItem = false;
    private bool hasUnlockedFinish = false;

    private bool playerInRange;
    private bool npcActive = true;

    private void Start()
    {
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
                StartDialogue();
            }
        }
        else
        {
            if (visualCue != null) visualCue.SetActive(false);
        }
    }

    private void StartDialogue()
    {
        InventoryManager inventoryManager = FindObjectOfType<InventoryManager>();
        if (inventoryManager == null)
        {
            Debug.LogWarning("InventoryManager not found in scene!");
            return;
        }

        bool hasItem = inventoryManager.HasItem(itemToCheck);
        TextAsset dialogueToPlay = null;

        // HAS ITEM
        if (hasItem)
        {
            if (!hasTalkedWithItem)
            {
                dialogueToPlay = firstDialogueHasItem;
                hasTalkedWithItem = true;
                StartCoroutine(UnlockAfterDialogue()); // Unlock finish point after first “has item” talk
            }
            else
            {
                dialogueToPlay = repeatDialogueHasItem;
            }
        }
        //  MISSING ITEM
        else
        {
            if (!hasTalkedWithoutItem)
            {
                dialogueToPlay = firstDialogueMissingItem;
                hasTalkedWithoutItem = true;
            }
            else
            {
                dialogueToPlay = repeatDialogueMissingItem;
            }
        }

        if (dialogueToPlay == null)
        {
            Debug.LogWarning($"{gameObject.name}: Missing dialogue file for this condition!");
            return;
        }

        DialogueManager.GetInstance().EnterDialogueMode(dialogueToPlay);
    }

    private IEnumerator UnlockAfterDialogue()
    {
        // Wait until the dialogue finishes
        while (DialogueManager.GetInstance() != null && DialogueManager.GetInstance().dialogueIsPlaying)
            yield return null;

        // Unlock the FinishPoint once dialogue ends
        if (!hasUnlockedFinish && finishPoint != null)
        {
            finishPoint.AllowNextLevel();
            hasUnlockedFinish = true;
            Debug.Log($"[{gameObject.name}] Player talked to me — FinishPoint unlocked!");
        }

        // Hide the NPC (only for the "has item" path)
        if (npcVisual != null)
            npcVisual.SetActive(false);

        if (visualCue != null)
            visualCue.SetActive(false);

        // Optionally mark NPC as inactive
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
