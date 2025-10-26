using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Ink.Runtime;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;

    private Story currentStory;

    public bool dialogueIsPlaying { get; private set; }

    private static DialogueManager instance;

    // Event that other scripts can listen to
    public event Action OnDialogueComplete;

    private void Awake()
    {
        // Ensure only one instance exists, and replace any old ones after retry
        if (instance != null && instance != this)
        {
            Destroy(instance.gameObject);
        }

        instance = this;
        Debug.Log("DialogueManager initialized: " + gameObject.name);
    }

    public static DialogueManager GetInstance() => instance;

    private void Start()
    {
        dialogueIsPlaying = false;
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
        else
            Debug.LogError("DialogueManager: Dialogue Panel not assigned!");
    }

    private void Update()
    {
        if (!dialogueIsPlaying)
            return;

        //  Log when input is detected (for debugging)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("SPACE pressed while dialogue active");
            ContinueStory();
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        if (inkJSON == null)
        {
            Debug.LogError("DialogueManager: No Ink JSON assigned!");
            return;
        }

        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        ContinueStory();
    }

    private IEnumerator ExitDialogueMode()
    {
        yield return new WaitForSeconds(0.2f);
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";

        // Notify listeners that dialogue ended
        OnDialogueComplete?.Invoke();
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            dialogueText.text = currentStory.Continue();
        }
        else
        {
            StartCoroutine(ExitDialogueMode());
        }
    }
}
