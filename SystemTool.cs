using System;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace FUnlock
{
    public static class SystemTools
    {
        public static class ProcessManager
        {
            public static async Task<int> KillProcessesByPattern(string[] patterns)
            {
                int killedCount = 0;

                foreach (Process process in Process.GetProcesses())
                {
                    try
                    {
                        string processName = process.ProcessName.ToLower();
                        string processPath = "";

                        try
                        {
                            processPath = process.MainModule?.FileName?.ToLower() ?? "";
                        }
                        catch { }

                        foreach (string pattern in patterns)
                        {
                            if (processName.Contains(pattern.ToLower()) || processPath.Contains(pattern.ToLower()))
                            {
                                process.Kill();
                                killedCount++;
                                await Task.Delay(100);
                                break;
                            }
                        }
                    }
                    catch { }
                }

                return killedCount;
            }

            public static int KillProcessByName(string name)
            {
                int killedCount = 0;
                foreach (Process process in Process.GetProcessesByName(name))
                {
                    try
                    {
                        process.Kill();
                        killedCount++;
                    }
                    catch { }
                }
                return killedCount;
            }
        }

        public static class ServicesManager
        {
            public static void StopService(string serviceName)
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
        }

        public static class SystemIntegrity
        {
            public static bool RunSFCScan()
            {
                try
                {
                    var process = Process.Start(new ProcessStartInfo
                    {
                        FileName = "sfc.exe",
                        Arguments = "/scannow",
                        UseShellExecute = true,
                        Verb = "runas",
                        CreateNoWindow = false
                    });

                    return process != null;
                }
                catch
                {
                    return false;
                }
            }

            public static bool RunDISMRepair()
            {
                try
                {
                    var process = Process.Start(new ProcessStartInfo
                    {
                        FileName = "DISM.exe",
                        Arguments = "/Online /Cleanup-Image /RestoreHealth",
                        UseShellExecute = true,
                        Verb = "runas",
                        CreateNoWindow = false
                    });

                    return process != null;
                }
                catch
                {
                    return false;
                }
            }

            public static bool CreateSystemRestorePoint(string description = "FUnlock Recovery Point")
            {
                try
                {
                    var process = Process.Start(new ProcessStartInfo
                    {
                        FileName = "powershell.exe",
                        Arguments = $"-Command \"Checkpoint-Computer -Description '{description}' -RestorePointType 'MODIFY_SETTINGS'\"",
                        CreateNoWindow = true,
                        UseShellExecute = false,
                        Verb = "runas"
                    });

                    process?.WaitForExit(30000);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}