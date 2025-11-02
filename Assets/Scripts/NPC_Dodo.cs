using System.Collections;
using UnityEngine;

public class NPC_DodoController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject npcVisual;
    [SerializeField] private Sprite newSpriteAfterWin;
    [SerializeField] private GameObject memoryGameCanvas;

    [Header("Dialogue Files")]
    [SerializeField] private TextAsset dialogueBeforeCards;
    [SerializeField] private TextAsset dialogueAfterCards;
    [SerializeField] private TextAsset dialogueAfterWin;

    [Header("Item Check")]
    [SerializeField] private string requiredItem = "cards";

    [Header("State Info (Debug)")]
    [SerializeField] private bool isActive = true;
    [SerializeField] private bool hasWonGame = false;

    private SpriteRenderer spriteRenderer;
    private DialogueManager dialogueManager;
    private InventoryManager inventoryManager;

    public bool IsActive => isActive;
    public bool HasWonGame => hasWonGame;

    private void Start()
    {
        spriteRenderer = npcVisual != null ? npcVisual.GetComponent<SpriteRenderer>() : null;
        dialogueManager = DialogueManager.GetInstance();
        inventoryManager = FindObjectOfType<InventoryManager>();

        if (memoryGameCanvas != null)
            memoryGameCanvas.SetActive(false);
    }

    // Call this from your DialogueTrigger or directly when pressing E
    public void Interact()
    {
        if (!isActive) return;

        if (hasWonGame)
        {
            // Already finished the game -> final dialogue
            if (dialogueAfterWin != null)
                dialogueManager.EnterDialogueMode(dialogueAfterWin);
        }
        else if (inventoryManager != null && inventoryManager.HasItem(requiredItem))
        {
            // Player has the “cards” -> start dialogue that leads to minigame
            if (dialogueAfterCards != null)
            {
                dialogueManager.OnDialogueComplete += StartMemoryGame;
                dialogueManager.EnterDialogueMode(dialogueAfterCards);
            }
        }
        else
        {
            // Missing item dialogue
            if (dialogueBeforeCards != null)
                dialogueManager.EnterDialogueMode(dialogueBeforeCards);
        }
    }

    public void StartMemoryGame()
    {
        dialogueManager.OnDialogueComplete -= StartMemoryGame;
        StartCoroutine(WaitForDialogueAndStartGame());
    }

    private IEnumerator WaitForDialogueAndStartGame()
    {
        // Wait until dialogue is finished
        while (dialogueManager.dialogueIsPlaying)
            yield return null;

        // Now start the memory game after dialogue ends
        yield return StartCoroutine(StartMiniGameAfterDelay());
    }

    private IEnumerator StartMiniGameAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);

        if (memoryGameCanvas != null)
        {
            memoryGameCanvas.SetActive(true);

            // Ensure GameManager exists and is active inside the canvas
            GameManager gmInCanvas = memoryGameCanvas.GetComponentInChildren<GameManager>(true);
            if (gmInCanvas != null && !gmInCanvas.gameObject.activeSelf)
                gmInCanvas.gameObject.SetActive(true);
        }

        // Wait one frame so Unity can register the enabled objects
        yield return null;

        GameManager memoryGame = FindObjectOfType<GameManager>();
        if (memoryGame != null)
        {
            memoryGame.OnGameWin += OnMemoryGameWin;
            Debug.Log(" Dodo subscribed to memory game win event.");
        }
        else
        {
            Debug.LogWarning(" GameManager not found in scene after enabling memory game!");
        }
    }

    private void OnMemoryGameWin()
    {
        Debug.Log("Dodo: Player won the memory game!");
        hasWonGame = true;

        if (memoryGameCanvas != null)
            memoryGameCanvas.SetActive(false);

        // Change sprite
        if (spriteRenderer != null && newSpriteAfterWin != null)
            spriteRenderer.sprite = newSpriteAfterWin;

        // Allow player to finish / unlock next level
        FinishPoint finishPoint = FindObjectOfType<FinishPoint>();
        if (finishPoint != null)
        {
            finishPoint.AllowNextLevel();
            Debug.Log(" Dodo: Next level unlocked!");
        }
        else
        {
            Debug.LogWarning(" No FinishPoint found in the scene!");
        }

        // Unsubscribe so it doesn’t double-trigger
        GameManager mg = FindObjectOfType<GameManager>();
        if (mg != null)
            mg.OnGameWin -= OnMemoryGameWin;

        // Optional dialogue after winning
        if (dialogueAfterWin != null)
            dialogueManager.EnterDialogueMode(dialogueAfterWin);
        else
            Debug.Log("Dodo: Thanks for playing! (No after-win dialogue assigned)");
    }
}
