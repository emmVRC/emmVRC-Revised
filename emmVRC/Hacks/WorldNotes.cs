﻿using System;
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
    public class WorldNote
    {
        public string worldID;
        public string NoteText;
    }
    /*[ObsoleteAttribute("This class only remains for compatibility. Please use WorldNote.", false)]
    public class LegacyWorldNote
    {
        
        public string worldId;
        public string worldName;
        public string noteText;
    }*/
    public class WorldNotes
    {
        public static void Initialize()
        {
            if (!Directory.Exists(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/worldNotes")))
                Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/worldNotes"));
            /*if (File.Exists(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/worldNotes.json"))){
                List<LegacyWorldNote> legacyNotes = TinyJSON.Decoder.Decode(File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/worldNotes.json"))).Make<List<LegacyWorldNote>>();
                foreach (LegacyWorldNote note in legacyNotes)
                {
                    WorldNote convertedNote = new WorldNote { worldID = note.worldId, NoteText = note.noteText };
                    SaveNote(convertedNote);
                }
                File.Delete(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/worldNotes.json"));
            }*/
        }
        public static void LoadNote(string worldID, string displayName)
        {
            try
            {
                WorldNote loadedNote;
                if (File.Exists(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/worldNotes/" + worldID + ".json")))
                    loadedNote = TinyJSON.Decoder.Decode(File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/worldNotes/" + worldID + ".json"))).Make<WorldNote>();
                else
                    loadedNote = new WorldNote { worldID = worldID, NoteText = "" };
                VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowStandardPopupV2("Note for " + displayName, loadedNote.NoteText == "" ? "There is currently no note for this world." : loadedNote.NoteText, "Change Note", () =>
                {
                    VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0.ShowInputPopup("Enter a note for " + displayName + ":", "", UnityEngine.UI.InputField.InputType.Standard, false, "Accept", new System.Action<string, Il2CppSystem.Collections.Generic.List<UnityEngine.KeyCode>, UnityEngine.UI.Text>((string newNoteText, Il2CppSystem.Collections.Generic.List<UnityEngine.KeyCode> keyk, UnityEngine.UI.Text tx) =>
                    {
                        WorldNote newNote = new WorldNote { worldID = worldID, NoteText = newNoteText };
                        SaveNote(newNote);
                    }), null, "Enter note....");
                });
            } catch (Exception ex)
            {
                emmVRCLoader.Logger.LogError("Failed to load note: " + ex.ToString());
            }
        }
        public static void SaveNote(WorldNote note)
        {
            File.WriteAllText(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/worldNotes/" + note.worldID + ".json"), TinyJSON.Encoder.Encode(note, TinyJSON.EncodeOptions.PrettyPrint));
        }
    }
}
