using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace emmVRCLoader
{
    public static class Logger
    {
        private static bool consoleEnabled = true;
        private static StreamWriter log;
        private static string fileprefix = "emmVRC_";
        private static int errorCount = 0;

        internal static void Init()
        {
            /*if (log == null)
            {
                string logFilePath = null;
                logFilePath = Path.Combine(Environment.CurrentDirectory, ("Logs/" + fileprefix + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff") + ".log"));
                FileInfo logFileInfo = new FileInfo(logFilePath);
                DirectoryInfo logDirInfo = new DirectoryInfo(logFileInfo.DirectoryName);

                if (!logDirInfo.Exists)
                    logDirInfo.Create();
                else
                    CleanOld(logDirInfo);

                FileStream fileStream = null;
                if (!logFileInfo.Exists)
                    fileStream = logFileInfo.Create();
                else
                    fileStream = new FileStream(logFilePath, FileMode.Open, FileAccess.Write, FileShare.Read);

                log = new StreamWriter(fileStream);
                log.AutoFlush = true;
                try
                {
                    string configpath = null;
                    configpath = Path.Combine(Environment.CurrentDirectory, "UserData\\emmVRC\\config.json");
                }
                catch (Exception ex)
                {
                    consoleEnabled = false;
                    Logger.LogError(ex.ToString());
                }
            }
            Log("[emmVRCLoader] Logger Initialized");*/
        }


        internal static void Stop() { if (log != null) log.Close(); }

        internal static void CleanOld(DirectoryInfo logDirInfo)
        {
            /*FileInfo[] filetbl = logDirInfo.GetFiles(fileprefix + "*");
            if (filetbl.Length > 0)
            {
                List<FileInfo> filelist = filetbl.ToList().OrderBy(x => x.LastWriteTime).ToList();
                for (int i = (filelist.Count - 10); i > -1; i--)
                {
                    FileInfo file = filelist[i];
                    file.Delete();
                }
            }*/
        }

        internal static string GetTimestamp() { return DateTime.Now.ToString("HH:mm:ss.fff"); }

        public static void Log(string s)
        {
            /*var normalConsoleColor = Console.ForegroundColor;
            var timestamp = GetTimestamp();
            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(timestamp);
            Console.ForegroundColor = normalConsoleColor;
            Console.Write("] [");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("emmVRC");
            Console.ForegroundColor = normalConsoleColor;
            Console.WriteLine("] " + s);
            if (log != null) log.WriteLine("[" + timestamp + "] [emmVRC] " + s);*/
            MelonLoader.MelonLogger.Msg(s);
        }

        public static void Log(string s, params object[] args)
        {
            //var normalConsoleColor = Console.ForegroundColor;
            //var timestamp = GetTimestamp();
            var formatted = string.Format(s, args);
            /*Console.Write("[");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(timestamp);
            Console.ForegroundColor = normalConsoleColor;
            Console.Write("] [");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("emmVRC");
            Console.ForegroundColor = normalConsoleColor;
            Console.WriteLine("] "+ s, args);
            if (log != null) log.WriteLine("[" + timestamp + "] [emmVRC] " + formatted);*/
            MelonLoader.MelonLogger.Msg(formatted);
        }

        public static void LogError(string s)
        {
            var normalConsoleColor = Console.ForegroundColor;
            if (errorCount < 255)
            {
                /*var timestamp = GetTimestamp();
                Console.Write("[");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(timestamp);
                Console.ForegroundColor = normalConsoleColor;
                Console.Write("] [");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("emmVRC");
                Console.ForegroundColor = normalConsoleColor;
                Console.Write("] [");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Error");
                Console.ForegroundColor = normalConsoleColor;
                Console.WriteLine("] "+ s);
                if (log != null) log.WriteLine("[" + timestamp + "] [emmVRC] [Error] " + s);*/
                MelonLoader.MelonLogger.Error(s);
                errorCount++;
            }
            if (errorCount == 255)
            {
                /*var timestamp = GetTimestamp();
                Console.Write("[");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(timestamp);
                Console.ForegroundColor = normalConsoleColor;
                Console.Write("] [");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("emmVRC");
                Console.ForegroundColor = normalConsoleColor;
                Console.Write("] [");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Error");
                Console.ForegroundColor = normalConsoleColor;
                Console.WriteLine("] The console error limit has been reached. Please report this issue to Emilia!");
                if (log != null) log.WriteLine("[" + timestamp + "] [emmVRC] [Error] The log file error limit has been reached. Please report this issue to Emilia!");*/
                MelonLoader.MelonLogger.Error("The error limit has been reached. Please report this issue to Emilia!");
                errorCount++;
            }
        }

        public static void LogError(string s, params object[] args)
        {
            var normalConsoleColor = Console.ForegroundColor;
            if (errorCount < 255)
            {
                var timestamp = GetTimestamp();
                var formatted = string.Format(s, args);
                /*Console.Write("[");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(timestamp);
                Console.ForegroundColor = normalConsoleColor;
                Console.Write("] [");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("emmVRC");
                Console.ForegroundColor = normalConsoleColor;
                Console.Write("] [");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Error");
                Console.ForegroundColor = normalConsoleColor;
                Console.WriteLine("] " + formatted);
                if (log != null) log.WriteLine("[" + timestamp + "] [emmVRC] [Error] " + formatted);*/
                MelonLoader.MelonLogger.Error(formatted);
                errorCount++;
            }
            if (errorCount == 255)
            {
                /*var timestamp = GetTimestamp();
                Console.Write("[");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(timestamp);
                Console.ForegroundColor = normalConsoleColor;
                Console.Write("] [");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("emmVRC");
                Console.ForegroundColor = normalConsoleColor;
                Console.Write("] [");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Error");
                Console.ForegroundColor = normalConsoleColor;
                Console.WriteLine("] The console error limit has been reached. Please report this issue to Emilia!");
                if (log != null) log.WriteLine("[" + timestamp + "] [emmVRC] [Error] The log file error limit has been reached. Please report this issue to Emilia!");*/
                MelonLoader.MelonLogger.Error("The error limit has been reached. Please report this issue to Emilia!");
                errorCount++;
            }
        }
        public static void LogDebug(string s)
        {
            if (!Environment.CommandLine.Contains("--emmvrc.debug")) return;
            /*var normalConsoleColor = Console.ForegroundColor;
            var timestamp = GetTimestamp();
            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(timestamp);
            Console.ForegroundColor = normalConsoleColor;
            Console.Write("] [");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("emmVRC");
            Console.ForegroundColor = normalConsoleColor;
            Console.Write("] [");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("Debug");
            Console.ForegroundColor = normalConsoleColor;
            Console.WriteLine("] " + s);
            if (log != null) log.WriteLine("[" + timestamp + "] [emmVRC] [Debug] " + s);*/
            MelonLoader.MelonLogger.Msg("[Debug] " + s);
        }

        public static void LogDebug(string s, params object[] args)
        {
            if (!Environment.CommandLine.Contains("--emmvrc.debug")) return;
            //var normalConsoleColor = Console.ForegroundColor;
            //var timestamp = GetTimestamp();
            var formatted = string.Format(s, args);
            /*Console.Write("[");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(timestamp);
            Console.ForegroundColor = normalConsoleColor;
            Console.Write("] [");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("emmVRC");
            Console.ForegroundColor = normalConsoleColor;
            Console.Write("] [");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("Debug");
            Console.ForegroundColor = normalConsoleColor;
            Console.WriteLine("] " + s, args);
            if (log != null) log.WriteLine("[" + timestamp + "] [emmVRC] [Debug] " + formatted);*/
            MelonLoader.MelonLogger.Msg("[Debug] " + formatted);
        }
    }
}
