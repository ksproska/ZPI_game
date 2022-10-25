using LevelUtils;
using NUnit.Framework;
using System;
using System.Collections;
using System.IO;
using System.Text.Json;
using UnityEngine;
using UnityEngine.TestTools;

namespace CurrentState
{
    public class CurrentGameStateTests
    {
        class TestCase
        {
            public readonly bool musicOn;
            public readonly float musicVolume;
            public readonly bool effectsOn;
            public readonly float effectsVolume;

            public TestCase(bool musicOn, float musicVolume, bool effectsOn, float effectsVolume)
            {
                this.musicOn = musicOn;
                this.musicVolume = musicVolume;
                this.effectsOn = effectsOn;
                this.effectsVolume = effectsVolume;
            }
        }
        private void SetUpLevelUtilsObjects()
        {
            GameObject go = new GameObject();
            CurrentGameState currentGameState = go.AddComponent<CurrentGameState>();
            CurrentGameState.Instance = currentGameState;
            currentGameState.LoadTestConfiguration();
        }

        [UnityTest, Order(1)]
        public IEnumerator GetUserSettingsTest()
        {
            SetUpLevelUtilsObjects();

            var defUserSettingsTC = new TestCase(true, 0.5f, false, 0.5f);
            Assert.AreEqual(defUserSettingsTC.musicOn, CurrentGameState.Instance.IsMusicOn);
            Assert.AreEqual(defUserSettingsTC.musicVolume, CurrentGameState.Instance.MusicVolume);
            Assert.AreEqual(defUserSettingsTC.effectsOn, CurrentGameState.Instance.AreEffectsOn);
            Assert.AreEqual(defUserSettingsTC.effectsVolume, CurrentGameState.Instance.EffectsVolume);
            yield return null;
        }
        [UnityTest, Order(2)]
        public IEnumerator ChangeUserSettingsTest()
        {
            // SetDefUserSettings();
            SetUpLevelUtilsObjects();

            var userSettingsTC = new TestCase(false, 0.11f, true, 0.99f);
            CurrentGameState.Instance.IsMusicOn = userSettingsTC.musicOn;
            CurrentGameState.Instance.MusicVolume = userSettingsTC.musicVolume;
            CurrentGameState.Instance.AreEffectsOn = userSettingsTC.effectsOn;
            CurrentGameState.Instance.EffectsVolume = userSettingsTC.effectsVolume;
            string jsonFile = File.ReadAllText(CurrentGameState.JSON_FILE_NAME_TESTS);
            UserSettingsJson savedFile = JsonSerializer.Deserialize<UserSettingsJson>(jsonFile);
            Assert.AreEqual(userSettingsTC.musicOn, savedFile.MusicOn);
            Assert.AreEqual(userSettingsTC.musicVolume, savedFile.MusicVolume);
            Assert.AreEqual(userSettingsTC.effectsOn, savedFile.EffectsOn);
            Assert.AreEqual(userSettingsTC.effectsVolume, savedFile.EffectsVolume);

            Assert.Throws<ArgumentOutOfRangeException>(() => CurrentGameState.Instance.MusicVolume = -1f);
            Assert.Throws<ArgumentOutOfRangeException>(() => CurrentGameState.Instance.MusicVolume = 3f);
            Assert.Throws<ArgumentOutOfRangeException>(() => CurrentGameState.Instance.EffectsVolume = -1f);
            Assert.Throws<ArgumentOutOfRangeException>(() => CurrentGameState.Instance.EffectsVolume = 5f);

            yield return null;

            UserSettingsJson defUserSettings = new UserSettingsJson(true, 0.5f, false, 0.5f);
            string defSettingsJson = JsonSerializer.Serialize(defUserSettings);
            File.WriteAllText(CurrentGameState.JSON_FILE_NAME_TESTS, defSettingsJson);
        }
        
    }
}
