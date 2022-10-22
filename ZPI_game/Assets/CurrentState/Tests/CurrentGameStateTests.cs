using NUnit.Framework;
using System;
using System.IO;
using System.Text.Json;

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
        [SetUp]
        public static void Init()
        {
            UserSettingsJson defUserSettings = new UserSettingsJson(true, 0.5f, false, 0.5f);
            string defSettingsJson = JsonSerializer.Serialize(defUserSettings);
            File.WriteAllText(CurrentGameState.JSON_FILE_NAME, defSettingsJson);
        }
        [Test, Order(1)]
        public static void GetUserSettingsTest()
        {
            var defUserSettingsTC = new TestCase(true, 0.5f, false, 0.5f);
            Assert.AreEqual(defUserSettingsTC.musicOn, CurrentGameState.IsMusicOn);
            Assert.AreEqual(defUserSettingsTC.musicVolume, CurrentGameState.MusicVolume);
            Assert.AreEqual(defUserSettingsTC.effectsOn, CurrentGameState.AreEffectsOn);
            Assert.AreEqual(defUserSettingsTC.effectsVolume, CurrentGameState.EffectsVolume);
        }
        [Test, Order(2)]
        public static void ChangeUserSettingsTest()
        {
            var userSettingsTC = new TestCase(false, 0.11f, true, 0.99f);
            CurrentGameState.IsMusicOn = userSettingsTC.musicOn;
            CurrentGameState.MusicVolume = userSettingsTC.musicVolume;
            CurrentGameState.AreEffectsOn = userSettingsTC.effectsOn;
            CurrentGameState.EffectsVolume = userSettingsTC.effectsVolume;
            string jsonFile = File.ReadAllText(CurrentGameState.JSON_FILE_NAME);
            UserSettingsJson savedFile = JsonSerializer.Deserialize<UserSettingsJson>(jsonFile);
            Assert.AreEqual(userSettingsTC.musicOn, savedFile.MusicOn);
            Assert.AreEqual(userSettingsTC.musicVolume, savedFile.MusicVolume);
            Assert.AreEqual(userSettingsTC.effectsOn, savedFile.EffectsOn);
            Assert.AreEqual(userSettingsTC.effectsVolume, savedFile.EffectsVolume);

            Assert.Throws<ArgumentOutOfRangeException>(() => CurrentGameState.MusicVolume = -1f);
            Assert.Throws<ArgumentOutOfRangeException>(() => CurrentGameState.MusicVolume = 3f);
            Assert.Throws<ArgumentOutOfRangeException>(() => CurrentGameState.EffectsVolume = -1f);
            Assert.Throws<ArgumentOutOfRangeException>(() => CurrentGameState.EffectsVolume = 5f);
        }
        [TearDown]
        public void Cleanup()
        {
            JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
            UserSettingsJson defUserSettings = new UserSettingsJson(true, 0.5f, false, 0.5f);
            string defSettingsJson = JsonSerializer.Serialize(defUserSettings, options);
            File.WriteAllText(CurrentGameState.JSON_FILE_NAME, defSettingsJson);
        }
    }
}
