using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using emmVRC.Libraries;
namespace emmVRC.Objects
{
    public class Slider
    {
        public QMSingleButton basePosition;
        public GameObject slider = null;
        public Slider(string parentPath, int x, int y, System.Action<float> evt, float defaultValue = 0f)
        {
            basePosition = new QMSingleButton(parentPath, x, y, "", null, "");
            basePosition.setActive(false);

            slider = GameObject.Instantiate(QuickMenuUtils.GetVRCUiMInstance().menuContent().transform.Find("Screens/Settings/AudioDevicePanel/VolumeSlider"), QuickMenuUtils.GetQuickMenuInstance().transform.Find(parentPath)).gameObject;
                try
                {
                    slider.transform.Find("Fill Area/Label").gameObject.SetActive(false);
                }
                catch (Exception ex)
                {
                    ex = new Exception();
                }
            
            slider.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            slider.transform.localPosition = basePosition.getGameObject().transform.localPosition;
            slider.GetComponentInChildren<RectTransform>().anchorMin += new Vector2(0.06f, 0);
            slider.GetComponentInChildren<RectTransform>().anchorMax += new Vector2(0.1f, 0);
            slider.GetComponentInChildren<UnityEngine.UI.Slider>().onValueChanged = new UnityEngine.UI.Slider.SliderEvent();
            slider.GetComponentInChildren<UnityEngine.UI.Slider>().value = defaultValue;
            slider.GetComponentInChildren<UnityEngine.UI.Slider>().onValueChanged.AddListener(UnhollowerRuntimeLib.DelegateSupport.ConvertDelegate<UnityEngine.Events.UnityAction<float>>(evt));

            Components.EnableDisableListener listener = slider.AddComponent<Components.EnableDisableListener>();
            listener.OnEnabled += () => {
                slider.GetComponentInChildren<UnityEngine.UI.Slider>().value = defaultValue;
                GameObject.DestroyImmediate(listener);
            };

        }
    }
}
