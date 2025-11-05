using UnityEngine;


[RequireComponent(typeof(Collider2D))]
public class DialogueTrigger_Dodo : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    [Header("References")]
    [SerializeField] private NPC_DodoController dodoController;

    private bool playerInRange;

    private void Start()
    {
        if (visualCue != null)
            visualCue.SetActive(false);
    }

    private void Update()
    {
        if (dodoController == null || !dodoController.IsActive) return;

        if (playerInRange)
        {
            if (visualCue != null && !visualCue.activeSelf)
                visualCue.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (visualCue != null)
                    visualCue.SetActive(false);

                // Delegate all dialogue handling to the NPC controller
                dodoController.Interact();
            }
        }
        else
        {
            if (visualCue != null && visualCue.activeSelf)
                visualCue.SetActive(false);
        }
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
