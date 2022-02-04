using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using UnhollowerRuntimeLib;
using emmVRC.Objects.ModuleBases;

namespace emmVRC.Functions.Core
{
    [Priority(0)]
    public class Resources : MelonLoaderEvents
    {
        // Main AssetBundle for emmVRC's Resources
        private static AssetBundle emmVRCBundle;
        // Path to emmVRC's Resources
        private static string resourcePath = Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/Resources");
        private static string dependenciesPath = Path.Combine(Environment.CurrentDirectory, "Dependencies");

        // Gradient texture and material, for use in custom colors for the loading screen
        public static Cubemap blankGradient;

        // Icons, for use in notifications, the logo, nameplate textures, buttons and the HUDs
        public static Sprite offlineSprite;
        public static Sprite onlineSprite;
        public static Sprite owonlineSprite; // April fools~
        public static Sprite alertSprite;
        public static Sprite errorSprite;
        public static Sprite messageSprite;
        public static Sprite alarmSprite;
        public static Sprite rpSprite;
        public static Sprite crownSprite;
        public static Sprite Media_Nav;
        public static Sprite Media_PlayPause;
        public static Sprite Media_Stop;
        public static Sprite HUD_Base;
        public static Sprite HUD_Minimized;
        public static Sprite emmHUDLogo;
        public static Sprite TabIcon;
        public static Sprite authorSprite;
        public static Sprite lensOn;
        public static Sprite lensOff;
        public static Sprite zoomIn;
        public static Sprite zoomOut;

        public static Sprite WorldIcon;
        public static Sprite WorldHistoryIcon;
        public static Sprite PlayerIcon;
        public static Sprite PlayerHistoryIcon;
        public static Sprite ProgramsIcon;
        public static Sprite SettingsIcon;
        public static Sprite AlarmClockIcon;
        public static Sprite TeamIcon;
        public static Sprite SupporterIcon;
        public static Sprite ChangelogIcon;
        public static Sprite CheckMarkIcon;



        public static AudioClip customLoadingMusic;

        // Texture for use on the emmVRC Network panel
        public static Texture2D panelTexture;

        public static Texture2D saveTexture;
        public static Texture2D deleteTexture;
        public static Texture2D flyTexture;

        public static Texture2D toggleOnTexture;
        public static Texture2D toggleOffTexture;

        // Audio clips

        public static AudioClip alarmTone;

        // Quick Method for making adding sprites easier
        private Sprite LoadSprite(string sprite)
        {
            Sprite newSprite = emmVRCBundle.LoadAsset(sprite, Il2CppType.Of<Sprite>()).Cast<Sprite>();
            newSprite.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            return newSprite;
        }

        private Texture2D LoadTexture(string texture)
        {
            Texture2D newTexture = emmVRCBundle.LoadAsset(texture, Il2CppType.Of<Texture2D>()).Cast<Texture2D>();
            newTexture.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            return newTexture;
        }

        private Cubemap LoadCubemap(string cubemap)
        {
            Cubemap newCubemap = emmVRCBundle.LoadAsset(cubemap, Il2CppType.Of<Cubemap>()).Cast<Cubemap>();
            newCubemap.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            return newCubemap;
        }

        private AudioClip LoadAudioClip(string audioclip)
        {
            AudioClip newAudioClip = emmVRCBundle.LoadAsset(audioclip, Il2CppType.Of<AudioClip>()).Cast<AudioClip>();
            newAudioClip.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            return newAudioClip;
        }

        public override void OnUiManagerInit()
        {
            emmVRCLoader.Logger.LogDebug("Initializing resources...");
            MelonLoader.MelonCoroutines.Start(LoadResources());
        }

        // Main function for loading in all the resources from the web and locally
        public IEnumerator LoadResources()
        {

            // Fetch the resources asset bundle, for things like sprites.
            if (Environment.CommandLine.Contains("--emmvrc.assetdev") && Environment.CommandLine.Contains("--emmvrc.devmode"))
            {
                emmVRCBundle = AssetBundle.LoadFromFile(Path.Combine(dependenciesPath, "Normal.emm"));
            }
            else
            {
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
                    assetBundleRequest = UnityWebRequest.Get("https://dl.emmvrc.com/downloads/emmvrcresources/resources.php");

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
            alarmSprite = LoadSprite("Alarm.png");
            rpSprite = LoadSprite("RP.png");

            lensOn = LoadSprite("LensOn.png");
            lensOff = LoadSprite("LensOff.png");
            zoomIn = LoadSprite("ZoomIn.png");
            zoomOut = LoadSprite("ZoomOut.png");

            crownSprite = LoadSprite("Crown.png");
            authorSprite = LoadSprite("Author.png");

            Media_Nav = LoadSprite("Media_NAV.png");
            Media_PlayPause = LoadSprite("Media_PLAY_PAUSE.png");
            Media_Stop = LoadSprite("Media_STOP.png");

            HUD_Base = LoadSprite("UIMaximized.png");
            HUD_Minimized = LoadSprite("UIMinimized.png");
            emmHUDLogo = LoadSprite("emmSimplifedLogo.png");

            TabIcon = LoadSprite("TabIcon.png");

            WorldIcon = LoadSprite("Globe.png");
            WorldHistoryIcon = LoadSprite("GlobeHistory.png");
            PlayerIcon = LoadSprite("Player.png");
            PlayerHistoryIcon = LoadSprite("PlayerHistory.png");
            SupporterIcon = LoadSprite("Heart.png");
            ProgramsIcon = LoadSprite("Programs.png");
            SettingsIcon = LoadSprite("Settings.png");
            AlarmClockIcon = LoadSprite("AlarmClock.png");
            TeamIcon = LoadSprite("RoseIcon.png");
            ChangelogIcon = LoadSprite("Changelog.png");

            CheckMarkIcon = LoadSprite("Checkmark.png");

            panelTexture = LoadTexture("Panel.png");

            saveTexture = LoadTexture("Save.png");
            deleteTexture = LoadTexture("Delete.png");
            flyTexture = LoadTexture("Fly.png");

            toggleOnTexture = LoadTexture("E_GUI_Toggle_ON.png");
            toggleOffTexture = LoadTexture("E_GUI_Toggle_OFF.png");

            blankGradient = LoadCubemap("Gradient.png");

            alarmTone = LoadAudioClip("AlarmSound.ogg");

        }
    }
}