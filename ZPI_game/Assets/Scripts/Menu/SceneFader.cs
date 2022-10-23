using System.Collections;
using System.Collections.Generic;
using CurrentState;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneFader : MonoBehaviour
{
    #region FIELDS
    [SerializeField] RawImage fadeImage;
    [SerializeField] float fadeSpeed;
    [SerializeField] private AudioSource source;
    [SerializeField] public bool fadeOutMusic;
    [SerializeField] public enum FadeDirection
    {
        In,
        Out,
    }
    #endregion

    #region SETUP
    private void Start()
    {
        fadeImage.gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        StartCoroutine(Fade(FadeDirection.In));
    }
    #endregion

    #region FADE
    private IEnumerator Fade(FadeDirection fadeDirection, bool withMusic = false)
    {
        fadeImage.gameObject.SetActive(true);
        float alpha = (fadeDirection == FadeDirection.In) ? 1 : 0;
        float fadeEndValue = 1 - alpha;

        if(fadeDirection == FadeDirection.In)
        {
            var maxVolume = source.volume;
            while(alpha >= fadeEndValue)
            {
                SetImageColor(ref alpha, fadeDirection);
                if (withMusic)
                {
                    source.volume = Mathf.Lerp(0, maxVolume, alpha);
                }
                yield return null;
            }
            fadeImage.enabled = false;
        }
        else
        {
            fadeImage.enabled = true;
            var maxVolume = CurrentGameState.MusicVolume;
            while(alpha <= fadeEndValue)
            {
                SetImageColor(ref alpha, fadeDirection);
                if (withMusic)
                {
                    source.volume = Mathf.Lerp(maxVolume, 0, alpha);
                }
                yield return null;
            }
        }
    }
    #endregion

    #region HELPERS
    public IEnumerator FadeAndLoadScene(FadeDirection fadeDirection, string sceneName, bool fadeAudio = false)
    {
        yield return Fade(fadeDirection, fadeAudio);
        SceneManager.LoadScene(sceneName);
    }
    private void SetImageColor(ref float alpha, FadeDirection fadeDirection)
    {
        fadeImage.color = new Color(
            fadeImage.color.r,
            fadeImage.color.g,
            fadeImage.color.b,
            alpha
            );
        alpha += Time.deltaTime * (1f / fadeSpeed) * ((fadeDirection == FadeDirection.In) ? -1 : 1);
    }
    #endregion
}
