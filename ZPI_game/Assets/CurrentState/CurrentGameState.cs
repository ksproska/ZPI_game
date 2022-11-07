using System;
using System.IO;
using System.Text.Json;
using LevelUtils;
using UnityEngine;

namespace CurrentState
{
    public class CurrentGameState : MonoBehaviour
    {
        public const string JSON_FILE_NAME_TESTS = "Assets\\CurrentState\\Tests\\user_settings.json";
        public const int TRUE = 1;
        public const int FALSE = 0;
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
            CurrentUserId = -1;
            UserSettings userSettings = GetUserSettings();
            _isMusicOn = userSettings.MusicOn;
            _musicVolume = userSettings.MusicVolume;
            _areEffectsOn = userSettings.EffectsOn;
            _effectsVolume = userSettings.EffectsVolume;
        }
        private void CreateDefUsrSett()
        {
            PlayerPrefs.SetInt("MusicOn", TRUE);
            PlayerPrefs.SetFloat("MusicVolume", 0.5f);
            PlayerPrefs.SetInt("EffectsOn", TRUE);
            PlayerPrefs.SetFloat("EffectsVolume", 0.5f);
        }
        private void CreateDefUsrSettFile()
        {
            UserSettings userSettings = new UserSettings(true, 0.5f, true, 0.5f);
            JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
            string jsonStringUS = JsonSerializer.Serialize(userSettings, options);
            File.WriteAllText(JSON_FILE_NAME_TESTS, jsonStringUS);
        }
        private UserSettings GetUserSettingsForTests()
        {
            new FileInfo(JSON_FILE_NAME_TESTS).Directory.Create();
            if (!File.Exists(JSON_FILE_NAME_TESTS))
                CreateDefUsrSettFile();
            string jsonFile = File.ReadAllText(JSON_FILE_NAME_TESTS);
            return JsonSerializer.Deserialize<UserSettings>(jsonFile);
        }
        private UserSettings GetUserSettings()
        {
            
            if (!ArePlayerPrefsSet())
                CreateDefUsrSett();
            return new UserSettings (
                    PlayerPrefs.GetInt("MusicOn") == TRUE,
                    PlayerPrefs.GetFloat("MusicVolume"),
                    PlayerPrefs.GetInt("EffectsOn") == TRUE,
                    PlayerPrefs.GetFloat("EffectsVolume")
                );
        }
        private bool ArePlayerPrefsSet()
        {
            return PlayerPrefs.HasKey("MusicOn") && PlayerPrefs.HasKey("MusicVolume") && PlayerPrefs.HasKey("EffectsOn") && PlayerPrefs.HasKey("EffectsVolume");
        }
        private void SaveUserSettings()
        {
            UserSettings userSettings = new UserSettings(_isMusicOn, _musicVolume, _areEffectsOn, _effectsVolume);
            
            if (_isTestConfig)
            {
                JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
                string jsonStringUS = JsonSerializer.Serialize(userSettings, options);
                File.WriteAllText(JSON_FILE_NAME_TESTS, jsonStringUS);
            }  
            else
            {
                PlayerPrefs.SetInt("MusicOn", userSettings.MusicOn ? TRUE : FALSE);
                PlayerPrefs.SetFloat("MusicVolume", userSettings.MusicVolume);
                PlayerPrefs.SetInt("EffectsOn", userSettings.EffectsOn ? TRUE : FALSE);
                PlayerPrefs.SetFloat("EffectsVolume", userSettings.EffectsVolume);
            }
        }
        public LoadSaveHelper.SlotNum CurrentSlot { get; set; }
        public string CurrentLevelName { get; set; }
        public int CurrentUserId { get; set; }
        public string CurrentUserNickname { get; set; }
        
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
            UserSettings userSettings = GetUserSettingsForTests();
            _isMusicOn = userSettings.MusicOn;
            _musicVolume = userSettings.MusicVolume;
            _areEffectsOn = userSettings.EffectsOn;
            _effectsVolume = userSettings.EffectsVolume;
        }
    }
    public class UserSettings
    {
        public bool MusicOn { get; set; }
        public float MusicVolume { get; set; }
        public bool EffectsOn { get; set; }
        public float EffectsVolume { get; set; }
        public UserSettings(bool musicOn, float musicVolume, bool effectsOn, float effectsVolume)
        {
            MusicOn = musicOn;
            MusicVolume = musicVolume;
            EffectsOn = effectsOn;
            EffectsVolume = effectsVolume;
        }
    }
}
