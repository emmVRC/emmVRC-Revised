using emmVRC.Hacks;
using emmVRC.Managers;
using emmVRC.Menus;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using emmVRC.Objects;
using emmVRC.Objects.Localization;
using emmVRC.Objects.ModuleBases;

namespace emmVRC.Functions.Core
{
    [Priority(0)]
    public class Localization : MelonLoaderEvents
    {
        public static Language currentLanguage = new English();

        public override void OnUiManagerInit()
        {
            SystemLanguage language;
            if (Configuration.JSONConfig.LanguageOverride != -1)
                language = (SystemLanguage)Configuration.JSONConfig.LanguageOverride;
            else
                language = Application.systemLanguage;
            emmVRCLoader.Logger.LogDebug("System language as reported by Unity is " + Application.systemLanguage.ToString());
            switch (language)
            {
                default:
                    currentLanguage = new English();
                    break;
                case (SystemLanguage.English):
                    currentLanguage = new English();
                    break;
                case (SystemLanguage.German):
                    currentLanguage = new German();
                    break;
            }

        }       
    }
}
