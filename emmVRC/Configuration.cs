﻿using Il2CppSystem;
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
        public static Config JSONConfig;
        public static void Initialize()
        {
            if (!Directory.Exists(Path.Combine(System.Environment.CurrentDirectory, "UserData/emmVRC")))
                Directory.CreateDirectory(Path.Combine(System.Environment.CurrentDirectory, "UserData/emmVRC"));
            if (File.Exists(Path.Combine(System.Environment.CurrentDirectory, "UserData/emmVRC/config.json"))){
                //JSONConfig = JsonConvert.DeserializeObject<Config>(File.ReadAllText(Path.Combine(System.Environment.CurrentDirectory, "UserData/emmVRC/config.json")));
                JSONConfig = TinyJSON.Decoder.Decode(File.ReadAllText(Path.Combine(System.Environment.CurrentDirectory, "UserData/emmVRC/config.json"))).Make<Config>();
                if (JSONConfig == null)
                {
                    JSONConfig = new Config();
                    emmVRCLoader.Logger.LogError("The configuration file was null. Resetting...");
                }
            }
            else
            {
                JSONConfig = new Config();
            }
        }
        public static void SaveConfig()
        {
            File.WriteAllText(Path.Combine(System.Environment.CurrentDirectory, "UserData/emmVRC/config.json"), TinyJSON.Encoder.Encode(JSONConfig, TinyJSON.EncodeOptions.PrettyPrint));
        }
        public static Color menuColor()
        {
            Color col;
            col = Libraries.ColorConversion.HexToColor(JSONConfig.UIColorHex);
            //ColorUtility.TryParseHtmlString(resources.jsonConfig.uiColor, out col);
            return col;
        }
        public static Color defaultMenuColor()
        {
            return new Color(0.05f, 0.65f, 0.68f);
        }
    }
}
