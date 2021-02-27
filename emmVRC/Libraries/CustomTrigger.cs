/*using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VRC.SDKBase;
using VRC;
using VRC.Core;
using UnityEngine;
using Harmony;

namespace emmVRC.Libraries
{
    public class CustomTrigger
    {
        private static bool LongCheck(long input, long type) { return (input & type) == type; }

        private static Transform VRCTrackingManager_GetTrackedTransform(int trackerpos)
        {
            if (trackingmanagerGetTrackedTransform == null)
                trackingmanagerGetTrackedTransform = typeof(VRCTrackingManager).GetMethod("GetTrackedTransform");
            if (trackingmanagerGetTrackedTransform != null)
            {
                object[] paramtbl = new object[] { trackerpos };
                return (Transform)trackingmanagerGetTrackedTransform.Invoke(null, paramtbl);
            }
            return null;
        }

        private static TutorialManager GetTutorialManager()
        {
            if (tutorialManagerInstance == null)
            {
                FieldInfo[] nonpublicStaticPopupFields = typeof(TutorialManager).GetFields(BindingFlags.NonPublic | BindingFlags.Static);
                if (nonpublicStaticPopupFields.Length == 0)
                    return null;
                FieldInfo tutorialManagerInstanceField = nonpublicStaticPopupFields.First(field => field.FieldType == typeof(TutorialManager));
                if (tutorialManagerInstanceField == null)
                    return null;
                tutorialManagerInstance = tutorialManagerInstanceField.GetValue(null) as TutorialManager;
            }
            return tutorialManagerInstance;
        }

        private static List<VRC_Interactable> VRCUiCursor_InteractablesList(object __0)
        {
            if (interactablesfield == null)
            {
                Type[] typestbl = typeof(VRCUiCursor).GetNestedTypes();
                if (typestbl.Length != 0)
                {
                    FieldInfo[] fieldstbl = typestbl[0].GetFields();
                    if (fieldstbl.Length != 0)
                        interactablesfield = fieldstbl.First(field => field.FieldType == typeof(List<>).MakeGenericType(typeof(VRC_Interactable)));
                }
            }
            if (interactablesfield != null)
                return interactablesfield.GetValue(__0) as List<VRC_Interactable>;
            return null;
        }

        private static VRC_Pickup VRCUiCursor_Pickup(object __0)
        {
            if (pickupfield == null)
            {
                Type[] typestbl = typeof(VRCUiCursor).GetNestedTypes();
                if (typestbl.Length != 0)
                {
                    FieldInfo[] fieldstbl = typestbl[0].GetFields();
                    if (fieldstbl.Length != 0)
                        pickupfield = fieldstbl.First(field => field.FieldType == typeof(VRC_Pickup));
                }
            }
            if (pickupfield != null)
                return pickupfield.GetValue(__0) as VRC_Pickup;
            return null;
        }

        private static bool VRCInputManager_GetSetting(int setting)
        {
            if (inputManagerGetSetting == null)
                inputManagerGetSetting = typeof(VRCInputManager).GetMethod("GetSetting");
            if (inputManagerGetSetting != null)
            {
                object[] paramtbl = new object[] { setting };
                return (bool)inputManagerGetSetting.Invoke(null, paramtbl);
            }
            return false;
        }
        private static void SetTargetInfo(VRCUiCursor __instance, object __0)
        {
            // VRCUiCursor Left = 2
            bool left_handed = ((int)__instance.field_Public_EnumNPublicSealedvaNoRiLe4vUnique_0 == 2);
            VRC_Pickup outputpickup = null;
            Il2CppSystem.Collections.Generic.List<VRC_Interactable> outputinteractiblelist = null;

            // VRCInputManager LegacyGrasp = 5
            if (!VRCInputManager_GetSetting(5))
            {
                Component component = null;
                List<VRC_Interactable> interactablelist = VRCUiCursor_InteractablesList(__0);
                if (interactablelist != null)
                {
                    foreach (VRC_Interactable vrc_Interactable in interactablelist)
                    {
                        VRC_UseEvents component1 = vrc_Interactable.GetComponent<VRC_UseEvents>();
                        VRCSDK2.VRC_CustomTrigger component2 = vrc_Interactable.GetComponent<VRCSDK2.VRC_CustomTrigger>();
                        VRC_Trigger component3 = vrc_Interactable.GetComponent<VRC_Trigger>();
                        bool flag1 = (component1 != null);
                        bool flag2 = (component2 != null);
                        bool flag3 = ((component3 != null) && (component3.HasInteractiveTriggers || component3.HasPickupTriggers));
                        if (flag1 || flag2 || flag3)
                        {
                            component = vrc_Interactable;
                            break;
                        }
                    }
                }

                __instance.field_Public_VRCUiSelectedOutline_0.Clone(null);

                // VRCUiCursor.ILJCEPNGACG.Interactable = 8
                if (LongCheck((long)__instance.over, 8) && (component != null))
                {
                    // VRCTracking.CJBHMLMLAHK.HandTracker_LeftPalm = 9
                    // VRCTracking.CJBHMLMLAHK.HandTracker_RightPalm = 10
                    Transform trackedTransform = VRCTrackingManager_GetTrackedTransform(left_handed ? 9 : 10);
                    if ((trackedTransform == null) || VRCTrackingManager.Method_Public_Static_Boolean_Vector3_0(trackedTransform.position))
                    {
                        __instance.field_Public_VRCUiSelectedOutline_0.Clone(component);
                        GetTutorialManager().Method_Public_Void_List_1_VRC_Interactable_Component_Boolean_0(interactablelist, component, left_handed);
                        outputinteractiblelist = interactablelist.ToArray();
                    }
                }

                // VRCUiCursor Pickup = 16
                VRC_Pickup newpickup = VRCUiCursor_Pickup(__0);
                if (LongCheck((long)__instance.over, 16) && (newpickup != null))
                {
                    if (newpickup.currentlyHeldBy == null)
                    {
                        __instance.field_Public_VRCUiSelectedOutline_0.Clone(newpickup);
                        GetTutorialManager().Method_Public_Void_VRC_Pickup_Boolean_PDM_0(newpickup, left_handed);
                        outputpickup = newpickup;
                    }
                    else
                        __instance.field_Public_VRCUiSelectedOutline_0.Clone(null);
                }
            }
            else
                __instance.field_Public_VRCUiSelectedOutline_0.Clone(null);

            VRCHandGrasper vrchandGrasper = null;
            if (VRCPlayer.field_Internal_Static_VRCPlayer_0 != null)
                vrchandGrasper = VRCPlayer.field_Internal_Static_VRCPlayer_0.Method_Public_VRCHandGrasper_ControllerHand_0(left_handed ? ControllerHand.Left : ControllerHand.Right);
            if (vrchandGrasper != null)
                vrchandGrasper.Method_Public_Void_VRC_Pickup_List_1_VRC_Interactable_0(outputpickup, outputinteractiblelist);
        }
        private static IEnumerable<CodeInstruction> SetTargetInfo_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> codelist = new List<CodeInstruction>();
            for (int i = 0; i < instructions.Count<CodeInstruction>(); i++)
            {
                CodeInstruction codeInstruction = instructions.ElementAt(i);
                if ("call Boolean get_legacyGrasp()".Equals(codeInstruction.ToString()))
                    break;
                else
                    codelist.Add(codeInstruction);
            }
            return codelist.AsEnumerable<CodeInstruction>();
        }
    }

}
*/