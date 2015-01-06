﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Storage;

namespace VLC_WINRT_APP.Helpers
{
    public static class LogHelper
    {
        public static StorageFile LogFile;
        public static bool usedForRead = false;
        static LogHelper()
        {
            Initialize();
        }

        static async Task Initialize()
        {
            LogFile = await ApplicationData.Current.LocalFolder.CreateFileAsync("LogFile.txt", CreationCollisionOption.OpenIfExists);
            Log("------------------------------------------");
            Log("------------------------------------------");
            Log("------------------------------------------");
            Log("App launch " + DateTime.Now.ToString());
        }
        public static void Log(object o)
        {
            Debug.WriteLine(o.ToString());
            WriteInLog(o.ToString());
        }

        static void WriteInLog(string value)
        {
            if (LogFile != null && !usedForRead) FileIO.AppendTextAsync(LogFile, value);
        }
    }
}
