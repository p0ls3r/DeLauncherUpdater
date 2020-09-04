using System;
using System.Diagnostics;

namespace DeLauncher
{
    class InstancesChecker
    {
        public static bool AlreadyRunning()
        {
            Process[] processes = Process.GetProcesses();
            Process currentProc = Process.GetCurrentProcess();            
            foreach (Process process in processes)
            {
                if (currentProc.ProcessName == process.ProcessName && currentProc.Id != process.Id)
                {
                    throw new ApplicationException("Another instance of this process is already running: " + process.Id);
                }
            }
            return false;
        }
    }
}
