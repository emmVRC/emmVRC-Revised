using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using emmVRC.Objects;
using UnityEngine;
using UnityEngine.UI;

namespace emmVRC
{
    public class Configuration
    {
        public static Config JSONConfig { get; private set; }
        public static List<KeyValuePair<string, Action>> onConfigUpdated;
        public static void Initialize()
        {
            onConfigUpdated = new List<KeyValuePair<string, Action>>();
            Directory.CreateDirectory(Path.Combine(System.Environment.CurrentDirectory, "UserData/emmVRC"));
            if (File.Exists(Path.Combine(System.Environment.CurrentDirectory, "UserData/emmVRC/config.json"))){
                //JSONConfig = JsonConvert.DeserializeObject<Config>(File.ReadAllText(Path.Combine(System.Environment.CurrentDirectory, "UserData/emmVRC/config.json")));
                try
                {
                    JSONConfig = TinyJSON.Decoder.Decode(File.ReadAllText(Path.Combine(System.Environment.CurrentDirectory, "UserData/emmVRC/config.json"))).Make<Config>();
                    if (JSONConfig == null)
                    {
                        emmVRCLoader.Logger.LogError("An error occured while parsing the config file. It has been moved to config.old.json, and a new one has been created.");
                        emmVRCLoader.Logger.LogError("Error: The parsed config was null...");
                        File.Move(Path.Combine(System.Environment.CurrentDirectory, "UserData/emmVRC/config.json"), Path.Combine(System.Environment.CurrentDirectory, "UserData/emmVRC/config.old.json"));
                        JSONConfig = new Config();
                    }
                } catch (System.Exception ex)
                {
                    emmVRCLoader.Logger.LogError("An error occured while parsing the config file. It has been moved to config.old.json, and a new one has been created.");
                    emmVRCLoader.Logger.LogError("Error: " + ex.ToString());
                    if (File.Exists(Path.Combine(System.Environment.CurrentDirectory, "UserData/emmVRC/config.old.json")))
                        File.Delete(Path.Combine(System.Environment.CurrentDirectory, "UserData/emmVRC/config.old.json"));
                    File.Move(Path.Combine(System.Environment.CurrentDirectory, "UserData/emmVRC/config.json"), Path.Combine(System.Environment.CurrentDirectory, "UserData/emmVRC/config.old.json"));
                    JSONConfig = new Config();
                }
            }
            else
            {
                JSONConfig = new Config();
            }
        }
        public static void WriteConfigOption(string configOptionName, object newValue)
        {
            if (!typeof(Config).GetFields().Any(a => a.Name == configOptionName))
            {
                emmVRCLoader.Logger.LogError("Invalid configuration option specified: "+configOptionName);
                return;
            }
            typeof(Config).GetField(configOptionName).SetValue(JSONConfig, newValue);
            SaveConfig();
            foreach (KeyValuePair<string, Action> listener in onConfigUpdated)
                if (listener.Key == configOptionName)
                    listener.Value.Invoke();
        }
        public static void WipeConfig()
        {
            JSONConfig = new Objects.Config { WelcomeMessageShown = true };
            SaveConfig();
            foreach (KeyValuePair<string, Action> listenerAction in onConfigUpdated)
                listenerAction.Value.Invoke();
        }
        private static void SaveConfig()
        {
            File.WriteAllText(Path.Combine(System.Environment.CurrentDirectory, "UserData/emmVRC/config.json"), TinyJSON.Encoder.Encode(JSONConfig, TinyJSON.EncodeOptions.PrettyPrint));
        }
        public static Color menuColor()
        {
            Color col;
            col = Libraries.ColorConversion.HexToColor(JSONConfig.UIColorHex);
            return col;
        }
        public static Color defaultMenuColor()
        {
            return new Color(0.05f, 0.65f, 0.68f);
        }
    }
}
