using Newtonsoft.Json;
using System;
using System.IO;

namespace ToskersDiscordSkeleton.Services
{
    public class ConfigurationService
    {
        public string Token { get; set; }

        private string currentConfigPath;

        public static ConfigurationService Create(string configPath, string token)
        {
            ConfigurationService newConfig = new ConfigurationService();
            newConfig.Token = token;
            newConfig.currentConfigPath = configPath;
            newConfig.Save();
            return newConfig;
        }

        public static ConfigurationService Load(string configPath)
        {
            var path = configPath;
            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                var config = JsonConvert.DeserializeObject<ConfigurationService>(json);
                config.currentConfigPath = path;
                return config;
            }
            else
                throw new FileNotFoundException($"No configuration found in: {configPath}!");
        }

        public static ConfigurationService LoadOrDefault(string configPath, string token)
        {
            try
            {
                return Load(configPath);
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine($"Error reading from {configPath}. Message = {e.Message}");
                return Create(configPath, token);
            }
        }

        private void Save()
        {
            var json = JsonConvert.SerializeObject(this, Formatting.Indented);

            if (!string.IsNullOrEmpty(currentConfigPath))
                File.WriteAllText(currentConfigPath, json);
            else
                File.WriteAllText("config.json", json);
        }
    }
}