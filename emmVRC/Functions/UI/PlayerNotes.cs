using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using emmVRC.Libraries;
using UnityEngine;
using UnityEngine.Events;
using emmVRC.Utils;
using emmVRC.Objects;
using emmVRC.Objects.ModuleBases;

namespace emmVRC.Functions.UI
{
    public class PlayerNote
    {
        public string UserID;
        public string NoteText;
    }
    public class PlayerNotes : MelonLoaderEvents
    {
        public override void OnUiManagerInit()
        {
            if (!Directory.Exists(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/UserNotes")))
                Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/UserNotes"));
        }
        public static void LoadNote(string userID, string displayName)
        {
            try
            {
                PlayerNote loadedNote;
                if (File.Exists(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/UserNotes/" + userID + ".json")))
                {
                    string NoteText = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/UserNotes/" + userID + ".json"));
                    if (NoteText.Contains("emmVRC.Hacks"))
                    {
                        NoteText = NoteText.Replace("emmVRC.Hacks", "emmVRC.Functions.UI");
                        File.WriteAllText(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/UserNotes/" + userID + ".json"), NoteText);
                    }
                    loadedNote = TinyJSON.Decoder.Decode(File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/UserNotes/" + userID + ".json"))).Make<PlayerNote>();
                }

                else
                    loadedNote = new PlayerNote { UserID = userID, NoteText = "" };
                VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopupV2("Note for " + displayName, string.IsNullOrWhiteSpace(loadedNote.NoteText) ? "There is currently no note for this user." : loadedNote.NoteText, "Change Note", () =>
                {
                    VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowInputPopup("Enter a note for "+displayName+":", loadedNote.NoteText, UnityEngine.UI.InputField.InputType.Standard, false, "Accept", new System.Action<string, Il2CppSystem.Collections.Generic.List<UnityEngine.KeyCode>, UnityEngine.UI.Text>((string newNoteText, Il2CppSystem.Collections.Generic.List<UnityEngine.KeyCode> keyk, UnityEngine.UI.Text tx) => {
                        PlayerNote newNote = new PlayerNote { UserID = userID, NoteText = newNoteText };
                        SaveNote(newNote);
                    }), null, "Enter note....");
                });
            } catch (Exception ex)
            {
                emmVRCLoader.Logger.LogError("Failed to load note: " + ex.ToString());
            }
        }
        public static void LoadNoteQM(string userID, string displayName)
        {
            try
            {
                PlayerNote loadedNote;
                if (File.Exists(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/UserNotes/" + userID + ".json")))
                {
                    string NoteText = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/UserNotes/" + userID + ".json"));
                    if (NoteText.Contains("emmVRC.Hacks"))
                    {
                        NoteText = NoteText.Replace("emmVRC.Hacks", "emmVRC.Functions.UI");
                        File.WriteAllText(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/UserNotes/" + userID + ".json"), NoteText);
                    }
                    loadedNote = TinyJSON.Decoder.Decode(File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/UserNotes/" + userID + ".json"))).Make<PlayerNote>();
                }

                else
                    loadedNote = new PlayerNote { UserID = userID, NoteText = "" };
                ButtonAPI.GetQuickMenuInstance().ShowCustomDialog("Note for " + displayName, string.IsNullOrWhiteSpace(loadedNote.NoteText) ? "There is currently no note for this user." : loadedNote.NoteText, "", "Change Note", "Close", null, () =>
                  {
                      VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowInputPopup("Enter a note for " + displayName + ":", loadedNote.NoteText, UnityEngine.UI.InputField.InputType.Standard, false, "Accept", new System.Action<string, Il2CppSystem.Collections.Generic.List<UnityEngine.KeyCode>, UnityEngine.UI.Text>((string newNoteText, Il2CppSystem.Collections.Generic.List<UnityEngine.KeyCode> keyk, UnityEngine.UI.Text tx) => {
                          PlayerNote newNote = new PlayerNote { UserID = userID, NoteText = newNoteText };
                          SaveNote(newNote);
                      }), null, "Enter note....");
                  }, null);
            }
            catch (Exception ex)
            {
                emmVRCLoader.Logger.LogError("Failed to load note: " + ex.ToString());
            }
        }
        public static void SaveNote(PlayerNote note)
        {
            File.WriteAllText(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/UserNotes/" + note.UserID + ".json"), TinyJSON.Encoder.Encode(note, TinyJSON.EncodeOptions.PrettyPrint | TinyJSON.EncodeOptions.NoTypeHints));
        }
    }
}
