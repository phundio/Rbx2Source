using System;
using System.Collections.Generic;
using Microsoft.Win32;

namespace Rbx2Source.Resources
{
    static class Settings
    {
        private static Dictionary<string,object> cache;
        private static RegistryKey rbx2Source;
        private static bool unsaved = false;

        public static bool UnsavedChanges
        {
            get { return unsaved; }
        }

        public static object GetSetting(string key)
        {
            if (!cache.ContainsKey(key))
                cache[key] = rbx2Source.GetValue(key);

            return cache[key];
        }

        public static T GetSetting<T>(string key)
        {
            object value = GetSetting(key);
            if (value != null && value is T)
                return (T)value;
            else
                return default(T);
        }

        public static void SetSetting(string key, object value, bool save = false)
        {
            cache[key] = value;
            unsaved = true;
            if (save) Save();
        }

        public static void Save()
        {
            foreach (string key in cache.Keys)
                rbx2Source.SetValue(key, cache[key]);

            unsaved = false;
        }

        public static void RestoreSavedChanges()
        {
            foreach (string key in rbx2Source.GetValueNames())
                SetSetting(key, rbx2Source.GetValue(key));

            unsaved = false;
        }

        public static void Initialize()
        {
            SetSetting("Username", "CloneTrooper1019");
            SetSetting("AssetId", 19027209);
            SetSetting("CompilerType", "Avatar");
        }

        public static void RestoreDefaults()
        {
            SetSetting("PrecacheAssets", true);
            SetSetting("AssembleMultithread", false);
            SetSetting("GenPrimitives", false);
            SetSetting("ArchiveModels", true);
            SetSetting("DarkTheme", false);
            SetSetting("UnitScale", 10.00);
            SetSetting("ModelCompilerParams", "-nop4 -verbose");
            SetSetting("TextureCompilerParams", "-format ABGR8888");
            Save();
        }

        private static RegistryKey Open(RegistryKey current, string target)
        {
            return current.CreateSubKey(target, RegistryKeyPermissionCheck.ReadWriteSubTree);
        }

        static Settings()
        {
            RegistryKey currentUser = Registry.CurrentUser;
            RegistryKey software = Open(currentUser, "SOFTWARE");
            rbx2Source = Open(software, "Rbx2Source");
            cache = new Dictionary<string, object>();

            RestoreSavedChanges();

            if (GetSetting("Initialized") == null)
            {
                Initialize();
                SetSetting("Initialized", true, true);
            }

            if (GetSetting("SetupDefaultSettings") == null)
            {
                RestoreDefaults();
                SetSetting("SetupDefaultSettings", true, true);
            }
        }
    }
}