using DeveloperUtils;
using LevelUtils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoHandler : MonoBehaviour
{
    private VideoPlayer player;
    [SerializeField] private VideoClip _clip;
    [SerializeField] private Button finishWatching;
    private Slider _slider;

    [SerializeField] float slideDuration;
    void Start()
    {
        finishWatching.gameObject.SetActive(false);
        var sceneName = SceneManager.GetActiveScene().name;
        if(LevelMap.Instance.IsLevelDone(sceneName, CurrentState.CurrentGameState.Instance.CurrentSlot))
        {
            finishWatching.gameObject.SetActive(true);
        }
        player = GetComponent<VideoPlayer>();
        player.clip = _clip;
        _slider = FindObjectOfType<Slider>();
        _slider.maxValue = (float) player.clip.length;
    }

    private void Update()
    {
        _slider.value = (float) player.time;
        if(player.time + 5 > _clip.length)
        {
            finishWatching.gameObject.SetActive(true);
        }
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

    public void FinishWatching()
    {
        var sceneName = SceneManager.GetActiveScene().name;
        LevelMap.Instance.CompleteALevel(sceneName, CurrentState.CurrentGameState.Instance.CurrentSlot);
    }
}
