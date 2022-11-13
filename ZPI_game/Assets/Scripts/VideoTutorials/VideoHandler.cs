using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoHandler : MonoBehaviour
{
    private VideoPlayer player;
    [SerializeField] private VideoClip _clip;
    private Slider _slider;

    [SerializeField] float slideDuration;
    void Start()
    {
        player = GetComponent<VideoPlayer>();
        player.clip = _clip;
        _slider = FindObjectOfType<Slider>();
        _slider.maxValue = (float) player.clip.length;
    }

    private void Update()
    {
        _slider.value = (float) player.time;
    }

    public void SetTime(float newTime)
    {
        player.time = newTime;
    }

    public void Reset()
    {
        player.time = 0;
        player.Play();
    }
    
    public void Next()
    {
        player.time += slideDuration;
    }

    public void Previous()
    {
        player.time -= slideDuration;
    }

    public void StartStop()
    {
        if (player.isPlaying == false)
        {
            player.Play();
        }
        else {
            player.Pause();
        }
    }
}
