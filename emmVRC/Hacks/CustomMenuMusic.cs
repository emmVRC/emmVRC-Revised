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
            if (!ModCompatibility.BetterLoadingScreen){
                
                if (!Directory.Exists(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/CustomMenuMusic")))
                    Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/CustomMenuMusic"));
                if (File.Exists(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/custommenu.ogg")))
                {
                    try
                    {
                        File.Move(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/custommenu.ogg"), Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/CustomMenuMusic/custommenu.ogg"));
                    } catch (Exception ex)
                    {
                        ex = new Exception();
                        emmVRCLoader.Logger.LogError("A custommenu.ogg was detected, but you already have one in the CustomMenuMusic folder. Please put custom menu music in the Ogg Vorbis format into UserData/emmVRC/CustomMenuMusic instead.");
                    }
                }
                string[] availableCustomMenuMusics = Directory.GetFiles(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/CustomMenuMusic"));
                if (availableCustomMenuMusics.Length >= 1)
                {
                    emmVRCLoader.Logger.Log("Processing custom menu music...");
                    System.Random rndm = new System.Random();
                    int randomIndex = rndm.Next(availableCustomMenuMusics.Length);
                    emmVRCLoader.Logger.LogDebug("Picked track: " + availableCustomMenuMusics[randomIndex]);
                    GameObject loadingMusic1 = GameObject.Find("LoadingBackground_TealGradient_Music/LoadingSound");
                    GameObject loadingMusic2 = Libraries.QuickMenuUtils.GetVRCUiMInstance().menuContent().transform.Find("Popups/LoadingPopup/LoadingSound").gameObject;
                    if (loadingMusic1 != null)
                        loadingMusic1.GetComponent<AudioSource>().Stop();
                    if (loadingMusic2 != null)
                        loadingMusic2.GetComponent<AudioSource>().Stop();
                    WWW CustomLoadingMusicWWW = new WWW(string.Format("file://{0}", availableCustomMenuMusics[randomIndex]).Replace(@"\", "/"));
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
            }
            yield return null;

        }
    }
}
