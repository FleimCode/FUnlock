using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using System.Management;
using System.ServiceProcess;
using System.Globalization;
using System.Windows.Input;

namespace FUnlock
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            CheckAdminRights();
        }

        private void CheckAdminRights()
        {
            var identity = System.Security.Principal.WindowsIdentity.GetCurrent();
            var principal = new System.Security.Principal.WindowsPrincipal(identity);
            bool isAdmin = principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);

            if (!isAdmin)
            {
                txtStatus.Text = "⚠️ Для полной функциональности требуются права администратора";
            }
        }

        private void UpdateStatus(string message)
        {
            txtStatus.Text = message;
        }

        private async Task ShowProgress(string message)
        {
            UpdateStatus(message);
            progressBar.Visibility = Visibility.Visible;
            await Task.Delay(100);
        }

        private void HideProgress()
        {
            progressBar.Visibility = Visibility.Collapsed;
        }

        private async void UnlockTaskManager_Click(object sender, RoutedEventArgs e)
        {
            await ShowProgress("Разблокировка диспетчера задач...");
            try
            {
                RegistryHelper.SystemRestrictions.RemoveAllRestrictions();
                UpdateStatus("✅ Диспетчер задач разблокирован");
            }
            catch (Exception ex)
            {
                UpdateStatus($"❌ Ошибка: {ex.Message}");
            }
            HideProgress();
        }

        private async void UnlockRegistry_Click(object sender, RoutedEventArgs e)
        {
            await ShowProgress("Разблокировка редактора реестра...");
            try
            {
                RegistryHelper.SystemRestrictions.RemoveAllRestrictions();
                UpdateStatus("✅ Редактор реестра разблокирован");
            }
            catch (Exception ex)
            {
                UpdateStatus($"❌ Ошибка: {ex.Message}");
            }
            HideProgress();
        }

        private async void UnlockCMD_Click(object sender, RoutedEventArgs e)
        {
            await ShowProgress("Разблокировка командной строки...");
            try
            {
                RegistryHelper.SystemRestrictions.RemoveAllRestrictions();
                UpdateStatus("✅ Командная строка разблокирована");
            }
            catch (Exception ex)
            {
                UpdateStatus($"❌ Ошибка: {ex.Message}");
            }
            HideProgress();
        }

        private async void UnlockLock_Click(object sender, RoutedEventArgs e)
        {
            await ShowProgress("Разблокировка смены пользователя...");
            try
            {
                RegistryHelper.SystemRestrictions.RemoveAllRestrictions();
                UpdateStatus("✅ Смена пользователя разблокирована");
            }
            catch (Exception ex)
            {
                UpdateStatus($"❌ Ошибка: {ex.Message}");
            }
            HideProgress();
        }

        private async void EnableSecurity_Click(object sender, RoutedEventArgs e)
        {
            await ShowProgress("Включение средств безопасности Windows...");
            try
            {
                RegistryHelper.SecuritySettings.EnableWindowsSecurity();
                UpdateStatus("✅ Средства безопасности включены");
            }
            catch (Exception ex)
            {
                UpdateStatus($"❌ Ошибка: {ex.Message}");
            }
            HideProgress();
        }

        private async void FixDNS_Click(object sender, RoutedEventArgs e)
        {
            await ShowProgress("Исправление DNS...");
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = "/c ipconfig /flushdns && ipconfig /registerdns",
                    UseShellExecute = true,
                    Verb = "runas",
                    CreateNoWindow = true
                }).WaitForExit();

                UpdateStatus("✅ DNS-настройки исправлены");
            }
            catch (Exception ex)
            {
                UpdateStatus($"❌ Ошибка: {ex.Message}");
            }
            HideProgress();
        }

        private async void RestoreCursor_Click(object sender, RoutedEventArgs e)
        {
            await ShowProgress("Восстановление курсора...");
            try
            {
                RegistryHelper.CursorSettings.RestoreDefaultCursors();
                UpdateStatus("✅ Курсор восстановлен");
            }
            catch (Exception ex)
            {
                UpdateStatus($"❌ Ошибка: {ex.Message}");
            }
            HideProgress();
        }

        private async void FixLanguage_Click(object sender, RoutedEventArgs e)
        {
            await ShowProgress("Восстановление языковых настроек...");
            try
            {
                RegistryHelper.LanguageSettings.RestoreDefaultLanguages();
                UpdateStatus("✅ Языковые настройки восстановлены");
            }
            catch (Exception ex)
            {
                UpdateStatus($"❌ Ошибка: {ex.Message}");
            }
            HideProgress();
        }

        private async void RestoreDesktop_Click(object sender, RoutedEventArgs e)
        {
            await ShowProgress("Восстановление рабочего стола...");
            try
            {
                RegistryHelper.SystemRestrictions.RemoveAllRestrictions();
                UpdateStatus("✅ Рабочий стол восстановлен");
            }
            catch (Exception ex)
            {
                UpdateStatus($"❌ Ошибка: {ex.Message}");
            }
            HideProgress();
        }

        private async void FixStartMenu_Click(object sender, RoutedEventArgs e)
        {
            await ShowProgress("Исправление меню Пуск...");
            try
            {
                RegistryHelper.SystemRestrictions.RemoveAllRestrictions();
                UpdateStatus("✅ Меню Пуск исправлено");
            }
            catch (Exception ex)
            {
                UpdateStatus($"❌ Ошибка: {ex.Message}");
            }
            HideProgress();
        }

        private async void ResetDNS_Click(object sender, RoutedEventArgs e)
        {
            await ShowProgress("Сброс DNS кэша...");
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = "/c ipconfig /flushdns",
                    UseShellExecute = true,
                    Verb = "runas",
                    CreateNoWindow = true
                }).WaitForExit();

                UpdateStatus("✅ DNS кэш сброшен");
            }
            catch (Exception ex)
            {
                UpdateStatus($"❌ Ошибка: {ex.Message}");
            }
            HideProgress();
        }

        private async void FixNetworking_Click(object sender, RoutedEventArgs e)
        {
            await ShowProgress("Исправление сетевых настроек...");
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = "/c ipconfig /release && ipconfig /renew",
                    UseShellExecute = true,
                    Verb = "runas",
                    CreateNoWindow = true
                }).WaitForExit();

                UpdateStatus("✅ Сетевые настройки исправлены");
            }
            catch (Exception ex)
            {
                UpdateStatus($"❌ Ошибка: {ex.Message}");
            }
            HideProgress();
        }

        private async void CleanStartup_Click(object sender, RoutedEventArgs e)
        {
            await ShowProgress("Очистка автозапуска реестра...");
            try
            {
                RegistryHelper.StartupSettings.CleanStartupItems();
                UpdateStatus("✅ Автозапуск реестра очищен");
            }
            catch (Exception ex)
            {
                UpdateStatus($"❌ Ошибка: {ex.Message}");
            }
            HideProgress();
        }

        private async void ShowHiddenFiles_Click(object sender, RoutedEventArgs e)
        {
            await ShowProgress("Отображение скрытых файлов...");
            try
            {
                Registry.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Hidden", 1, RegistryValueKind.DWord);
                UpdateStatus("✅ Скрытые файлы отображены");
            }
            catch (Exception ex)
            {
                UpdateStatus($"❌ Ошибка: {ex.Message}");
            }
            HideProgress();
        }

        private async void RemoveAutorunInf_Click(object sender, RoutedEventArgs e)
        {
            await ShowProgress("Удаление файлов autorun.inf...");
            try
            {
                string[] drives = Directory.GetLogicalDrives();
                foreach (string drive in drives)
                {
                    string autorunPath = Path.Combine(drive, "autorun.inf");
                    if (File.Exists(autorunPath))
                    {
                        File.SetAttributes(autorunPath, FileAttributes.Normal);
                        File.Delete(autorunPath);
                    }
                }
                UpdateStatus("✅ Файлы autorun.inf удалены");
            }
            catch (Exception ex)
            {
                UpdateStatus($"❌ Ошибка: {ex.Message}");
            }
            HideProgress();
        }

        private async void RemoveRestrictions_Click(object sender, RoutedEventArgs e)
        {
            await ShowProgress("Удаление ограничений...");
            try
            {
                RegistryHelper.SystemRestrictions.RemoveAllRestrictions();
                UpdateStatus("✅ Все ограничения удалены");
            }
            catch (Exception ex)
            {
                UpdateStatus($"❌ Ошибка: {ex.Message}");
            }
            HideProgress();
        }

        private async void CleanTemp_Click(object sender, RoutedEventArgs e)
        {
            await ShowProgress("Очистка временных файлов...");
            try
            {
                string tempPath = Path.GetTempPath();
                DirectoryInfo di = new DirectoryInfo(tempPath);
                foreach (FileInfo file in di.GetFiles())
                {
                    try { file.Delete(); } catch { }
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    try { dir.Delete(true); } catch { }
                }
                UpdateStatus("✅ Временные файлы очищены");
            }
            catch (Exception ex)
            {
                UpdateStatus($"❌ Ошибка: {ex.Message}");
            }
            HideProgress();
        }

        private async void StopServices_Click(object sender, RoutedEventArgs e)
        {
            await ShowProgress("Остановка подозрительных служб...");
            try
            {
                string[] suspiciousServices = {
                    "malwareservice", "virusservice", "trojanservice"
                };

                foreach (string serviceName in suspiciousServices)
                {
                    try
                    {
                        ServiceController service = new ServiceController(serviceName);
                        if (service.Status == ServiceControllerStatus.Running)
                        {
                            service.Stop();
                            service.WaitForStatus(ServiceControllerStatus.Stopped);
                        }
                    }
                    catch { }
                }
                UpdateStatus("✅ Подозрительные службы остановлены");
            }
            catch (Exception ex)
            {
                UpdateStatus($"❌ Ошибка: {ex.Message}");
            }
            HideProgress();
        }

        private async void CleanHost_Click(object sender, RoutedEventArgs e)
        {
            await ShowProgress("Очистка файла hosts...");
            try
            {
                string hostsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "drivers\\etc\\hosts");
                string defaultContent = "127.0.0.1 localhost";
                File.WriteAllText(hostsPath, defaultContent);
                UpdateStatus("✅ Файл hosts очищен");
            }
            catch (Exception ex)
            {
                UpdateStatus($"❌ Ошибка: {ex.Message}");
            }
            HideProgress();
        }

        private async void KillProcesses_Click(object sender, RoutedEventArgs e)
        {
            await ShowProgress("Завершение подозрительных процессов...");
            try
            {
                string[] patterns = { "malware", "virus" };
                int killedCount = await SystemTools.ProcessManager.KillProcessesByPattern(patterns);
                UpdateStatus($"✅ Завершено процессов: {killedCount}");
            }
            catch (Exception ex)
            {
                UpdateStatus($"❌ Ошибка: {ex.Message}");
            }
            HideProgress();
        }

        private async void KillSpecificProcess_Click(object sender, RoutedEventArgs e)
        {
            string processName = txtProcessName.Text.Trim();
            if (string.IsNullOrEmpty(processName) || processName == "Введите имя процесса")
            {
                UpdateStatus("❌ Введите имя процесса для завершения");
                return;
            }

            await ShowProgress($"Завершение процесса: {processName}...");
            try
            {
                int killedCount = SystemTools.ProcessManager.KillProcessByName(processName);
                UpdateStatus($"✅ Завершено процессов: {killedCount}");
            }
            catch (Exception ex)
            {
                UpdateStatus($"❌ Ошибка: {ex.Message}");
            }
            HideProgress();
        }

        private void txtProcessName_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtProcessName.Text == "Введите имя процесса")
            {
                txtProcessName.Text = "";
            }
        }

        private void txtProcessName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtProcessName.Text))
            {
                txtProcessName.Text = "Введите имя процесса";
            }
        }

        private async void ScanSFC_Click(object sender, RoutedEventArgs e)
        {
            await ShowProgress("Запуск сканирования SFC...");
            try
            {
                bool success = SystemTools.SystemIntegrity.RunSFCScan();
                if (success)
                {
                    UpdateStatus("✅ Сканирование SFC запущено");
                }
                else
                {
                    UpdateStatus("❌ Не удалось запустить SFC сканирование");
                }
            }
            catch (Exception ex)
            {
                UpdateStatus($"❌ Ошибка: {ex.Message}");
            }
            HideProgress();
        }

        private async void UpdateSystem_Click(object sender, RoutedEventArgs e)
        {
            await ShowProgress("Проверка обновлений...");
            try
            {
                bool success = SystemTools.SystemIntegrity.RunDISMRepair();
                if (success)
                {
                    UpdateStatus("✅ Запущена проверка обновлений через DISM");
                }
                else
                {
                    UpdateStatus("❌ Не удалось запустить DISM");
                }
            }
            catch (Exception ex)
            {
                UpdateStatus($"❌ Ошибка: {ex.Message}");
            }
            HideProgress();
        }

        private async void CreateRestorePoint_Click(object sender, RoutedEventArgs e)
        {
            await ShowProgress("Создание точки восстановления...");
            try
            {
                bool success = SystemTools.SystemIntegrity.CreateSystemRestorePoint();
                if (success)
                {
                    UpdateStatus("✅ Точка восстановления создана");
                }
                else
                {
                    UpdateStatus("❌ Не удалось создать точку восстановления");
                }
            }
            catch (Exception ex)
            {
                UpdateStatus($"❌ Ошибка: {ex.Message}");
            }
            HideProgress();
        }

        private async void FlushDNS_Click(object sender, RoutedEventArgs e)
        {
            await ShowProgress("Сброс DNS...");
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = "/c ipconfig /flushdns",
                    UseShellExecute = true,
                    Verb = "runas",
                    CreateNoWindow = true
                }).WaitForExit();
                UpdateStatus("✅ DNS сброшен");
            }
            catch (Exception ex)
            {
                UpdateStatus($"❌ Ошибка: {ex.Message}");
            }
            HideProgress();
        }

        private async void ResetWinsock_Click(object sender, RoutedEventArgs e)
        {
            await ShowProgress("Сброс Winsock...");
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = "/c netsh winsock reset",
                    UseShellExecute = true,
                    Verb = "runas",
                    CreateNoWindow = true
                }).WaitForExit();
                UpdateStatus("✅ Winsock сброшен (требуется перезагрузка)");
            }
            catch (Exception ex)
            {
                UpdateStatus($"❌ Ошибка: {ex.Message}");
            }
            HideProgress();
        }

        private async void FixBoot_Click(object sender, RoutedEventArgs e)
        {
            await ShowProgress("Исправление загрузки...");
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = "/k bootrec /fixmbr && bootrec /fixboot && bootrec /rebuildbcd",
                    UseShellExecute = true,
                    Verb = "runas"
                });

                UpdateStatus("✅ Команды исправления загрузки запущены");
            }
            catch (Exception ex)
            {
                UpdateStatus($"❌ Ошибка: {ex.Message}");
            }
            HideProgress();
        }
    }
}