using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class IntroSceneManager : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;

    private void Start()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += VideoPlayer_loopPointReached;
        }
        else
        {
            Debug.LogError("VideoPlayer is not assigned in the Inspector.");
        }
    }

    private void VideoPlayer_loopPointReached(VideoPlayer source)
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentIndex + 1);
    }
}
