using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOM
{
    public class ConfigurationManager
    {
        private static ConfigurationManager _instance;

        private static readonly object _lock = new object();

        private Dictionary<string, string> _settings;

        private ConfigurationManager()
        {
            _settings = new Dictionary<string, string>();
        }

        public static ConfigurationManager GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new ConfigurationManager();
                    }
                }
            }
            return _instance;
        }

        public void LoadFromFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    var parts = line.Split('=');
                    if (parts.Length == 2)
                    {
                        _settings[parts[0].Trim()] = parts[1].Trim();
                    }
                }
            }
            else
            {
                throw new FileNotFoundException("Файл конфигурации не найден.");
            }
        }

        public void SaveToFile(string filePath)
        {
            using (var writer = new StreamWriter(filePath))
            {
                foreach (var setting in _settings)
                {
                    writer.WriteLine($"{setting.Key}={setting.Value}");
                }
            }
        }

        public string GetSetting(string key)
        {
            if (_settings.ContainsKey(key))
            {
                return _settings[key];
            }
            else
            {
                throw new KeyNotFoundException("Настройка не найдена.");
            }
        }

        public void SetSetting(string key, string value)
        {
            if (_settings.ContainsKey(key))
            {
                _settings[key] = value;
            }
            else
            {
                _settings.Add(key, value);
            }
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Parallel.Invoke(
                () => AccessConfigurationManager(),
                () => AccessConfigurationManager(),
                () => AccessConfigurationManager()
            );

            var configManager = ConfigurationManager.GetInstance();

            configManager.SetSetting("AppMode", "Production");
            configManager.SetSetting("Version", "1.0.0");

            configManager.SaveToFile("config.txt");

            var newConfigManager = ConfigurationManager.GetInstance();
            newConfigManager.LoadFromFile("config.txt");

            Console.WriteLine(newConfigManager.GetSetting("AppMode"));
            Console.WriteLine(newConfigManager.GetSetting("Version"));
        }

        static void AccessConfigurationManager()
        {
            var configManager = ConfigurationManager.GetInstance();
            Console.WriteLine($"Instance HashCode: {configManager.GetHashCode()}");
        }
    }
}