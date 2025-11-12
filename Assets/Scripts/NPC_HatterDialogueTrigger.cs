using System.Collections;
using UnityEngine;

public class HatterDialogueTrigger : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    [Header("Dialogue Files (Safe Opened)")]
    [SerializeField] private TextAsset firstDialogueSafeOpened;
    [SerializeField] private TextAsset repeatDialogueSafeOpened;

    [Header("Dialogue Files (Safe Closed)")]
    [SerializeField] private TextAsset firstDialogueSafeClosed;
    [SerializeField] private TextAsset repeatDialogueSafeClosed;

    [Header("NPC Visual")]
    [SerializeField] private GameObject npcVisual;

    [Header("Finish Point Reference")]
    [SerializeField] private FinishPoint finishPoint; // Assign in Inspector

    private bool hasTalkedWithSafeOpened = false;
    private bool hasTalkedWithSafeClosed = false;
    private bool hasUnlockedFinish = false;

    private bool playerInRange = false;
    private bool npcActive = true;
    private bool lastSafeState = false;

    private void Start()
    {
        if (visualCue != null)
            visualCue.SetActive(false);

        if (npcVisual != null)
            npcVisual.SetActive(true);

        lastSafeState = Cat.isSafeOpened;
    }

    private void Update()
    {
        if (!npcActive) return;

        DialogueManager dialogueManager = DialogueManager.GetInstance();
        if (dialogueManager == null)
        {
            Debug.LogWarning("DialogueManager not found!");
            return;
        }

        // Show visual cue only when player is near and dialogue not active
        if (playerInRange && !dialogueManager.dialogueIsPlaying)
        {
            if (visualCue != null)
                visualCue.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
                StartDialogue();
        }
        else
        {
            if (visualCue != null)
                visualCue.SetActive(false);
        }

        // Automatically trigger dialogue when the safe opens
        if (playerInRange && Cat.isSafeOpened && !lastSafeState)
        {
            lastSafeState = true;
            AutoTriggerSafeOpenedDialogue();
        }
    }

    private void StartDialogue()
    {
        bool safeOpened = Cat.isSafeOpened;
        TextAsset dialogueToPlay = null;

        if (safeOpened)
        {
            if (!hasTalkedWithSafeOpened)
            {
                dialogueToPlay = firstDialogueSafeOpened;
                hasTalkedWithSafeOpened = true;
                StartCoroutine(UnlockAfterDialogue());
            }
            else
            {
                dialogueToPlay = repeatDialogueSafeOpened;
            }
        }
        else
        {
            if (!hasTalkedWithSafeClosed)
            {
                dialogueToPlay = firstDialogueSafeClosed;
                hasTalkedWithSafeClosed = true;
            }
            else
            {
                dialogueToPlay = repeatDialogueSafeClosed;
            }
        }

        if (dialogueToPlay == null)
        {
            Debug.LogWarning($"{gameObject.name}: Missing dialogue file for this condition!");
            return;
        }

        DialogueManager.GetInstance().EnterDialogueMode(dialogueToPlay);
    }

    private void AutoTriggerSafeOpenedDialogue()
    {
        // Auto-start first "safe opened" dialogue if not done yet
        if (!hasTalkedWithSafeOpened)
        {
            StartDialogue();
        }
    }

    private IEnumerator UnlockAfterDialogue()
    {
        // Wait for dialogue to finish
        while (DialogueManager.GetInstance() != null && DialogueManager.GetInstance().dialogueIsPlaying)
            yield return null;

        // Unlock FinishPoint after first “safe opened” conversation
        if (!hasUnlockedFinish && finishPoint != null)
        {
            finishPoint.AllowNextLevel();
            hasUnlockedFinish = true;
            Debug.Log($"[{gameObject.name}] FinishPoint unlocked after safe opened dialogue!");
        }

        // Keep NPC active and visible
        if (visualCue != null)
            visualCue.SetActive(false);

        // Do NOT disable NPC — player can still talk again later
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!npcActive) return;
        if (collider.CompareTag("Player"))
        {
            playerInRange = true;
            lastSafeState = Cat.isSafeOpened;

            // If player walks up after already opening the safe, auto play first “safe opened” dialogue
            if (Cat.isSafeOpened && !hasTalkedWithSafeOpened)
            {
                AutoTriggerSafeOpenedDialogue();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (!npcActive) return;
        if (collider.CompareTag("Player"))
            playerInRange = false;
    }
}
