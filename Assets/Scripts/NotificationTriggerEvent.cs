using System.Collections;
using UnityEngine;
using TMPro;

public class NotificationTriggerEvent : MonoBehaviour
{
    [Header("UI Content")]
    [SerializeField] private TextMeshProUGUI notificationTextUI;
    [SerializeField] private string notificationMessage = "Default notification text";

    [Header("Message Customisation")]
    [SerializeField] private bool removeAfterExit = false;
    [SerializeField] private bool disableAfterTimer = false;
    [SerializeField] private float disableTimer = 5.0f;

    [Header("Notification Animation")]
    [SerializeField] private Animator notificationAnim;

    private BoxCollider2D objectCollider;

    private void Awake()
    {
        objectCollider = GetComponent<BoxCollider2D>();
        if (objectCollider == null)
            Debug.LogWarning("No BoxCollider2D found on " + gameObject.name);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered notification trigger: " + gameObject.name);
            StartCoroutine(EnableNotification());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && removeAfterExit)
        {
            Debug.Log(" Player exited notification trigger");
            RemoveNotification();
        }
    }

    private IEnumerator EnableNotification()
    {
        if (objectCollider != null)
            objectCollider.enabled = false; // prevent retriggering

        if (notificationAnim != null)
            notificationAnim.Play("NotificationFadeIn");

        if (notificationTextUI != null)
            notificationTextUI.text = notificationMessage;

        if (disableAfterTimer)
        {
            yield return new WaitForSeconds(disableTimer);
            RemoveNotification();
        }
    }

    private void RemoveNotification()
    {
        if (notificationAnim != null)
            notificationAnim.Play("NotificationFadeOut");

        // deactivate trigger if one-time use
        gameObject.SetActive(false);
    }
}
