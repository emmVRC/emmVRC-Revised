/*
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using emmVRC.Objects;
using System.Collections;
using UnityEngine;
using VRC.Core;
using emmVRC.Menus;
using UnityEngine.UI;
using emmVRC.Libraries;

namespace emmVRC.Hacks
{
    public class AvatarPropertySaving
    {
        public static System.Collections.Generic.List<string> vrcBlacklist = new System.Collections.Generic.List<string> { "Viseme", "GestureLeft", "GestureLeftWeight", "GestureRight", "GestureRightWeight", "VelocityX", "VelocityY", "VelocityZ", "AngularY", "Grounded", "Seated", "Upright", "Supine", "GroundProximity", "AFK", "IsLocal", "VRCEmote", "VRCFaceBlendH", "VRCFaceBlendV", "TrackingType", "VRMode", "AvatarVersion" };
        public static void Initialize()
        {
            if (!Directory.Exists(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/AvatarProperties/")))
                Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/AvatarProperties/"));
        }
        public static IEnumerator OnLoadAvatar(VRC.SDKBase.VRC_AvatarDescriptor avatarDescriptor)
        {
            yield return new WaitForSecondsRealtime(0.25f);
            if (avatarDescriptor != null && avatarDescriptor.GetComponentInParent<VRCPlayer>() != null && avatarDescriptor.GetComponentInParent<VRCPlayer>().prop_Player_0.prop_APIUser_0.id == APIUser.CurrentUser.id)
            {
                VRCAvatarManager mngr = VRCPlayer.field_Internal_Static_VRCPlayer_0.prop_VRCAvatarManager_0;
                if (mngr != null && mngr.prop_VRCAvatarDescriptor_0 != null)
                {
                    PlayerTweaksMenu.SaveAvatarParameters.getGameObject().GetComponent<Button>().enabled = true;
                    if (VRCPlayer.field_Internal_Static_VRCPlayer_0.prop_ApiAvatar_0 != null && VRCPlayer.field_Internal_Static_VRCPlayer_0.prop_ApiAvatar_0.id != null)
                    {
                        string avatarID = VRCPlayer.field_Internal_Static_VRCPlayer_0.prop_ApiAvatar_0.id;
                        emmVRCLoader.Logger.LogDebug("SDK3 avatar detected");
                        if (File.Exists(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/AvatarProperties/", avatarID + ".json")))
                        {
                            try
                            {
                                emmVRCLoader.Logger.LogDebug("Avatar parameter file exists, loading...");
                                List<SerializableAvatarParameter> parameters = TinyJSON.Decoder.Decode(File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/AvatarProperties/", avatarID + ".json"))).Make<List<SerializableAvatarParameter>>();
                                foreach (SerializableAvatarParameter param in parameters)
                                {
                                    emmVRCLoader.Logger.LogDebug("Parameter name: " + param.name + ", float value: " + param.floatValue + ", int value: " + param.intValue);
                                    if (mngr.field_Private_AvatarPlayableController_0 != null)
                                    {
                                        emmVRCLoader.Logger.LogDebug("Applying parameter...");
                                        foreach (Il2CppSystem.Collections.Generic.KeyValuePair<int, ObjectPublicAnStInObLi1BoInSiBoUnique> val in mngr.field_Private_AvatarPlayableController_0.field_Private_Dictionary_2_Int32_ObjectPublicAnStInObLi1BoInSiBoUnique_0)
                                        {
                                            if (val.Value.prop_String_0 == param.name)
                                                switch (val.value.prop_EnumNPublicSealedvaUnBoInFl5vUnique_0)
                                                {
                                                    case ObjectPublicAnStInObLi1BoInSiBoUnique.EnumNPublicSealedvaUnBoInFl5vUnique.Float:
                                                        emmVRCLoader.Logger.LogDebug("Parameter is float");
                                                        val.value.prop_Single_0 = (float)param.floatValue;
                                                        break;
                                                    case ObjectPublicAnStInObLi1BoInSiBoUnique.EnumNPublicSealedvaUnBoInFl5vUnique.Int:
                                                        emmVRCLoader.Logger.LogDebug("Parameter is int");
                                                        val.value.prop_Int32_1 = (int)param.intValue;
                                                        break;
                                                }
                                        }
                                        //mngr.field_Private_AvatarPlayableController_0.Method_Private_Void_0();
                                    }
                                }
                                mngr.field_Private_AvatarPlayableController_0.ApplyParameters(0);

                            }
                            catch (Exception ex)
                            {
                                emmVRCLoader.Logger.Log("Error occurred reading property file for this avatar. The avatar has most likely changed since the last time properties were saved.");
                                emmVRCLoader.Logger.LogError(ex.ToString());
                                File.Delete(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/AvatarProperties/", mngr.field_Private_ApiAvatar_0.id + ".json"));
                            }
                        }
                    }

                    else
                    {
                        PlayerTweaksMenu.SaveAvatarParameters.getGameObject().GetComponent<Button>().enabled = false;
                    }
                }
                else
                {
                    PlayerTweaksMenu.SaveAvatarParameters.getGameObject().GetComponent<Button>().enabled = false;
                }
            }
        }
        public static void SaveAvatarParameters()
        {
            VRCPlayer selectedPlayer = VRCPlayer.field_Internal_Static_VRCPlayer_0;
            if (selectedPlayer.prop_VRCAvatarManager_0.prop_VRCAvatarDescriptor_0 == null) return;
            if (selectedPlayer.prop_VRCAvatarManager_0.field_Private_AvatarPlayableController_0.field_Private_Dictionary_2_Int32_ObjectPublicAnStInObLi1BoInSiBoUnique_0 != null)
            {
                System.Collections.Generic.List<SerializableAvatarParameter> parameters = new System.Collections.Generic.List<SerializableAvatarParameter>();
                foreach (var thing in selectedPlayer.prop_VRCAvatarManager_0.field_Private_AvatarPlayableController_0.field_Private_Dictionary_2_Int32_ObjectPublicAnStInObLi1BoInSiBoUnique_0)
                {
                    if (vrcBlacklist.IndexOf(thing.value.prop_String_0) == -1)
                    {
                        switch (thing.value.field_Private_EnumNPublicSealedvaUnBoInFl5vUnique_0)
                        {
                            case ObjectPublicAnStInObLi1BoInSiBoUnique.EnumNPublicSealedvaUnBoInFl5vUnique.Float:
                                parameters.Add(new SerializableAvatarParameter { name = thing.value.prop_String_0, floatValue = thing.value.prop_Single_0 });
                                break;
                            case ObjectPublicAnStInObLi1BoInSiBoUnique.EnumNPublicSealedvaUnBoInFl5vUnique.Int:
                                parameters.Add(new SerializableAvatarParameter { name = thing.value.prop_String_0, intValue = thing.value.prop_Int32_1 });
                                break;
                        }
                    }
                }
                File.WriteAllText(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/AvatarProperties/", selectedPlayer.prop_VRCAvatarManager_0.field_Private_ApiAvatar_0.id + ".json"), TinyJSON.Encoder.Encode(parameters, TinyJSON.EncodeOptions.PrettyPrint));
            }
        }
        public static void ClearAvatarParameters()
        {
            if (File.Exists(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/AvatarProperties/", VRCPlayer.field_Internal_Static_VRCPlayer_0.prop_VRCAvatarManager_0.field_Private_ApiAvatar_0.id + ".json")))
                File.Delete(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/AvatarProperties/", VRCPlayer.field_Internal_Static_VRCPlayer_0.prop_VRCAvatarManager_0.field_Private_ApiAvatar_0.id + ".json"));
        }
    }
}*/