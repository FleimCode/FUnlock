using Microsoft.Win32;
using System;
using System.Collections.Generic;

namespace FUnlock
{
    public static class RegistryHelper
    {
        public static class SystemRestrictions
        {
            private const string POLICIES_SYSTEM = @"Software\Microsoft\Windows\CurrentVersion\Policies\System";
            private const string POLICIES_EXPLORER = @"Software\Microsoft\Windows\CurrentVersion\Policies\Explorer";

            public static void RemoveAllRestrictions()
            {
                try
                {
                    using (RegistryKey key = Registry.CurrentUser.OpenSubKey(POLICIES_SYSTEM, true))
                    {
                        if (key != null)
                        {
                            string[] restrictionValues = {
                                "DisableTaskMgr", "DisableRegistryTools", "DisableCMD",
                                "DisableChangePassword", "DisableLockWorkstation"
                            };

                            foreach (string value in restrictionValues)
                            {
                                try { key.DeleteValue(value, false); } catch { }
                            }
                        }
                    }

                    using (RegistryKey key = Registry.CurrentUser.OpenSubKey(POLICIES_EXPLORER, true))
                    {
                        if (key != null)
                        {
                            string[] explorerRestrictions = {
                                "NoControlPanel", "NoFind", "NoRun", "NoDesktop",
                                "NoStartMenuMyGames", "NoStartMenuMorePrograms",
                                "NoStartMenuMyMusic", "NoStartMenuNetworkPlaces"
                            };

                            foreach (string value in explorerRestrictions)
                            {
                                try { key.DeleteValue(value, false); } catch { }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Ошибка при удалении ограничений: {ex.Message}");
                }
            }
        }

        public static class LanguageSettings
        {
            public static void RestoreDefaultLanguages()
            {
                try
                {
                    using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\International", true))
                    {
                        if (key != null)
                        {
                            var settings = new Dictionary<string, string>
                            {
                                {"LocaleName", "ru-RU"},
                                {"sCountry", "Россия"},
                                {"sLanguage", "RUS"},
                                {"iCountry", "7"},
                                {"sShortDate", "dd.MM.yyyy"},
                                {"sLongDate", "d MMMM yyyy 'г.'"},
                                {"sTimeFormat", "HH:mm:ss"},
                                {"sShortTime", "HH:mm"},
                                {"sCurrency", "₽"},
                                {"sThousand", " "},
                                {"sDecimal", ","},
                                {"sList", ";"}
                            };

                            foreach (var setting in settings)
                            {
                                key.SetValue(setting.Key, setting.Value);
                            }
                        }
                    }

                    using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Keyboard Layout\Preload"))
                    {
                        string[] valueNames = key.GetValueNames();
                        foreach (string valueName in valueNames)
                        {
                            key.DeleteValue(valueName);
                        }

                        key.SetValue("1", "00000419");
                        key.SetValue("2", "00000409");
                    }

                    using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Keyboard Layout\Toggle"))
                    {
                        key.SetValue("Hotkey", "1");
                        key.SetValue("Language Hotkey", "1");
                        key.SetValue("Layout Hotkey", "3");
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Ошибка при восстановлении языковых настроек: {ex.Message}");
                }
            }

            public static void RemoveUnwantedLanguages()
            {
                try
                {
                    string[] unwantedLanguages = {
                        "00000401",
                        "00000404",
                        "00000804",
                        "0000041E",
                        "0000042A",
                        "0000041D",
                        "00000415",
                        "0000041F",
                        "00000429",
                        "0000040D"
                    };

                    using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Keyboard Layout\Preload", true))
                    {
                        if (key != null)
                        {
                            string[] valueNames = key.GetValueNames();
                            foreach (string valueName in valueNames)
                            {
                                string value = key.GetValue(valueName)?.ToString();
                                if (value != null)
                                {
                                    foreach (string unwanted in unwantedLanguages)
                                    {
                                        if (value.Equals(unwanted, StringComparison.OrdinalIgnoreCase))
                                        {
                                            key.DeleteValue(valueName);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Ошибка при удалении нежелательных языков: {ex.Message}");
                }
            }
        }

        public static class SecuritySettings
        {
            public static void EnableWindowsSecurity()
            {
                try
                {
                    using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Windows Defender", true))
                    {
                        key?.DeleteValue("DisableAntiSpyware", false);
                        key?.DeleteValue("DisableRealtimeMonitoring", false);
                    }
                    using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", true))
                    {
                        if (key != null)
                        {
                            key.SetValue("EnableLUA", 1);
                            key.SetValue("ConsentPromptBehaviorAdmin", 5);
                            key.SetValue("ConsentPromptBehaviorUser", 3);
                            key.SetValue("EnableInstallerDetection", 1);
                            key.SetValue("PromptOnSecureDesktop", 1);
                        }
                    }
                    using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", true))
                    {
                        key?.DeleteValue("NoAutoUpdate", false);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Ошибка при включении средств безопасности: {ex.Message}");
                }
            }
        }

        public static class CursorSettings
        {
            public static void RestoreDefaultCursors()
            {
                try
                {
                    using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Cursors", true))
                    {
                        if (key != null)
                        {
                            var cursorTypes = new Dictionary<string, string> {
                                {"", ""},
                                {"AppStarting", ""},
                                {"Arrow", ""},
                                {"Crosshair", ""},
                                {"Hand", ""},
                                {"Help", ""},
                                {"IBeam", ""},
                                {"No", ""},
                                {"NWPen", ""},
                                {"SizeAll", ""},
                                {"SizeNESW", ""},
                                {"SizeNS", ""},
                                {"SizeNWSE", ""},
                                {"SizeWE", ""},
                                {"UpArrow", ""},
                                {"Wait", ""}
                            };

                            foreach (var cursor in cursorTypes)
                            {
                                key.SetValue(cursor.Key, cursor.Value);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Ошибка при восстановлении курсора: {ex.Message}");
                }
            }
        }

        public static class StartupSettings
        {
            private const string WIN_NT_CURRENT_VERSION = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\";

            public static void CleanAppInitDlls()
            {
                try
                {
                    using (RegistryKey key = Registry.LocalMachine.OpenSubKey(WIN_NT_CURRENT_VERSION + @"Windows", true))
                    {
                        if (key != null)
                        {
                            key.SetValue("AppInit_DLLs", "");
                        }
                    }

                    using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows NT\CurrentVersion\Windows", true))
                    {
                        if (key != null)
                        {
                            key.SetValue("AppInit_DLLs", "");
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Ошибка при очистке AppInit_DLLs: {ex.Message}");
                }
            }

            public static void RestoreWinlogonSettings()
            {
                try
                {
                    using (RegistryKey key = Registry.LocalMachine.OpenSubKey(WIN_NT_CURRENT_VERSION + @"Winlogon", true))
                    {
                        if (key != null)
                        {
                            key.SetValue("Userinit", @"C:\Windows\system32\userinit.exe,");
                            key.SetValue("Shell", "explorer.exe");
                        }
                    }

                    using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows NT\CurrentVersion\Winlogon", true))
                    {
                        if (key != null)
                        {
                            key.SetValue("Userinit", @"C:\Windows\system32\userinit.exe,");
                            key.SetValue("Shell", "explorer.exe");
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Ошибка при восстановлении Winlogon: {ex.Message}");
                }
            }

            public static void CleanStartupItems()
            {
                try
                {
                    CleanAppInitDlls();
                    RestoreWinlogonSettings();
                }
                catch (Exception ex)
                {
                    throw new Exception($"Ошибка при очистке автозапуска: {ex.Message}");
                }
            }
        }

        public static class AutoRunSettings
        {
            public static void CleanSuspiciousAutorunEntries()
            {
                string[] suspiciousNames = {
                    "malware", "virus", "trojan", "adware", "spyware", "keylogger"
                };

                string[] startupPaths = {
                    @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run",
                    @"SOFTWARE\Microsoft\Windows\CurrentVersion\RunOnce",
                    @"SOFTWARE\Microsoft\Windows\CurrentVersion\RunServices",
                    @"SOFTWARE\Microsoft\Windows\CurrentVersion\RunServicesOnce",
                };

                foreach (string path in startupPaths)
                {
                    using (RegistryKey key = Registry.CurrentUser.OpenSubKey(path, true))
                    {
                        if (key != null)
                        {
                            CleanSuspiciousEntries(key, suspiciousNames);
                        }
                    }
                    using (RegistryKey key = Registry.LocalMachine.OpenSubKey(path, true))
                    {
                        if (key != null)
                        {
                            CleanSuspiciousEntries(key, suspiciousNames);
                        }
                    }
                }
            }

            private static void CleanSuspiciousEntries(RegistryKey key, string[] suspiciousNames)
            {
                string[] valueNames = key.GetValueNames();
                foreach (string valueName in valueNames)
                {
                    string value = key.GetValue(valueName)?.ToString()?.ToLower();
                    if (value != null)
                    {
                        foreach (string suspicious in suspiciousNames)
                        {
                            if (valueName.ToLower().Contains(suspicious) || value.Contains(suspicious))
                            {
                                try
                                {
                                    key.DeleteValue(valueName);
                                    break;
                                }
                                catch { }
                            }
                        }
                    }
                }
            }
        }
    }
}