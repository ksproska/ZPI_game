using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using CurrentState;
using LevelUtils;

public class GoToScene : MonoBehaviour
{
    [SerializeField] public string scene;
    [SerializeField] AudioClip clip;
    public void GoTo()
    {
        CurrentGameState.Instance.CurrentLevelName = scene;
        SceneManager.LoadScene(scene);
    }

    public void GoToAfterSound()
    {
        AudioSource audioSource = Camera.main.gameObject.GetComponent<AudioSource>();
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
        StartCoroutine(PlaySoundAndFade(audioSource, fader));
    }

    public void PlaySound()
    {
        AudioSource source = Camera.main.gameObject.GetComponent<AudioSource>();
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
        source.PlayOneShot(clip);
        return fader.FadeAndLoadScene(SceneFader.FadeDirection.Out, scene, fader.fadeOutMusic);
    }
}
