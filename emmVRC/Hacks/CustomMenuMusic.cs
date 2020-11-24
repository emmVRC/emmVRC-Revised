using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using emmVRC.Libraries;

namespace emmVRC.Hacks
{
    public class CustomMenuMusic
    {
        public static IEnumerator Initialize()
        {
            if (!ModCompatibility.BetterLoadingScreen && File.Exists(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/custommenu.ogg"))){
                emmVRCLoader.Logger.Log("Processing custom menu music...");
                GameObject loadingMusic1 = GameObject.Find("LoadingBackground_TealGradient_Music/LoadingSound");
                GameObject loadingMusic2 = Libraries.QuickMenuUtils.GetVRCUiMInstance().menuContent.transform.Find("Popups/LoadingPopup/LoadingSound").gameObject;
                if (loadingMusic1 != null)
                    loadingMusic1.GetComponent<AudioSource>().Stop();
                if (loadingMusic2 != null)
                    loadingMusic2.GetComponent<AudioSource>().Stop();
                WWW CustomLoadingMusicWWW = new WWW(string.Format("file://{0}", Environment.CurrentDirectory + "/UserData/emmVRC/custommenu.ogg").Replace(@"\", "/"));
                AudioClip customLoadingMusic = CustomLoadingMusicWWW.GetAudioClip();
                while (!CustomLoadingMusicWWW.isDone || customLoadingMusic.loadState == AudioDataLoadState.Loading) ;

                if (customLoadingMusic != null)
                {
                    if (loadingMusic1 != null)
                    {
                        loadingMusic1.GetComponent<AudioSource>().clip = customLoadingMusic;
                        loadingMusic1.GetComponent<AudioSource>().Play();
                    }
                    if (loadingMusic2 != null)
                    {
                        loadingMusic2.GetComponent<AudioSource>().clip = customLoadingMusic;
                        loadingMusic2.GetComponent<AudioSource>().Play();
                    }
                }
            }
            yield return null;

        }
    }
}
