using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Cryo.Script;
using CurrentState;
using LevelUtils;
using UnityEngine;
using UnityEngine.UI;

namespace Cutscenes.SpecificCutscenes
{
    public class IntroductionRegrets: MonoBehaviour, ICutscenePlayable
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip audioClip;
        [SerializeField] private Image fader;
        [SerializeField] private Text text;
        [SerializeField] private GameObject chatPanel;
        [SerializeField] private CryoUI cryo;
        private List<string> textLines;
        private int currentIndex = 0;

        private void Awake()
        {
            var allText = Resources.Load<TextAsset>($"Cutscenes/conversation").text;
            textLines = CutsceneFileReader.ReadLines(allText);
            text.text = "";
            text.color = Color.black;
            fader.gameObject.SetActive(true);
            cryo.gameObject.SetActive(false);
            chatPanel.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                NextLineOnClick();
            }
        }

        private void NextLineOnClick()
        {
            if (currentIndex >= textLines.Count) return;
            var who = CutsceneLineParser.WhoSays(textLines[currentIndex]);
            var textLine = CutsceneLineParser.GetCharacterLine(textLines[currentIndex]);
            if (who != "C")
            {
                cryo.ShowDialogBox(false);
                chatPanel.SetActive(true);
                text.text = textLine;
            }
            else
            {
                if (!cryo.gameObject.activeSelf) cryo.gameObject.SetActive(true);
                chatPanel.SetActive(false);
                CutsceneLineParser.SetupCryo(ref cryo, textLines[currentIndex]);
                cryo.Say(textLine);
            }
            currentIndex += 1;
        }

        public void SaveCutscene()
        {
            var levelMap = FindObjectOfType<LevelMap>();
            var currentState = FindObjectOfType<CurrentGameState>();
            levelMap.CompleteALevel(currentState.CurrentLevelName, currentState.CurrentSlot);
        }

        public IEnumerator Play()
        {
            audioSource.clip = audioClip;
            fader.CrossFadeAlpha(0, 2, true);
            yield return new WaitForSeconds(0.5f);
            audioSource.Play();
            yield return new WaitForSeconds(1.5f);
            NextLineOnClick();

            while(currentIndex != textLines.Count)
            {
                yield return null;
            }
            SaveCutscene();
            yield return new WaitForSeconds(3f);

            //HumanSay();
            //yield return new WaitForSeconds(8f);
            //HumanSay();
            //yield return new WaitForSeconds(8f);
            //HumanSay();
            //yield return new WaitForSeconds(8f);
            //HumanSay();
            //yield return new WaitForSeconds(8f);
            //HumanSay();
            //yield return new WaitForSeconds(8f);
            //cryo.SetBothEyesTypes(EyeType.Angry);
            //cryo.SetMouthType(MouthType.Angry);
            //cryo.ShowDialogBox(false);
            //cryo.gameObject.SetActive(true);
            //CryoSay();
            //yield return new WaitForSeconds(4f);
            //HumanSay();
            //yield return new WaitForSeconds(3f); // Huh, what is this!
            //CryoSay();
            //yield return new WaitForSeconds(6f);
            //HumanSay();
            //yield return new WaitForSeconds(6f);
            //cryo.SetBothEyesTypes(EyeType.Sad);
            //cryo.SetMouthType(MouthType.Confused);
            //CryoSay();
            //yield return new WaitForSeconds(2f);
            //HumanSay();
            //yield return new WaitForSeconds(6f);
            //cryo.SetBothEyesTypes(EyeType.Eye);
            //CryoSay(); // oh
            //yield return new WaitForSeconds(2f);
            //CryoSay();
            //yield return new WaitForSeconds(6f);
            //HumanSay();
            //yield return new WaitForSeconds(5f); // I could use a hand
            //cryo.SetBothEyesTypes(EyeType.Eye);
            //cryo.SetMouthType(MouthType.Smile);
            //CryoSay();
            //yield return new WaitForSeconds(7f);
            //HumanSay();
            //yield return new WaitForSeconds(2f);
            //CryoSay();
            //yield return new WaitForSeconds(6f);
            //cryo.SetBothEyesTypes(EyeType.EyeSmall);
            //cryo.SetMouthType(MouthType.Smile);
            //CryoSay();
            //yield return new WaitForSeconds(5f);
            //HumanSay();
            //yield return new WaitForSeconds(1f);
            //cryo.SetRightEyeType(EyeType.Wink);
            //cryo.SetLeftEyeType(EyeType.Eye);
            //cryo.SetMouthType(MouthType.Smile);
            //CryoSay();
            //yield return new WaitForSeconds(3f);
        }
    }
}