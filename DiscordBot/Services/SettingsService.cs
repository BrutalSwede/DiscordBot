using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using Newtonsoft.Json;

namespace DiscordBot.SettingsService
{

    //TODO: Rewrite with json instead of xml

    public class Settings
    {
        public bool PostRSS { get; set; }
        public bool TestBool { get; set; }
    }

    internal class BotToken
    {
        public String Token { get; set; }
    }

    public static class SettingsController
    {
        public static async void Init()
        {
            await Task.Run(() =>
            {

                if (!File.Exists("settings.json"))
                {
                    Console.WriteLine("Performing first time setup...");
                    Console.WriteLine("Creating settings file...");
                    Settings newSettings = new Settings()
                    {
                        PostRSS = false,
                        TestBool = true
                    };
                    
                    using (FileStream stream = new FileStream("settings.json", FileMode.Create))
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        JsonSerializer serializer = new JsonSerializer();

                        serializer.Serialize(writer, newSettings);
                    }
                }
                else
                {
                    Console.WriteLine("settings.json already exists");
                }
            });
        }

        public static async Task<String> GetBotTokenFromJson()
        {
            BotToken tokenObject = null;
            Console.WriteLine("Attempting to load token from file");
            if (!File.Exists("config.json"))
            {
                Console.WriteLine("config.json is missing in root");
            }
            else
            {
                using (StreamReader stream = new StreamReader("config.json"))
                {
                    try
                    {
                        tokenObject = JsonConvert.DeserializeObject<BotToken>(await stream.ReadToEndAsync());
                    }
                    catch (JsonSerializationException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }

            if (tokenObject == null)
                throw new JsonSerializationException();

            return tokenObject.Token;
        }

        public static async Task<Settings> GetSettingsAsync()
        {
            return await Task.Run(() =>
            {
                using (StreamReader stream = new StreamReader("settings.json"))
                {
                    string jsonString = stream.ReadToEnd();
                    Settings settings = JsonConvert.DeserializeObject<Settings>(jsonString);
                    return settings;
                }
            });
        }

        public static async void SaveSettings(Settings settings)
        {
            await Task.Run(() =>
            {
                using (StreamWriter stream = new StreamWriter("settings.json"))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(stream, settings);
                }
            });
        }
    }
}
