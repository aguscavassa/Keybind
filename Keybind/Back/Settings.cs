using Keybind.Front;
using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Xaml.Media;
using System;
using System.Globalization;
using System.IO;
using System.Text.Json;

namespace Keybind.Back
{
    public static class SettingsManagement
    {
        public static readonly CultureInfo[] Languages = new CultureInfo[]
            {
                    CultureInfo.CreateSpecificCulture("en"),
                    CultureInfo.CreateSpecificCulture("es")
            };
        public class Settings
        {
            public int ActiveLanguage { get; set; }
            public bool UseMicaEffectsSetting { get; set; }

            [NonSerialized] public string[] LanguagesAsNames;

            public Settings()
            {
                ActiveLanguage = 0;
                UseMicaEffectsSetting = true;
            }

            public Settings(int activeLanguage, bool useMicaEffectsSetting)
            {
                ActiveLanguage = activeLanguage;
                UseMicaEffectsSetting = useMicaEffectsSetting;
            }
        }

        private static Settings currentSettings;
        public static Settings CurrentSettings { get => currentSettings; set => currentSettings = value; }

        // Tries to get a Settings json object from the disk, if not, creates and returns a new one.
        public static Settings GetSettingsFromDisk()
        {
            try
            {
                string Text = File.ReadAllText($"{Lifecycle.UserDir}\\settings.json");
                CurrentSettings = JsonSerializer.Deserialize<Settings>(Text);
            }
            catch
            {
                CurrentSettings = new Settings();
                File.WriteAllText($"{Lifecycle.UserDir}\\settings.json", JsonSerializer.Serialize(CurrentSettings));
            }
            CurrentSettings.LanguagesAsNames = GetCulturesAsNames();
            return CurrentSettings;
        }

        public static Settings GetSettings()
        {
            return CurrentSettings ?? GetSettingsFromDisk();
        }

        public static void SetSettings(Settings settings)
        {
            CurrentSettings = settings;
            Lifecycle.SetLanguage(CurrentSettings.ActiveLanguage);
            App.GetMainWindow().SystemBackdrop = (CurrentSettings.UseMicaEffectsSetting ? new MicaBackdrop(){ Kind = MicaKind.BaseAlt} : null);
            App.GetMainWindow().NavigateDefault(typeof(MainView), null);
            File.WriteAllText($"{Lifecycle.UserDir}\\settings.json", JsonSerializer.Serialize(CurrentSettings));
        }

        public static string[] GetCulturesAsNames()
        {
            string[] culturesAsNames = new string[Languages.Length];
            for (int i = 0; i < Languages.Length; i++)
            {
                culturesAsNames[i] = Languages[i].Name;
            }
            return culturesAsNames;
        }
    }
}
