using UnityEngine;
using UnityEngine.Video;

public class BootSequence : MonoBehaviour
{
    [SerializeField]
    private GameObject canvas;
    private VideoPlayer videoPlayer;

    [SerializeField]
    private float inactivityTime;
    private float timer;

    private bool playing = true;
    private bool playedOnce;

    private void Awake()
    {
        videoPlayer = GetComponentInChildren<VideoPlayer>();
        videoPlayer.loopPointReached += OnFinished;
    }

    private void Update()
    {
        /*if (MouseMoved() && playedOnce)
            ResetTimer();
        
        if (playing) return;
        
        timer += Time.deltaTime;
        
        if (timer >= inactivityTime)
            StartBootSequence();*/
    }

    private void StartBootSequence()
    {
        canvas.SetActive(true);
        playing = true;
    }

    private void ResetTimer()
    {
        timer = 0f;
        canvas.SetActive(false);
        playing = false;
        
        videoPlayer.Stop();
        videoPlayer.frame = 0;
        videoPlayer.Prepare();
    }

    private bool MouseMoved()
    {
        float deltaX = Input.GetAxisRaw("Mouse X");
        float deltaY = Input.GetAxisRaw("Mouse Y");

        return deltaX != 0 || deltaY != 0;
    }

    private void OnFinished(VideoPlayer player)
    {
        playing = false;
        AudioManager.Instance.Play("ost_main_loop");
        
        if (playedOnce) return;

        playedOnce = true;
        videoPlayer.isLooping = true;
        canvas.SetActive(false);
        timer = 0f;
    }
}
