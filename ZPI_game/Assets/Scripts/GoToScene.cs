using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToScene : MonoBehaviour
{
    [SerializeField] public string scene;
    [SerializeField] AudioClip clip;
    public void GoTo()
    {
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

    IEnumerator PlaySoundAndGoTo(AudioSource source)
    {
        source.PlayOneShot(clip);
        yield return new WaitWhile(() => source.isPlaying);
        GoTo();
    }
}
