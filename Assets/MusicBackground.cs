using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundMusic : MonoBehaviour
{
    public static BackgroundMusic instance;
    public AudioSource audioSource;

    [Header("Konfiguracja")]
    public string[] musicScenes;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

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
        CheckMusic(scene.name);
    }

    private void CheckMusic(string sceneName)
    {
        bool shouldPlay = false;

        foreach (string name in musicScenes)
        {
            if (name == sceneName)
            {
                shouldPlay = true;
                break;
            }
        }

        if (shouldPlay)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {

            audioSource.Stop();
        }
    }
}