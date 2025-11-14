using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoToMenu : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string menuSceneName = "Main Menu";

    private void Start()
    {
        if (videoPlayer != null)
        {
            videoPlayer.Play();
            videoPlayer.loopPointReached += OnVideoEnd;
        }
        else
        {
            Debug.LogError("VideoPlayer is not assigned.");
        }
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        SceneManager.LoadScene(menuSceneName);
    }
}