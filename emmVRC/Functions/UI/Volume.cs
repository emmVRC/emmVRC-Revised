using emmVRC.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using emmVRC.Objects.ModuleBases;

namespace emmVRC.Functions.UI
{
    public class Volume : MelonLoaderEvents
    {
        private static UnityEngine.UI.Slider UIVolumeSlider;
        private static UnityEngine.UI.Slider WorldVolumeSlider;
        private static UnityEngine.UI.Slider VoicesVolumeSlider;
        private static UnityEngine.UI.Slider AvatarVolumeSlider;
        private static GameObject UIVolumeMuteButton;
        private static GameObject WorldVolumeMuteButton;
        private static GameObject VoiceVolumeMuteButton;
        private static GameObject AvatarVolumeMuteButton;
        public override void OnUiManagerInit()
        {
            GameObject baseObject = UnityEngine.Resources.FindObjectsOfTypeAll<VRCUiPageSettings>().FirstOrDefault().gameObject;
            UIVolumeSlider = baseObject.transform.Find("VolumePanel/VolumeUi").GetComponent<Slider>();
            UIVolumeSlider.onValueChanged.AddListener(new System.Action<float>((float flt) =>
            {
                if (Configuration.JSONConfig.UIVolumeMute && flt != 0)
                {
                    Configuration.WriteConfigOption("UIVolumeMute", false);
                    UIVolumeMuteButton.GetComponentInChildren<Text>().text = (Configuration.JSONConfig.UIVolumeMute ? "U" : "M");
                }
            }));
            WorldVolumeSlider = baseObject.transform.Find("VolumePanel/VolumeGameWorld").GetComponent<Slider>();
            WorldVolumeSlider.onValueChanged.AddListener(new System.Action<float>((float flt) =>
            {
                if (Configuration.JSONConfig.WorldVolumeMute && flt != 0)
                {
                    Configuration.WriteConfigOption("WorldVolumeMute", false);
                    WorldVolumeMuteButton.GetComponentInChildren<Text>().text = (Configuration.JSONConfig.WorldVolumeMute ? "U" : "M");
                }
            }));
            VoicesVolumeSlider = baseObject.transform.Find("VolumePanel/VolumeGameVoice").GetComponent<Slider>();
            VoicesVolumeSlider.onValueChanged.AddListener(new System.Action<float>((float flt) =>
            {
                if (Configuration.JSONConfig.VoiceVolumeMute && flt != 0)
                {
                    Configuration.WriteConfigOption("VoiceVolumeMute", false);
                    VoiceVolumeMuteButton.GetComponentInChildren<Text>().text = (Configuration.JSONConfig.VoiceVolumeMute ? "U" : "M");
                }
            }));
            AvatarVolumeSlider = baseObject.transform.Find("VolumePanel/VolumeGameAvatars").GetComponent<Slider>();
            AvatarVolumeSlider.onValueChanged.AddListener(new System.Action<float>((float flt) =>
            {
                if (Configuration.JSONConfig.AvatarVolumeMute && flt != 0)
                {
                    Configuration.WriteConfigOption("AvatarVolumeMute", false);
                    AvatarVolumeMuteButton.GetComponentInChildren<Text>().text = (Configuration.JSONConfig.AvatarVolumeMute ? "U" : "M");
                }
            }));
            UIVolumeMuteButton = GameObject.Instantiate(baseObject.transform.Find("Footer/Exit").gameObject, baseObject.transform.Find("VolumePanel").transform);
            UIVolumeMuteButton.GetComponent<RectTransform>().sizeDelta /= new Vector2(5.25f, 1.75f);
            UIVolumeMuteButton.GetComponent<RectTransform>().anchoredPosition += new Vector2(180f, 468.5f);
            UIVolumeMuteButton.GetComponentInChildren<Text>().fontSize = (int)(UIVolumeMuteButton.GetComponentInChildren<Text>().fontSize / 1.75);
            UIVolumeMuteButton.GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
            WorldVolumeMuteButton = GameObject.Instantiate(UIVolumeMuteButton, UIVolumeMuteButton.transform.parent);
            WorldVolumeMuteButton.GetComponent<RectTransform>().anchoredPosition -= new Vector2(0f, 47f);
            VoiceVolumeMuteButton = GameObject.Instantiate(WorldVolumeMuteButton, UIVolumeMuteButton.transform.parent);
            VoiceVolumeMuteButton.GetComponent<RectTransform>().anchoredPosition -= new Vector2(0f, 47f);
            AvatarVolumeMuteButton = GameObject.Instantiate(VoiceVolumeMuteButton, UIVolumeMuteButton.transform.parent);
            AvatarVolumeMuteButton.GetComponent<RectTransform>().anchoredPosition -= new Vector2(0f, 47f);
            UIVolumeMuteButton.GetComponent<Button>().onClick.AddListener(new System.Action(() =>
            {
                if (!Configuration.JSONConfig.UIVolumeMute)
                {
                    Configuration.WriteConfigOption("UIVolumeMute", true);
                    Configuration.WriteConfigOption("UIVolume", UIVolumeSlider.value);
                    UIVolumeSlider.value = 0f;
                    UIVolumeMuteButton.GetComponentInChildren<Text>().text = "U";
                }
                else
                {
                    Configuration.WriteConfigOption("UIVolumeMute", false);
                    UIVolumeSlider.value = Configuration.JSONConfig.UIVolume;
                    UIVolumeMuteButton.GetComponentInChildren<Text>().text = "M";
                }
            }));
            WorldVolumeMuteButton.GetComponent<Button>().onClick.AddListener(new System.Action(() =>
            {
                if (!Configuration.JSONConfig.WorldVolumeMute)
                {
                    Configuration.WriteConfigOption("WorldVolumeMute", true);
                    Configuration.WriteConfigOption("WorldVolume", UIVolumeSlider.value);
                    WorldVolumeSlider.value = 0f;
                    WorldVolumeMuteButton.GetComponentInChildren<Text>().text = "U";
                }
                else
                {
                    Configuration.WriteConfigOption("WorldVolumeMute", false);
                    WorldVolumeSlider.value = Configuration.JSONConfig.WorldVolume;
                    WorldVolumeMuteButton.GetComponentInChildren<Text>().text = "M";
                }
            }));
            VoiceVolumeMuteButton.GetComponent<Button>().onClick.AddListener(new System.Action(() =>
            {
                if (!Configuration.JSONConfig.VoiceVolumeMute)
                {
                    Configuration.WriteConfigOption("VoiceVolumeMute", true);
                    Configuration.WriteConfigOption("VoiceVolume", UIVolumeSlider.value);
                    VoicesVolumeSlider.value = 0f;
                    VoiceVolumeMuteButton.GetComponentInChildren<Text>().text = "U";
                }
                else
                {
                    Configuration.WriteConfigOption("VoiceVolumeMute", false);
                    VoicesVolumeSlider.value = Configuration.JSONConfig.VoiceVolume;
                    VoiceVolumeMuteButton.GetComponentInChildren<Text>().text = "M";
                }
            }));
            AvatarVolumeMuteButton.GetComponent<Button>().onClick.AddListener(new System.Action(() =>
            {
                if (!Configuration.JSONConfig.AvatarVolumeMute)
                {
                    Configuration.WriteConfigOption("AvatarVolumeMute", true);
                    Configuration.WriteConfigOption("AvatarVolume", UIVolumeSlider.value);
                    AvatarVolumeSlider.value = 0f;
                    AvatarVolumeMuteButton.GetComponentInChildren<Text>().text = "U";
                }
                else
                {
                    Configuration.WriteConfigOption("AvatarVolumeMute", false);
                    AvatarVolumeSlider.value = Configuration.JSONConfig.AvatarVolume;
                    AvatarVolumeMuteButton.GetComponentInChildren<Text>().text = "M";
                }
            })); 
            UIVolumeMuteButton.GetComponentInChildren<Text>().text = (Configuration.JSONConfig.UIVolumeMute ? "U" : "M");
            WorldVolumeMuteButton.GetComponentInChildren<Text>().text = (Configuration.JSONConfig.WorldVolumeMute ? "U" : "M");
            VoiceVolumeMuteButton.GetComponentInChildren<Text>().text = (Configuration.JSONConfig.VoiceVolumeMute ? "U" : "M");
            AvatarVolumeMuteButton.GetComponentInChildren<Text>().text = (Configuration.JSONConfig.AvatarVolumeMute ? "U" : "M");
        }
    }
}
