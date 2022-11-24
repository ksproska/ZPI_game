using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Cutscenes.SpecificCutscenes
{
    public class IntroductionAtomBomb : MonoBehaviour, ICutscenePlayable
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private Image fader;
        [SerializeField] private AudioClip bombSound;

        private void Awake()
        {
            fader.canvasRenderer.SetAlpha(1);
        }

        private void Start()
        {
            audioSource.volume = CurrentState.CurrentGameState.Instance.EffectsVolume;
            if (!CurrentState.CurrentGameState.Instance.IsMusicOn)
            {
                audioSource.volume = 0;
            }
        }

        public IEnumerator Play()
        {
            audioSource.clip = bombSound;
            audioSource.Play();
            yield return new WaitForSeconds(4);
            fader.CrossFadeAlpha(0, 2, true);
            yield return new WaitForSeconds(2);
            fader.color = new Color(1, 1, 1, 0);
            fader.canvasRenderer.SetAlpha(1);
            var alpha = 0f;
            while (alpha < 1)
            {
                alpha += Time.deltaTime;
                fader.color = new Color(1, 1, 1, Mathf.Min(alpha, 1));
                yield return null;
            }
            // while (audioSource.volume > 0)
            // {
            //     audioSource.volume -= Time.deltaTime;
            //     yield return null;
            // }
            yield return new WaitForSeconds(4);
            audioSource.Stop();
        }
    }
}