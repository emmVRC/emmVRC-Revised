using emmVRC.Objects;
using emmVRC.TinyJSON;
using emmVRC.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace emmVRC.Functions.Other
{
    public static class Programs
    {
        public static readonly string ProgramsFilePath = Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/programs.json");

        public static IReadOnlyList<Program> GetPrograms()
        {
            if (!File.Exists(ProgramsFilePath))
            {
                Program example = new Program
                {
                    name = "Notepad",
                    programPath = "C:\\Windows\\notepad.exe",
                    toolTip = "Example program: Launch Notepad. See programs.json for usage"
                };
                File.WriteAllText(ProgramsFilePath, Encoder.Encode(new List<Program>() { example }));
                return new List<Program>() { example };
            }

            try
            {
                return Decoder.Decode(File.ReadAllText(ProgramsFilePath)).Make<List<Program>>();
            }
            catch (Exception ex)
            {
                emmVRCLoader.Logger.LogError("Could not load the programs JSON file. Please make sure everything is correctly formatted!\n" + ex.ToString());
                return null;
            }
        }

        public static void OpenProgram(Program program)
        {
            try
            {
                if (!string.IsNullOrEmpty(program.programPath))
                    Process.Start(program.programPath);
                else if (!string.IsNullOrEmpty(program.url))
                    UnityWebRequestUtils.Get(program.url, null);
                else
                    emmVRCLoader.Logger.LogError("Could the program as both the path and url are invalid.");
            }
            catch (Exception ex)
            {
                emmVRCLoader.Logger.LogError("An error occured while launching your program:\n" + ex.ToString());
            }
        }
    }
}