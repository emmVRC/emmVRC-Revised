using emmVRC.Libraries;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using emmVRC.Objects;
using emmVRC.Managers;
using Il2CppSystem.Diagnostics;

namespace emmVRC.Menus
{
    public class ProgramMenu
    {
        public static PaginatedMenu baseMenu;

        public static void Initialize()
        {
            baseMenu = new PaginatedMenu(FunctionsMenu.baseMenu.menuBase, 203945, 102894, "Programs", "", null);
            baseMenu.menuEntryButton.DestroyMe();
            List<Program> programs = new List<Program>();
            if (!File.Exists(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/programs.json")))
            {
                programs = new List<Program>();
                Program exampleProgram = new Program();
                exampleProgram.name = "Notepad";
                exampleProgram.programPath = "C:\\Windows\\notepad.exe";
                exampleProgram.toolTip = "Example program: Launch Notepad. See programs.json for usage";
                programs.Add(exampleProgram);
                File.WriteAllText(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/programs.json"), TinyJSON.Encoder.Encode(programs, TinyJSON.EncodeOptions.PrettyPrint));
            }
            else
            {
                string input = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "UserData/emmVRC/programs.json"));
                try
                {
                    programs = TinyJSON.Decoder.Decode(input).Make<List<Program>>();
                } catch (Exception ex)
                {
                    emmVRCLoader.Logger.LogError("Your program list file is invalid. Please check JSON validity and try again. Error: "+ex.ToString());
                    Managers.NotificationManager.AddNotification("Your program list file is invalid. Please check JSON validity and try again.", "Dismiss", Managers.NotificationManager.DismissCurrentNotification, "", null, Resources.errorSprite, -1);
                }
            }
            foreach (Program program in programs)
            {
                baseMenu.pageItems.Add(new PageItem(program.name, () =>
                {
                    try
                    {
                        System.Diagnostics.Process.Start("C:\\Windows\\system32\\cmd.exe", "/C " + program.programPath);
                    } catch (Exception ex)
                    {
                        emmVRCLoader.Logger.LogError("Error occured while launching your program: " + ex.ToString());
                    }
                }, program.toolTip));
            }
        }
    }
}
