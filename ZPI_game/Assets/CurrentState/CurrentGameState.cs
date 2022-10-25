using System;
using System.IO;
using System.Text.Json;
using LevelUtils;
using UnityEngine;

namespace CurrentState
{
    public class CurrentGameState : MonoBehaviour
    {
        public const string JSON_FILE_NAME = "\\CurrentState\\user_settings.json";
        public const string JSON_FILE_NAME_TESTS = "Assets\\CurrentState\\Tests\\user_settings.json";
        private bool _isMusicOn;
        private float _musicVolume;
        private bool _areEffectsOn;
        private float _effectsVolume;
        private bool _isTestConfig = false;
        public static CurrentGameState Instance { get; set; }
        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            UserSettingsJson userSettings = GetUserSettings(Application.persistentDataPath + JSON_FILE_NAME);
            _isMusicOn = userSettings.MusicOn;
            _musicVolume = userSettings.MusicVolume;
            _areEffectsOn = userSettings.EffectsOn;
            _effectsVolume = userSettings.EffectsVolume;
        }
        private void CreateDefUsrSettFile(string configFilePath)
        {
            UserSettingsJson userSettings = new UserSettingsJson(true, 0.5f, true, 0.5f);
            JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
            string jsonStringUS = JsonSerializer.Serialize(userSettings, options);
            File.WriteAllText(configFilePath, jsonStringUS);
        }
        private UserSettingsJson GetUserSettings(string configFilePath)
        {
            new FileInfo(configFilePath).Directory.Create();
            if (!File.Exists(configFilePath))
                CreateDefUsrSettFile(configFilePath);
            string jsonFile = File.ReadAllText(configFilePath);
            return JsonSerializer.Deserialize<UserSettingsJson>(jsonFile);
        }
        private void SaveUserSettings()
        {
            UserSettingsJson userSettings = new UserSettingsJson(_isMusicOn, _musicVolume, _areEffectsOn, _effectsVolume);
            JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
            string jsonStringUS = JsonSerializer.Serialize(userSettings, options);
            if (_isTestConfig)
                File.WriteAllText(JSON_FILE_NAME_TESTS, jsonStringUS);
            else
                File.WriteAllText(Application.persistentDataPath + JSON_FILE_NAME, jsonStringUS);
        }
        public LoadSaveHelper.SlotNum CurrentSlot { get; set; }
        public string CurrentLevelName { get; set; }
        public bool IsMusicOn
        {
            get { return _isMusicOn; }
            set
            {
                _isMusicOn = value;
                SaveUserSettings();
            }
        }
        public float MusicVolume
        {
            get { return _musicVolume; }
            set
            {
                if(value < 0f || value > 1f)
                {
                    throw new ArgumentOutOfRangeException($"Music voulume ({value}) should be in the range <0,1>");
                }
                else
                {
                    _musicVolume = value;
                }
                SaveUserSettings();
            }
        }
        public bool AreEffectsOn
        {
            get { return _areEffectsOn; }
            set
            {
                _areEffectsOn = value;
                SaveUserSettings();
            }
        }
        public float EffectsVolume
        {
            get { return _effectsVolume; }
            set
            {
                if (value < 0f || value > 1f)
                {
                    throw new ArgumentOutOfRangeException($"Effects voulume ({value}) should be in the range <0,1>");
                }
                else
                {
                    _effectsVolume = value;
                }
                SaveUserSettings();
            }
        }
        public void LoadTestConfiguration()
        {
            _isTestConfig = true;
            UserSettingsJson userSettings = GetUserSettings(JSON_FILE_NAME_TESTS);
            _isMusicOn = userSettings.MusicOn;
            _musicVolume = userSettings.MusicVolume;
            _areEffectsOn = userSettings.EffectsOn;
            _effectsVolume = userSettings.EffectsVolume;
        }
    }
    public class UserSettingsJson
    {
        public bool MusicOn { get; set; }
        public float MusicVolume { get; set; }
        public bool EffectsOn { get; set; }
        public float EffectsVolume { get; set; }
        public UserSettingsJson(bool musicOn, float musicVolume, bool effectsOn, float effectsVolume)
        {
            MusicOn = musicOn;
            MusicVolume = musicVolume;
            EffectsOn = effectsOn;
            EffectsVolume = effectsVolume;
        }
    }
}
