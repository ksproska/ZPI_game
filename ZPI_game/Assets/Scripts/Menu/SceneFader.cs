using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneFader : MonoBehaviour
{
    #region FIELDS
    [SerializeField] RawImage fadeImage;
    [SerializeField] float fadeSpeed;
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
    private IEnumerator Fade(FadeDirection fadeDirection)
    {
        fadeImage.gameObject.SetActive(true);
        float alpha = (fadeDirection == FadeDirection.In) ? 1 : 0;
        float fadeEndValue = 1 - alpha;

        if(fadeDirection == FadeDirection.In)
        {
            while(alpha >= fadeEndValue)
            {
                SetImageColor(ref alpha, fadeDirection);
                yield return null;
            }
            fadeImage.enabled = false;
        }
        else
        {
            fadeImage.enabled = true;
            while(alpha <= fadeEndValue)
            {
                SetImageColor(ref alpha, fadeDirection);
                yield return null;
            }
        }
    }
    #endregion

    #region HELPERS
    public IEnumerator FadeAndLoadScene(FadeDirection fadeDirection, string sceneName)
    {
        yield return Fade(fadeDirection);
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
