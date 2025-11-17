using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PointerController : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public RectTransform safeZone;
    public float moveSpeed = 100f;

    private bool qteActive = false;
    private RectTransform pointerTransform;
    private Vector3 targetPosition;

    void Start()
    {
        pointerTransform = GetComponent<RectTransform>();
        targetPosition = pointB.position;
    }

    void Update()
    {
        if (!qteActive) return;

        // Move pointer toward target
        pointerTransform.position =
            Vector3.MoveTowards(pointerTransform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Reverse direction when reaching end points
        if (Vector3.Distance(pointerTransform.position, pointA.position) < 0.1f)
            targetPosition = pointB.position;
        else if (Vector3.Distance(pointerTransform.position, pointB.position) < 0.1f)
            targetPosition = pointA.position;

        // Check input
        if (Input.GetKeyDown(KeyCode.Space))
            CheckSuccess();
    }

    // Start the QTE (called from trigger)
    public void StartQTE()
    {
        qteActive = true;
    }

    private void CheckSuccess()
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(safeZone, pointerTransform.position, null))
        {
            Debug.Log("QTE Success!");
            EndQTE(true);
        }
        else
        {
            Debug.Log("QTE Fail!");
            EndQTE(false);
        }
    }

    private void EndQTE(bool success)
    {
        qteActive = false;

        if (success)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            // Wait before restarting the QTE
            StartCoroutine(RestartQTEAfterDelay(1f));
        }
    }

    private IEnumerator RestartQTEAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ResetQTE();
    }

    private void ResetQTE()
    {
        // Reset pointer to start
        pointerTransform.position = pointA.position;

        // Set direction toward pointB
        targetPosition = pointB.position;

        // Start QTE again
        qteActive = true;
    }
}
