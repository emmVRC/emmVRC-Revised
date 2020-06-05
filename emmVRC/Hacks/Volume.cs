using emmVRC.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace emmVRC.Hacks
{
    public class Volume
    {
        private static UnityEngine.UI.Slider UIVolumeSlider;
        private static UnityEngine.UI.Slider WorldVolumeSlider;
        private static UnityEngine.UI.Slider VoicesVolumeSlider;
        private static UnityEngine.UI.Slider AvatarVolumeSlider;
        private static GameObject UIVolumeMuteButton;
        private static GameObject WorldVolumeMuteButton;
        private static GameObject VoiceVolumeMuteButton;
        private static GameObject AvatarVolumeMuteButton;
        public static void Initialize()
        {
            try
            {
                UIVolumeSlider = QuickMenuUtils.GetVRCUiMInstance().menuContent.transform.Find("Screens/Settings/VolumePanel/VolumeUi").GetComponent<Slider>();
                WorldVolumeSlider = QuickMenuUtils.GetVRCUiMInstance().menuContent.transform.Find("Screens/Settings/VolumePanel/VolumeGameWorld").GetComponent<Slider>();
                VoicesVolumeSlider = QuickMenuUtils.GetVRCUiMInstance().menuContent.transform.Find("Screens/Settings/VolumePanel/VolumeGameVoice").GetComponent<Slider>();
                AvatarVolumeSlider = QuickMenuUtils.GetVRCUiMInstance().menuContent.transform.Find("Screens/Settings/VolumePanel/VolumeGameAvatars").GetComponent<Slider>();
                UIVolumeMuteButton = GameObject.Instantiate(QuickMenuUtils.GetVRCUiMInstance().menuContent.transform.Find("Screens/Settings/Footer/Exit").gameObject, QuickMenuUtils.GetVRCUiMInstance().menuContent.transform.Find("Screens/Settings/VolumePanel").transform);
                UIVolumeMuteButton.GetComponent<RectTransform>().sizeDelta /= new Vector2(5.25f, 1.75f);
                UIVolumeMuteButton.GetComponent<RectTransform>().anchoredPosition += new Vector2(180f, 468.5f);
                UIVolumeMuteButton.GetComponentInChildren<Text>().fontSize =  (int)(UIVolumeMuteButton.GetComponentInChildren<Text>().fontSize / 1.75);
                UIVolumeMuteButton.GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
                WorldVolumeMuteButton = GameObject.Instantiate(UIVolumeMuteButton, UIVolumeMuteButton.transform.parent);
                WorldVolumeMuteButton.GetComponent<RectTransform>().anchoredPosition -= new Vector2(0f, 47f);
                VoiceVolumeMuteButton = GameObject.Instantiate(WorldVolumeMuteButton, UIVolumeMuteButton.transform.parent);
                VoiceVolumeMuteButton.GetComponent<RectTransform>().anchoredPosition -= new Vector2(0f, 47f);
                AvatarVolumeMuteButton = GameObject.Instantiate(VoiceVolumeMuteButton, UIVolumeMuteButton.transform.parent);
                AvatarVolumeMuteButton.GetComponent<RectTransform>().anchoredPosition -= new Vector2(0f, 47f);
                UIVolumeMuteButton.GetComponent<Button>().onClick.AddListener(new System.Action(() => {
                    if (!Configuration.JSONConfig.UIVolumeMute)
                    {
                        Configuration.JSONConfig.UIVolumeMute = true;
                        Configuration.JSONConfig.UIVolume = UIVolumeSlider.value;
                        UIVolumeSlider.value = 0f;
                        Configuration.SaveConfig();
                        UIVolumeMuteButton.GetComponentInChildren<Text>().text = "U";
                    }
                    else
                    {
                        Configuration.JSONConfig.UIVolumeMute = false;
                        UIVolumeSlider.value = Configuration.JSONConfig.UIVolume;
                        Configuration.SaveConfig();
                        UIVolumeMuteButton.GetComponentInChildren<Text>().text = "M";
                    }
                }));
                WorldVolumeMuteButton.GetComponent<Button>().onClick.AddListener(new System.Action(() => {
                    if (!Configuration.JSONConfig.WorldVolumeMute)
                    {
                        Configuration.JSONConfig.WorldVolumeMute = true;
                        Configuration.JSONConfig.WorldVolume = WorldVolumeSlider.value;
                        WorldVolumeSlider.value = 0f;
                        Configuration.SaveConfig();
                        WorldVolumeMuteButton.GetComponentInChildren<Text>().text = "U";
                    }
                    else
                    {
                        Configuration.JSONConfig.WorldVolumeMute = false;
                        WorldVolumeSlider.value = Configuration.JSONConfig.WorldVolume;
                        Configuration.SaveConfig();
                        WorldVolumeMuteButton.GetComponentInChildren<Text>().text = "M";
                    }
                }));
                VoiceVolumeMuteButton.GetComponent<Button>().onClick.AddListener(new System.Action(() => {
                    if (!Configuration.JSONConfig.VoiceVolumeMute)
                    {
                        Configuration.JSONConfig.VoiceVolumeMute = true;
                        Configuration.JSONConfig.VoiceVolume = VoicesVolumeSlider.value;
                        VoicesVolumeSlider.value = 0f;
                        Configuration.SaveConfig();
                        VoiceVolumeMuteButton.GetComponentInChildren<Text>().text = "U";
                    }
                    else
                    {
                        Configuration.JSONConfig.VoiceVolumeMute = false;
                        VoicesVolumeSlider.value = Configuration.JSONConfig.VoiceVolume;
                        Configuration.SaveConfig();
                        VoiceVolumeMuteButton.GetComponentInChildren<Text>().text = "M";
                    }
                }));
                AvatarVolumeMuteButton.GetComponent<Button>().onClick.AddListener(new System.Action(() => {
                    if (!Configuration.JSONConfig.AvatarVolumeMute)
                    {
                        Configuration.JSONConfig.AvatarVolumeMute = true;
                        Configuration.JSONConfig.AvatarVolume = AvatarVolumeSlider.value;
                        AvatarVolumeSlider.value = 0f;
                        Configuration.SaveConfig();
                        AvatarVolumeMuteButton.GetComponentInChildren<Text>().text = "U";
                    }
                    else
                    {
                        Configuration.JSONConfig.AvatarVolumeMute = false;
                        AvatarVolumeSlider.value = Configuration.JSONConfig.AvatarVolume;
                        Configuration.SaveConfig();
                        AvatarVolumeMuteButton.GetComponentInChildren<Text>().text = "M";
                    }
                }));
                UIVolumeMuteButton.GetComponentInChildren<Text>().text = (Configuration.JSONConfig.UIVolumeMute ? "U" : "M");
                WorldVolumeMuteButton.GetComponentInChildren<Text>().text = (Configuration.JSONConfig.WorldVolumeMute ? "U" : "M");
                VoiceVolumeMuteButton.GetComponentInChildren<Text>().text = (Configuration.JSONConfig.VoiceVolumeMute ? "U" : "M");
                AvatarVolumeMuteButton.GetComponentInChildren<Text>().text = (Configuration.JSONConfig.AvatarVolumeMute ? "U" : "M");
            }
            catch (Exception ex)
            {
                emmVRCLoader.Logger.LogError(ex.ToString());
            }
        }
    }
}
