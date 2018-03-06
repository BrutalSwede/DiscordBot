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

namespace DiscordBot.Settings
{

    //TODO: Rewrite with json instead of xml

    public class Settings
    {
        public bool PostRSS { get; set; }
        public bool TestBool { get; set; }
    }

    public static class SettingsController
    {
        public static async void Init()
        {
            await Task.Run(() => 
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Settings));

                serializer.UnknownNode += new XmlNodeEventHandler(serializer_UnknownNode);
                serializer.UnknownAttribute += new XmlAttributeEventHandler(serializer_UnknownAttribute);

                if (!File.Exists("settings.xml"))
                {
                    Console.WriteLine("Performing first time setup...");
                    Console.WriteLine("Creating settings file...");
                    FileStream stream = new FileStream("settings.xml", FileMode.Create);
                    Settings newSettings = new Settings()   {
                                                            PostRSS = false,
                                                            TestBool = true
                                                            };
                    serializer.Serialize(stream, newSettings);
                    stream.Close();
                }
                else
                {
                    Console.WriteLine("settings.xml already exists");
                }
            });
        }

        public static async Task GetDiscordKey()
        {
            
        }

        public static async Task<Settings> GetSettingsAsync()
        {
            return await Task.Run(()=> 
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Settings));

                serializer.UnknownNode += new XmlNodeEventHandler(serializer_UnknownNode);
                serializer.UnknownAttribute += new XmlAttributeEventHandler(serializer_UnknownAttribute);

                using (FileStream stream = new FileStream("settings.xml", FileMode.Open))
                {
                    Settings settings = (Settings)serializer.Deserialize(stream);
                    stream.Close();
                    return settings;
                }
            });
        }

        public static async void SaveSettings(Settings settings)
        {
            await Task.Run(()=> 
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Settings));

                using (FileStream stream = new FileStream("settings.xml", FileMode.Create))
                {
                    serializer.Serialize(stream, settings);
                    stream.Close();
                }
            });
        }
        
        static void serializer_UnknownNode(object sender, XmlNodeEventArgs e)
        {
            Console.WriteLine($"Unknown node in settings.xml: {e.Name} \t {e.Text}");
        }

        static void serializer_UnknownAttribute(object sender, XmlAttributeEventArgs e)
        {
            XmlAttribute xmlAtt = e.Attr;
            Console.WriteLine($"Unknown attribute in settings.xml: {xmlAtt.Name} = {xmlAtt.Value}");
        }
    }
}
