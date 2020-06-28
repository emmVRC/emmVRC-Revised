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

namespace emmVRC.Hacks
{
    public class PlayerNote
    {
        public string UserID;
        public string NoteText;
    }
    /*[ObsoleteAttribute("This class only remains for compatibility. Please use PlayerNote.", false)]
    public class LegacyPlayerNote
    {
        
        public string userId;
        public string userName;
        public string noteText;
    }*/
    public class PlayerNotes
    {
        public static void Initialize()
        {
            if (!Directory.Exists(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/UserNotes")))
                Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/UserNotes"));
            /*if (File.Exists(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/userNotes.json"))){
                List<LegacyPlayerNote> legacyNotes = TinyJSON.Decoder.Decode(File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/userNotes.json"))).Make<List<LegacyPlayerNote>>();
                foreach (LegacyPlayerNote note in legacyNotes)
                {
                    PlayerNote convertedNote = new PlayerNote { UserID = note.userId, NoteText = note.noteText };
                    SaveNote(convertedNote);
                }
                File.Delete(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/userNotes.json"));
            }*/
        }
        public static void LoadNote(string userID, string displayName)
        {
            try
            {
                PlayerNote loadedNote;
                if (File.Exists(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/UserNotes/" + userID + ".json")))
                    loadedNote = TinyJSON.Decoder.Decode(File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/UserNotes/" + userID + ".json"))).Make<PlayerNote>();
                else
                    loadedNote = new PlayerNote { UserID = userID, NoteText = "" };
                VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopupV2("Note for " + displayName, loadedNote.NoteText == "" ? "There is currently no note for this user." : loadedNote.NoteText, "Change Note", () =>
                {
                    InputUtilities.OpenInputBox("Enter a note for " + displayName + ":", "Accept", (string newNoteText) =>
                    {
                        PlayerNote newNote = new PlayerNote { UserID = userID, NoteText = newNoteText };
                        SaveNote(newNote);
                    });
                });
            } catch (Exception ex)
            {
                emmVRCLoader.Logger.LogError("Failed to load note: " + ex.ToString());
            }
        }
        public static void SaveNote(PlayerNote note)
        {
            File.WriteAllText(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/UserNotes/" + note.UserID + ".json"), TinyJSON.Encoder.Encode(note, TinyJSON.EncodeOptions.PrettyPrint));
        }
    }
}
