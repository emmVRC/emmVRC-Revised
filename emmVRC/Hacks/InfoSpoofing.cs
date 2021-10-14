/*using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using emmVRC.Libraries;
using emmVRC.Objects.ModuleBases;

namespace emmVRC.Hacks
{
    public class NameSpoofGenerator
    {
        public static string spoofedName
        {
            get
            {
                if (Configuration.JSONConfig.InfoSpoofingName != "")
                    return Configuration.JSONConfig.InfoSpoofingName;
                else
                    return generatedSpoofName;
            }
        }
        private static string generatedSpoofName = "";
        private static List<string> adjectiveList = new List<string>()
        {
            "Adorable",
            "Adorbs",
            "Alluring",
            "Appealing",
            "Aromatic",
            "Beautiful",
            "Beauteous",
            "Bewitching",
            "Bonny",
            "Cute",
            "Charming",
            "Comely",
            "Darling",
            "Delightful",
            "Daring",
            "Dark",
            "Dreary",
            "Enchanting",
            "Engaging",
            "Exquisite",
            "Fair",
            "Fit",
            "Foxy",
            "Glamorous",
            "Gorgeous",
            "Humble",
            "Happy",
            "Heroic",
            "Harmonious",
            "Hot",
            "Hopeful",
            "Homely",
            "Hex",
            "Hypnotic",
            "Hyper",
            "Luscious",
            "Luxurious",
            "Magnificent",
            "Mistress",
            "Nice",
            "Personable",
            "Pleasing",
            "Pretty",
            "Picturesque",
            "Ravishing",
            "Scenic",
            "Seductive",
            "Sexy",
            "Sightly",
            "Smashing",
            "Splendid",
            "Stunning",
            "Sweet",
            "Tasty"
        };
        private static List<string> nounList = new List<string>()
        {
            "Angel",
            "Aurora",
            "Abelia",
            "Acer",
            "Allium",
            "Alpine",
            "Almond",
            "Abode",
            "Abyss",
            "Ace",
            "Aerie",
            "Alum",
            "Bamboo",
            "Bay",
            "Bella",
            "Bunny",
            "Blossom",
            "Blueberry",
            "Crystal",
            "Camellia",
            "Canna",
            "Carnation",
            "Diamond",
            "Daffodil",
            "Daylight",
            "Demon",
            "Demoness",
            "Emilia",
            "Emerald",
            "Elf",
            "Elderberry",
            "Eucalyptus",
            "Flower",
            "Fairy",
            "Feather",
            "Fox",
            "Garnet",
            "Gamer",
            "Grace",
            "Galaxy",
            "Gift",
            "Gianna",
            "Grape",
            "Hazel",
            "Heart",
            "Humility",
            "Hypatia",
            "Lavender",
            "Lush",
            "Lore",
            "Lapis",
            "Mana",
            "Miracle",
            "Moon",
            "Nora",
            "Nebula",
            "Night",
            "Platinum",
            "Port",
            "Princess",
            "Rhodium",
            "Ruby",
            "Succubus",
            "Seductress",
            "Savior",
            "Topaz",
            "Tera",
            "Tulip",
            "Tale",
            "Tail"
        };
        internal static void GenerateNewName()
        {
            System.Random r = new System.Random();
            int randomAdjectiveIndex = r.Next(adjectiveList.Count());
            string protoString = adjectiveList[randomAdjectiveIndex];
            while (protoString == adjectiveList[randomAdjectiveIndex])
            {
                int randomNounIndex = r.Next(nounList.Count());
                string protoString2 = protoString + nounList[randomNounIndex];
                if (protoString2.Length < 15)
                {
                    protoString = protoString2;
                }
            }
            generatedSpoofName = protoString;
        }
    }
    public class InfoSpoofing : MelonLoaderEvents, IWithGUI
    {
        private static bool Enabled = true;
        private static bool wasEnabled1 = false;
        private static bool wasEnabled2 = false;
        private static List<Transform> objectRoots;

        public override void OnUiManagerInit()
        {
            if (Configuration.JSONConfig.StealthMode) return;
            NameSpoofGenerator.GenerateNewName();
        }
        public void OnGUI()
        {
            if (QuickMenuUtils.GetQuickMenuInstance() == null || QuickMenuUtils.GetQuickMenuInstance().gameObject == null) return;
            if (objectRoots == null)
            {
                if (QuickMenuUtils.GetVRCUiMInstance().menuContent().transform.Find("Screens/Social") != null)
                    if (QuickMenuUtils.GetVRCUiMInstance().menuContent().transform.Find("Screens/WorldInfo") != null)
                        if (QuickMenuUtils.GetVRCUiMInstance().menuContent().transform.Find("Screens/UserInfo") != null)
                        {
                            objectRoots = new List<Transform>();
                            objectRoots.Add(QuickMenuUtils.GetVRCUiMInstance().menuContent().transform.Find("Screens/Social"));
                            objectRoots.Add(QuickMenuUtils.GetVRCUiMInstance().menuContent().transform.Find("Screens/WorldInfo"));
                            objectRoots.Add(QuickMenuUtils.GetVRCUiMInstance().menuContent().transform.Find("Screens/UserInfo"));
                            objectRoots.Add(QuickMenuUtils.GetVRCUiMInstance().menuContent().transform.Find("Screens/Settings"));
                        }
            }
            if ((Configuration.JSONConfig.InfoSpoofingEnabled) && objectRoots != null)
            {
                try
                {
                    if (RoomManager.field_Internal_Static_ApiWorld_0 != null)
                    {
                        foreach (Transform trns in objectRoots)
                        {
                            if (trns.gameObject.activeInHierarchy)
                            {
                                foreach (Text text in trns.GetComponentsInChildren<Text>())
                                {
                                    if (text.text.Contains(VRC.Core.APIUser.CurrentUser.GetName()))
                                        text.text = text.text.Replace(VRC.Core.APIUser.CurrentUser.GetName(), (NameSpoofGenerator.spoofedName));
                                }
                                wasEnabled1 = true;
                            }
                        }
                        if (QuickMenuUtils.GetQuickMenuInstance().gameObject.activeInHierarchy)
                        {
                            foreach (Text text in QuickMenuUtils.GetQuickMenuInstance().gameObject.GetComponentsInChildren<Text>())
                            {
                                if (text.text.Contains(VRC.Core.APIUser.CurrentUser.GetName()))
                                    text.text = text.text.Replace(VRC.Core.APIUser.CurrentUser.GetName(), (NameSpoofGenerator.spoofedName));
                            }
                            wasEnabled2 = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    ex = new Exception();
                }
                if (!Configuration.JSONConfig.InfoSpoofingEnabled && (wasEnabled1 || wasEnabled2) && objectRoots != null)
                {
                    try
                    {
                        if (RoomManager.field_Internal_Static_ApiWorld_0 != null)
                        {
                            foreach (Transform trns in objectRoots)
                            {
                                if (trns.gameObject.activeInHierarchy && wasEnabled1)
                                {
                                    foreach (Text text in trns.GetComponentsInChildren<Text>())
                                    {
                                        if (text.text.Contains(NameSpoofGenerator.spoofedName))
                                        {
                                            text.text = text.text.Replace(NameSpoofGenerator.spoofedName, VRC.Core.APIUser.CurrentUser.GetName());
                                        }
                                    }
                                    wasEnabled1 = false;
                                }
                            }
                            if (QuickMenuUtils.GetQuickMenuInstance().gameObject.activeInHierarchy && wasEnabled2)
                            {
                                foreach (Text text in QuickMenuUtils.GetQuickMenuInstance().gameObject.GetComponentsInChildren<Text>())
                                {
                                    if (text.text.Contains(NameSpoofGenerator.spoofedName))
                                    {
                                        text.text = text.text.Replace(NameSpoofGenerator.spoofedName, VRC.Core.APIUser.CurrentUser.GetName());
                                    }
                                }
                                wasEnabled2 = false;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        emmVRCLoader.Logger.LogError("Spoofer error: " + ex.ToString());
                    }
                }
            }

        }
    }
}
*/