using System.Collections;
using UnityEngine;

/// <summary>
/// Handles Dodo-specific dialogue logic: 
/// checks inventory, triggers correct dialogue, 
/// and communicates with NPC_DodoController.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class DialogueTrigger_Dodo : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    [Header("Dialogue Files")]
    [SerializeField] private TextAsset dialogueBeforeCards;
    [SerializeField] private TextAsset dialogueAfterCards;
    [SerializeField] private TextAsset dialogueAfterWin;

    [Header("Item Check")]
    [SerializeField] private string itemToCheck = "cards";

    [Header("References")]
    [SerializeField] private NPC_DodoController dodoController;

    private bool playerInRange;
    private DialogueManager dialogueManager;
    private InventoryManager inventoryManager;

    private void Start()
    {
        dialogueManager = DialogueManager.GetInstance();
        inventoryManager = FindObjectOfType<InventoryManager>();

        if (visualCue != null)
            visualCue.SetActive(false);
    }

    private void Update()
    {
        if (dodoController == null || !dodoController.IsActive) return;
        if (dialogueManager == null) return;

        if (playerInRange && !dialogueManager.dialogueIsPlaying)
        {
            if (visualCue != null && !visualCue.activeSelf)
                visualCue.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (visualCue != null)
                    visualCue.SetActive(false);

                HandleDialogue();
            }
        }
        else
        {
            if (visualCue != null && visualCue.activeSelf)
                visualCue.SetActive(false);
        }
    }

    private void HandleDialogue()
    {
        if (dodoController.HasWonGame)
        {
            dialogueManager.EnterDialogueMode(dialogueAfterWin);
        }
        else if (inventoryManager != null && inventoryManager.HasItem(itemToCheck))
        {
            dialogueManager.EnterDialogueMode(dialogueAfterCards);
            dialogueManager.OnDialogueComplete += StartMemoryGame;
        }
        else
        {
            dialogueManager.EnterDialogueMode(dialogueBeforeCards);
        }
    }

    private void StartMemoryGame()
    {
        dialogueManager.OnDialogueComplete -= StartMemoryGame;
        dodoController.StartMemoryGame();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (visualCue != null)
                visualCue.SetActive(false);
        }
    }
}
