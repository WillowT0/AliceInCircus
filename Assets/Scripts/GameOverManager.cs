using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [Header("Game Over UI")]
    public GameObject gameOverUI;

    private void Start()
    {
        if (gameOverUI != null)
            gameOverUI.SetActive(false);
    }

    public void ShowGameOver()
    {
        if (gameOverUI != null)
            gameOverUI.SetActive(true);

        Time.timeScale = 0f; // Pause the game
    }

    public void RetryGame()
    {
        Debug.Log("Retrying game...");

        // Ensure unpaused before reloading scene
        Time.timeScale = 1f;

        //  Reload scene cleanly
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }
}
