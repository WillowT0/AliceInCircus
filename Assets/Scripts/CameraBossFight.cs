using UnityEngine;
using Unity.Cinemachine;

public class CameraRiseTrigger : MonoBehaviour
{
    [Header("Player / Camera")]
    public Transform player;
    public CinemachineCamera vCam;
    public PlayerHealth playerHealth;

    private CinemachinePositionComposer composer;

    [Header("Rise Settings")]
    public Transform triggerPoint;
    public float upwardSpeed = 1f;
    public float maxRiseDistance = 10f;
    public float deathOffset = 1f;

    private bool rising = false;
    private float initialYOffset;

    [Header("UI Warning Indicator")]
    public GameObject risingIndicator;
    

    void Start()
    {
        // Get PositionComposer (new offset controller)
        composer = vCam.GetComponent<CinemachinePositionComposer>();

        if (composer == null)
        {
            Debug.LogError("CinemachinePositionComposer component missing! Add it to your Cinemachine Camera.");
            enabled = false;
            return;
        }

        initialYOffset = composer.TargetOffset.y;

        // Hide UI indicator at start
        if (risingIndicator != null)
            risingIndicator.SetActive(false);
    }

    void Update()
    {
        if (player == null || composer == null || playerHealth == null)
            return;

        // Start rising
        if (!rising && player.position.y >= triggerPoint.position.y)
            rising = true;

        // UI indicator
        if (risingIndicator != null)
        {
            if (rising && !risingIndicator.activeSelf)
                risingIndicator.SetActive(true);
        }

        

        // Move camera upward (modify the PositionComposer offset)
        if (rising)
        {
            Vector3 offset = composer.TargetOffset;
            float newY = offset.y + upwardSpeed * Time.deltaTime;

            if (newY > initialYOffset + maxRiseDistance)
                newY = initialYOffset + maxRiseDistance;

            offset.y = newY;
            composer.TargetOffset = offset;
        }

        // Check if player falls below visible bottom of camera
        float camBottomY = vCam.transform.position.y - Camera.main.orthographicSize;

        if (player.position.y < camBottomY - deathOffset)
        {
            KillPlayer();
        }
    }

    private void KillPlayer()
    {
        if (playerHealth != null)
            playerHealth.TakeDamage(playerHealth.currentHealth);
    }
}
