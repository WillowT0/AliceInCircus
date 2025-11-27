using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject optionsPanel;

    [Header("Input")]
    public InputActionReference pauseAction; 

    private bool isPaused = false;

    private void OnEnable()
    {
        if (pauseAction != null) pauseAction.action.Enable();
    }

    private void OnDisable()
    {
        if (pauseAction != null) pauseAction.action.Disable();
    }

    private void Update()
    {

        if (pauseAction.action.WasPressedThisFrame())
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        optionsPanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        optionsPanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }
}