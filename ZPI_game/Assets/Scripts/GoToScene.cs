using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using CurrentState;
using DeveloperUtils;
using LevelUtils;

public class GoToScene : MonoBehaviour
{
    [SerializeField] public string scene;
    [SerializeField] AudioClip clip;
    [SerializeField] private AudioSource audioSource;
    public void GoTo()
    {
        CurrentGameState.Instance.CurrentLevelName = scene;
        SceneManager.LoadScene(scene);
    }

    public void GoToAfterSound()
    {
        AudioSource audioSource = Camera.main.gameObject.GetComponent<AudioSource>();
        if (audioSource == null) audioSource = this.audioSource;
        if (audioSource != null)
        {
            StartCoroutine(PlaySoundAndGoTo(audioSource));
            return;
        }
        GoTo();
    }

    public void GoToTitle(){
        CurrentGameState.Instance.CurrentSlot = LoadSaveHelper.SlotNum.First;
        GoTo();
    }

    public void FadeOutScene()
    {
        SceneFader fader = FindObjectOfType<SceneFader>(true);
        AudioSource audioSource = Camera.main.gameObject.GetComponent<AudioSource>();
        if (audioSource == null) audioSource = this.audioSource;
        StartCoroutine(PlaySoundAndFade(audioSource, fader));
    }

    public void PlaySound()
    {
        AudioSource source = Camera.main.gameObject.GetComponent<AudioSource>();
        if (source == null) source = audioSource;
        if (clip != null && source != null)
        {
            source.PlayOneShot(clip);
        }
    }

    IEnumerator PlaySoundAndGoTo(AudioSource source)
    {
        source.PlayOneShot(clip);
        yield return new WaitWhile(() => source.isPlaying);
        GoTo();
    }

    IEnumerator PlaySoundAndFade(AudioSource source, SceneFader fader)
    {
        CurrentGameState.Instance.CurrentLevelName = scene;
        if (clip != null && source != null)
            source.PlayOneShot(clip);
        return fader.FadeAndLoadScene(SceneFader.FadeDirection.Out, scene, fader.fadeOutMusic);
    }
}
