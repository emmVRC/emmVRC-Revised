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
    internal class B64Textures
    {
        public static string Gradient = "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";
    }
    public static class Resources
    {
        // Main AssetBundle for emmVRC's Resources
        private static AssetBundle emmVRCBundle;
        // Path to emmVRC's Resources
        public static string resourcePath = Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/Resources");
        public static string dependenciesPath = Path.Combine(Environment.CurrentDirectory, "Dependencies");

        // Gradient texture and material, for use in custom colors for the loading screen
        public static Texture2D blankGradient = new Texture2D(16, 16);
        public static Material gradientMaterial = new Material(Shader.Find("Skybox/6 Sided"));

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

        // Main function for loading in all the resources from the web and locally
        public static IEnumerator LoadResources()
        {
            // If the path to the emmVRC Resources directory doesn't exist, make it exist
            if (!Directory.Exists(resourcePath))
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
                    webClient.DownloadFile("https://thetrueyoshifan.com/downloads/emmvrcresources/HUD/uiMinimized.png", Path.Combine(resourcePath, "HUD/UIMinimized.png"));
                    webClient.DownloadFile("https://thetrueyoshifan.com/downloads/emmvrcresources/HUD/uiMaximized.png", Path.Combine(resourcePath, "HUD/UIMaximized.png"));
                }
            }

            // Fetch the resources asset bundle, for things like sprites.
            if (Environment.CommandLine.Contains("--emmvrc.assetdev") && Environment.CommandLine.Contains("--emmvrc.devmode")) {
                if (Environment.CommandLine.Contains("--emmvrc.anniversarymode"))
                    emmVRCBundle = AssetBundle.LoadFromFile(Path.Combine(dependenciesPath, "anniversary.emm"));
                else if (Environment.CommandLine.Contains("--emmvrc.pridemode"))
                    emmVRCBundle = AssetBundle.LoadFromFile(Path.Combine(dependenciesPath, "pride.emm"));
                else
                    emmVRCBundle = AssetBundle.LoadFromFile(Path.Combine(dependenciesPath, "normal.emm"));
            }
            else {
                UnityWebRequest assetBundleRequest;
                if (Environment.CommandLine.Contains("--emmvrc.anniversarymode"))
                    assetBundleRequest = UnityWebRequest.Get("https://thetrueyoshifan.com/downloads/emmvrcresources/Seasonals/Anniversary.emm");
                else if (Environment.CommandLine.Contains("--emmvrc.pridemode"))
                    assetBundleRequest = UnityWebRequest.Get("https://thetrueyoshifan.com/downloads/emmvrcresources/Seasonals/Pride.emm");
                else if (Environment.CommandLine.Contains("--emmvrc.normalmode"))
                    assetBundleRequest = UnityWebRequest.Get("https://thetrueyoshifan.com/downloads/emmvrcresources/Seasonals/Normal.emm");
                else if (Environment.CommandLine.Contains("--emmvrc.halloweenmode"))
                    assetBundleRequest = UnityWebRequest.Get("https://thetrueyoshifan.com/downloads/emmvrcresources/Seasonals/Halloween.emm");
                else
                    assetBundleRequest = UnityWebRequest.Get("https://thetrueyoshifan.com/downloads/emmvrcresources/emmVRCResources.emm");

                assetBundleRequest.SendWebRequest();
                while (!assetBundleRequest.isDone)
                    yield return new WaitForSeconds(0.1f);

                AssetBundleCreateRequest dlBundle = AssetBundle.LoadFromMemoryAsync(assetBundleRequest.downloadHandler.data);
                while (!dlBundle.isDone)
                    yield return new WaitForSeconds(0.1f);
                emmVRCBundle = dlBundle.assetBundle;
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
            
            panelTexture = LoadTexture("Panel.png");

            saveTexture = LoadTexture("Save.png");
            deleteTexture = LoadTexture("Delete.png");
            flyTexture = LoadTexture("Fly.png");

            toggleOnTexture = LoadTexture("E_GUI_Toggle_ON.png"); 
            toggleOffTexture = LoadTexture("E_GUI_Toggle_OFF.png");

        }
    }
}