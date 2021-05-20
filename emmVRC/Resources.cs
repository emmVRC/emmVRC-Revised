using System;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO;
using UnhollowerRuntimeLib;
using VRC.UI;

namespace emmVRC
{
    public static class Resources
    {
        // Main AssetBundle for emmVRC's Resources
        private static AssetBundle emmVRCBundle;
        // Path to emmVRC's Resources
        public static string resourcePath = Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/Resources");
        public static string dependenciesPath = Path.Combine(Environment.CurrentDirectory, "Dependencies");

        // Gradient texture and material, for use in custom colors for the loading screen
        public static Cubemap blankGradient;

        // Icons, for use in notifications, the logo, nameplate textures, buttons and the HUDs
        public static Sprite offlineSprite;
        public static Sprite onlineSprite;
        public static Sprite owonlineSprite; // April fools~
        public static Sprite alertSprite;
        public static Sprite errorSprite;
        public static Sprite messageSprite;
        public static Sprite rpSprite;
        public static Sprite crownSprite;
        public static Sprite Media_Nav;
        public static Sprite Media_PlayPause;
        public static Sprite Media_Stop;
        public static Sprite HUD_Base;
        public static Sprite HUD_Minimized;
        public static Sprite emmHUDLogo;
        public static Sprite TabIcon;

        public static AudioClip customLoadingMusic;

        // Texture for use on the emmVRC Network panel
        public static Texture2D panelTexture;

        public static Texture2D saveTexture;
        public static Texture2D deleteTexture;
        public static Texture2D flyTexture;

        public static Texture2D toggleOnTexture;
        public static Texture2D toggleOffTexture;

        // Quick Method for making adding sprites easier
        private static Sprite LoadSprite(string sprite) {
            Sprite newSprite = emmVRCBundle.LoadAsset(sprite, Il2CppType.Of<Sprite>()).Cast<Sprite>();
            newSprite.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            return newSprite;
        }

        private static Texture2D LoadTexture(string texture) {
            Texture2D newTexture = emmVRCBundle.LoadAsset(texture, Il2CppType.Of<Texture2D>()).Cast<Texture2D>();
            newTexture.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            return newTexture;
        }

        private static Cubemap LoadCubemap(string cubemap)
        {
            Cubemap newCubemap = emmVRCBundle.LoadAsset(cubemap, Il2CppType.Of<Cubemap>()).Cast<Cubemap>();
            newCubemap.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            return newCubemap;
        }

        // Main function for loading in all the resources from the web and locally
        public static IEnumerator LoadResources()
        {
            // If the path to the emmVRC Resources directory doesn't exist, make it exist
            /*if (!Directory.Exists(resourcePath))
                Directory.CreateDirectory(resourcePath);

            // Check if the UI texture directory exists. If not, the files don't, either.
            if (!Directory.Exists(Path.Combine(resourcePath, "HUD")))
                Directory.CreateDirectory(Path.Combine(resourcePath, "HUD"));

            // Check if the Sprites directory exists. If not, the files don't, either.
            if (!Directory.Exists(Path.Combine(resourcePath, "Sprites")))
                Directory.CreateDirectory(Path.Combine(resourcePath, "Sprites"));

            // Check if the Textures directory exists. If not, the files don't, either.
            if (!Directory.Exists(Path.Combine(resourcePath, "Textures")))
                Directory.CreateDirectory(Path.Combine(resourcePath, "Textures"));

            // Fetch the HUD textures, if they do not exist
            if (!File.Exists(Path.Combine(resourcePath, "HUD/UIMinimized.png")) || !File.Exists(Path.Combine(resourcePath, "HUD/UIMaximized.png")))
            {
                // Fetch the byte[] streams of data from Emilia's servers
                using (WebClient webClient = new WebClient())
                {
                    webClient.DownloadFile("https://dl.emmvrc.com/downloads/emmvrcresources/HUD/uiMinimized.png", Path.Combine(resourcePath, "HUD/UIMinimized.png"));
                    webClient.DownloadFile("https://dl.emmvrc.com/downloads/emmvrcresources/HUD/uiMaximized.png", Path.Combine(resourcePath, "HUD/UIMaximized.png"));
                }
            }*/

            // Fetch the resources asset bundle, for things like sprites.
            if (Environment.CommandLine.Contains("--emmvrc.assetdev") && Environment.CommandLine.Contains("--emmvrc.devmode")) {
                emmVRCBundle = AssetBundle.LoadFromFile(Path.Combine(dependenciesPath, "Normal.emm"));
            }
            else {
                UnityWebRequest assetBundleRequest;
                if (Environment.CommandLine.Contains("--emmvrc.anniversarymode"))
                    assetBundleRequest = UnityWebRequest.Get("https://dl.emmvrc.com/downloads/emmvrcresources/Seasonals/Anniversary.emm");
                else if (Environment.CommandLine.Contains("--emmvrc.pridemode"))
                    assetBundleRequest = UnityWebRequest.Get("https://dl.emmvrc.com/downloads/emmvrcresources/Seasonals/Pride.emm");
                else if (Environment.CommandLine.Contains("--emmvrc.normalmode"))
                    assetBundleRequest = UnityWebRequest.Get("https://dl.emmvrc.com/downloads/emmvrcresources/Seasonals/Normal.emm");
                else if (Environment.CommandLine.Contains("--emmvrc.halloweenmode"))
                    assetBundleRequest = UnityWebRequest.Get("https://dl.emmvrc.com/downloads/emmvrcresources/Seasonals/Halloween.emm");
                else if (Environment.CommandLine.Contains("--emmvrc.xmasmode"))
                    assetBundleRequest = UnityWebRequest.Get("https://dl.emmvrc.com/downloads/emmvrcresources/Seasonals/Xmas.emm");
                else if (Environment.CommandLine.Contains("--emmvrc.beemode"))
                    assetBundleRequest = UnityWebRequest.Get("https://dl.emmvrc.com/downloads/emmvrcresources/Seasonals/Bee.emm");
                else
                    assetBundleRequest = UnityWebRequest.Get("https://dl.emmvrc.com/downloads/emmvrcresources/emmVRCResources.emm");

                assetBundleRequest.SendWebRequest();
                while (!assetBundleRequest.isDone && !assetBundleRequest.isHttpError)
                    yield return new WaitForSeconds(0.1f);

                if (assetBundleRequest.isHttpError)
                {
                    try
                    {
                        emmVRCBundle = AssetBundle.LoadFromFile(Path.Combine(dependenciesPath, "Resources.emm"));
                    }
                    catch (Exception ex)
                    {
                        emmVRCLoader.Logger.LogError("emmVRC could not load resources. Many UI elements and features will be broken.");
                    }
                }
                else
                {
                    File.WriteAllBytes(Path.Combine(dependenciesPath, "Resources.emm"), assetBundleRequest.downloadHandler.data);
                    AssetBundleCreateRequest dlBundle = AssetBundle.LoadFromMemoryAsync(assetBundleRequest.downloadHandler.data);
                    while (!dlBundle.isDone)
                        yield return new WaitForSeconds(0.1f);
                    emmVRCBundle = dlBundle.assetBundle;
                }
            }
            // Made loading much simpler. If issues are found add yield return before each sprite.
            offlineSprite = LoadSprite("Offline.png");
            onlineSprite = LoadSprite("Online.png");
            owonlineSprite = LoadSprite("OwOnline.png");

            alertSprite = LoadSprite("Alert.png");
            errorSprite = LoadSprite("Error.png");
            messageSprite = LoadSprite("Message.png");
            rpSprite = LoadSprite("RP.png");

            crownSprite = LoadSprite("Crown.png");

            Media_Nav = LoadSprite("Media_NAV.png");
            Media_PlayPause = LoadSprite("Media_PLAY_PAUSE.png");
            Media_Stop = LoadSprite("Media_STOP.png");

            HUD_Base = LoadSprite("UIMaximized.png");
            HUD_Minimized = LoadSprite("UIMinimized.png");
            emmHUDLogo = LoadSprite("emmSimplifedLogo.png");

            TabIcon = LoadSprite("TabIcon.png");
            
            panelTexture = LoadTexture("Panel.png");

            saveTexture = LoadTexture("Save.png");
            deleteTexture = LoadTexture("Delete.png");
            flyTexture = LoadTexture("Fly.png");

            toggleOnTexture = LoadTexture("E_GUI_Toggle_ON.png"); 
            toggleOffTexture = LoadTexture("E_GUI_Toggle_OFF.png");

            blankGradient = LoadCubemap("Gradient.png");

        }
    }
}