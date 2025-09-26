using RhythmGameHelper.Client.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RhythmGameHelper.Client.Services
{
    public interface ISettingService
    {
        UserSetting UserSetting { get; }
        void LoadSettings();
        void SaveSettings();
    }

    public class SettingService : ISettingService
    {
        private readonly string _filePath;
        public UserSetting UserSetting { get; private set; } = null!;

        public SettingService()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string appFolder = Path.Combine(appDataPath, "RhythmGameHelper");
            Directory.CreateDirectory(appFolder);
            _filePath = Path.Combine(appFolder, "usersettings.json");

            LoadSettings();
        }

        public void LoadSettings()
        {
            if (File.Exists(_filePath))
            {
                try
                {
                    string json = File.ReadAllText(_filePath);
                    UserSetting = JsonSerializer.Deserialize<UserSetting>(json) ?? new ();
                }
                catch
                {
                    UserSetting = new ();
                }
            }
            else UserSetting = new ();
        }

        public void SaveSettings()
        {
            string json = JsonSerializer.Serialize(UserSetting, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }
    }
}
