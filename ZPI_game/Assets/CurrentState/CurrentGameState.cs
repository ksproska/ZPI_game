using System;
using System.IO;
using System.Text.Json;
using LevelUtils;

namespace CurrentState
{
    public static class CurrentGameState
    {
        public const string JSON_FILE_NAME = "Assets\\LevelUtils\\user_settings.json";
        public const string JSON_FILE_NAME_TESTS = "Assets\\LevelUtils\\Tests\\user_settings.json";
        private static bool _isMusicOn;
        private static float _musicVolume;
        private static bool _areEffectsOn;
        private static float _effectsVolume;
        private static UserSettingsJson GetUserSettings()
        {
            if (!File.Exists(JSON_FILE_NAME))
                throw new FileNotFoundException(JSON_FILE_NAME);
            string jsonFile = File.ReadAllText(JSON_FILE_NAME);
            return JsonSerializer.Deserialize<UserSettingsJson>(jsonFile);
        }
        private static void SaveUserSettings()
        {
            UserSettingsJson userSettings = new UserSettingsJson(_isMusicOn, _musicVolume, _areEffectsOn, _effectsVolume);
            JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
            string jsonStringUS = JsonSerializer.Serialize(userSettings, options);
            File.WriteAllText(JSON_FILE_NAME, jsonStringUS);
        }
        public static LoadSaveHelper.SlotNum CurrentSlot { get; set; }
        public static string CurrentLevelName { get; set; }
        public static bool IsMusicOn
        {
            get { return _isMusicOn; }
            set
            {
                _isMusicOn = value;
                SaveUserSettings();
            }
        }
        public static float MusicVolume
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
        public static bool AreEffectsOn
        {
            get { return _areEffectsOn; }
            set
            {
                _areEffectsOn = value;
                SaveUserSettings();
            }
        }
        public static float EffectsVolume
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
        static CurrentGameState()
        {
            UserSettingsJson userSettings = GetUserSettings();
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
