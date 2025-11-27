using System.Collections;
using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.SceneManagement;

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

    private bool canKill = false;


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        rising = false;
        canKill = false;

        if (composer != null)
        {
            Vector3 offset = composer.TargetOffset;
            offset.y = initialYOffset;
            composer.TargetOffset = offset;
        }

        if (risingIndicator != null)
            risingIndicator.SetActive(false);

        StartCoroutine(EnableKillDelay());
    }


    private void Start()
    {
        composer = vCam.GetComponent<CinemachinePositionComposer>();

        if (composer == null)
        {
            Debug.LogError("CinemachinePositionComposer component missing! Add it to your Cinemachine Camera.");
            enabled = false;
            return;
        }

        initialYOffset = composer.TargetOffset.y;

        if (risingIndicator != null)
            risingIndicator.SetActive(false);

        StartCoroutine(EnableKillDelay());
    }


    IEnumerator EnableKillDelay()
    {
        yield return new WaitForSeconds(0.5f);
        canKill = true;
    }


    void Update()
    {
        if (player == null || composer == null || playerHealth == null)
            return;

        if (!rising && player.position.y >= triggerPoint.position.y)
            rising = true;

        if (risingIndicator != null && rising)
            risingIndicator.SetActive(true);

        if (rising)
        {
            Vector3 offset = composer.TargetOffset;

            float newY = offset.y + upwardSpeed * Time.deltaTime;
            newY = Mathf.Min(newY, initialYOffset + maxRiseDistance);

            offset.y = newY;
            composer.TargetOffset = offset;
        }

        float camBottomY = vCam.transform.position.y - Camera.main.orthographicSize;

        if (player.position.y < camBottomY - deathOffset)
        {
            KillPlayer();
        }
    }


    private void KillPlayer()
    {
        if (!canKill) return;
        playerHealth.TakeDamage(playerHealth.currentHealth);
    }
}
