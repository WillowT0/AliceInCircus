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
    [SerializeField] private TextAsset repeatDialogueBeforeCards; //  Repeat before cards
    [SerializeField] private TextAsset dialogueAfterCards;
    [SerializeField] private TextAsset dialogueAfterWin;
    [SerializeField] private TextAsset repeatDialogueAfterWin; //  Repeat after win

    [Header("Item Check")]
    [SerializeField] private string requiredItem = "cards";

    [Header("State Info (Debug)")]
    [SerializeField] private bool isActive = true;
    [SerializeField] private bool hasWonGame = false;

    private bool hasTalkedBeforeCards = false; //  Track repeat before cards
    private bool hasTalkedAfterWin = false;    //  Track repeat after win

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

    // Called by DialogueTrigger_Dodo
    public void Interact()
    {
        if (!isActive) return;

        //  After winning the memory game
        if (hasWonGame)
        {
            if (hasTalkedAfterWin && repeatDialogueAfterWin != null)
                dialogueManager.EnterDialogueMode(repeatDialogueAfterWin);
            else if (dialogueAfterWin != null)
            {
                dialogueManager.EnterDialogueMode(dialogueAfterWin);
                hasTalkedAfterWin = true;
            }
        }
        // Player has the required item (cards)
        else if (inventoryManager != null && inventoryManager.HasItem(requiredItem))
        {
            if (dialogueAfterCards != null)
            {
                dialogueManager.OnDialogueComplete += StartMemoryGame;
                dialogueManager.EnterDialogueMode(dialogueAfterCards);
            }
        }
        //  Player missing required item
        else
        {
            if (hasTalkedBeforeCards && repeatDialogueBeforeCards != null)
                dialogueManager.EnterDialogueMode(repeatDialogueBeforeCards);
            else if (dialogueBeforeCards != null)
            {
                dialogueManager.EnterDialogueMode(dialogueBeforeCards);
                hasTalkedBeforeCards = true;
            }
        }
    }

    public void StartMemoryGame()
    {
        dialogueManager.OnDialogueComplete -= StartMemoryGame;
        StartCoroutine(WaitForDialogueAndStartGame());
    }

    private IEnumerator WaitForDialogueAndStartGame()
    {
        while (dialogueManager.dialogueIsPlaying)
            yield return null;

        yield return StartCoroutine(StartMiniGameAfterDelay());
    }

    private IEnumerator StartMiniGameAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);

        if (memoryGameCanvas != null)
        {
            memoryGameCanvas.SetActive(true);
            GameManager gmInCanvas = memoryGameCanvas.GetComponentInChildren<GameManager>(true);
            if (gmInCanvas != null && !gmInCanvas.gameObject.activeSelf)
                gmInCanvas.gameObject.SetActive(true);
        }

        yield return null;

        GameManager memoryGame = FindObjectOfType<GameManager>();
        if (memoryGame != null)
        {
            memoryGame.OnGameWin += OnMemoryGameWin;
            Debug.Log("Dodo subscribed to memory game win event.");
        }
        else
        {
            Debug.LogWarning("GameManager not found in scene after enabling memory game!");
        }
    }

    private void OnMemoryGameWin()
    {
        Debug.Log("Dodo: Player won the memory game!");
        hasWonGame = true;

        if (memoryGameCanvas != null)
            memoryGameCanvas.SetActive(false);

        if (spriteRenderer != null && newSpriteAfterWin != null)
            spriteRenderer.sprite = newSpriteAfterWin;

        FinishPoint finishPoint = FindObjectOfType<FinishPoint>();
        if (finishPoint != null)
        {
            finishPoint.AllowNextLevel();
            Debug.Log("Dodo: Next level unlocked!");
        }
        else
        {
            Debug.LogWarning("No FinishPoint found in the scene!");
        }

        GameManager mg = FindObjectOfType<GameManager>();
        if (mg != null)
            mg.OnGameWin -= OnMemoryGameWin;

        // Play first after-win dialogue automatically
        if (dialogueAfterWin != null)
        {
            dialogueManager.EnterDialogueMode(dialogueAfterWin);
            hasTalkedAfterWin = true;
        }
        else
        {
            Debug.Log("Dodo: Thanks for playing! (No after-win dialogue assigned)");
        }
    }
}
