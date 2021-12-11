using emmVRC.Objects.ModuleBases;
using emmVRC.Utils;
using UnityEngine;
using VRC.Core;

namespace emmVRC.Functions.PlayerHacks
{
    public class ESP : MelonLoaderEvents
    {
        public static bool IsEnabled { get; private set; }

        // Create our own highlight fx instance so no other things get in the way of ours // There's a bit of a perf overhead but it's not really noticable
        private static HighlightsFXStandalone highlightFX;
        private static HighlightsFXStandalone friendHighlightFX;

        public override void OnApplicationStart()
        {
            NetworkEvents.OnPlayerJoined += (player) =>
            {
                if (APIUser.IsFriendsWith(player.field_Private_APIUser_0.id))
                    friendHighlightFX.field_Protected_HashSet_1_Renderer_0.Add(player.transform.Find("SelectRegion").GetComponent<Renderer>());
                else
                    highlightFX.field_Protected_HashSet_1_Renderer_0.Add(player.transform.Find("SelectRegion").GetComponent<Renderer>());
            };
            NetworkEvents.OnPlayerLeft += (player) =>
            {
                if (APIUser.IsFriendsWith(player.field_Private_APIUser_0.id))
                    friendHighlightFX.field_Protected_HashSet_1_Renderer_0.Remove(player.transform.Find("SelectRegion").GetComponent<Renderer>());
                else
                    highlightFX.field_Protected_HashSet_1_Renderer_0.Remove(player.transform.Find("SelectRegion").GetComponent<Renderer>());
            };
        }
        public override void OnUiManagerInit()
        {
            Configuration.onConfigUpdated.Add(new System.Collections.Generic.KeyValuePair<string, System.Action>("RiskyFunctionsEnabled", () => {
                if (!Configuration.JSONConfig.RiskyFunctionsEnabled && IsEnabled)
                {
                    SetActive(false);
                }
            }));
        }

        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            if (buildIndex != -1)
                return;

            if (highlightFX == null)
            {
                highlightFX = HighlightsFX.field_Private_Static_HighlightsFX_0.gameObject.AddComponent<HighlightsFXStandalone>();
                friendHighlightFX = HighlightsFX.field_Private_Static_HighlightsFX_0.gameObject.AddComponent<HighlightsFXStandalone>();
            }

            highlightFX.enabled = false;
            //highlightFX.field_Protected_HashSet_1_Renderer_0?.Clear();
            friendHighlightFX.highlightColor = Libraries.ColorConversion.HexToColor(Configuration.JSONConfig.FriendNamePlateColorHex) + new Color(0f, 0f, 0f, 1f);
            friendHighlightFX.enabled = false;
            //friendHighlightFX.field_Protected_HashSet_1_Renderer_0?.Clear();
        }

        public static void SetActive(bool active)
        {
            if (highlightFX != null)
            {
                highlightFX.enabled = active;
                friendHighlightFX.enabled = active;
            }
            IsEnabled = active;
        }
    }
}