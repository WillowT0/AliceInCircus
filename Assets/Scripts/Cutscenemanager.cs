using System.Collections;
using System.Collections.Generic;
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
            Debug.LogError("VideoPlayer nie jest przypisany w Inspectorze.");
        }
    }

    private void VideoPlayer_loopPointReached(VideoPlayer source)
    {
        SceneManager.LoadScene("Level 0");
    }
}