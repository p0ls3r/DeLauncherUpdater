using System;
using System.Windows.Forms;

namespace DeLauncher
{
    class Updater
    {
        public static int DefaultVersionNumber { get; } = -1;
        public static string LauncherFolder { get; } = @".LauncherFolder/";
        public static string VersionFileName { get; } = "version.xml";

        static void Main(string[] args)
        {
            try
            {
                if (!InstancesChecker.AlreadyRunning())
                {
                    if (ConnectivityChecker.CheckInternet() == ConnectivityChecker.ConnectionStatus.LimitedAccess || ConnectivityChecker.CheckInternet() == ConnectivityChecker.ConnectionStatus.NotConnected)
                    {
                        var connectionErrorMessage = "No connection to internet or repository - updates are not available! \rНет интернета или доступа к хранилищу - обновления недоступны!";
                        var connectionCaption = "No connection. Отсутствует соединение";
                        var buttonsError = MessageBoxButtons.OK;
                        var result = MessageBox.Show(connectionErrorMessage, connectionCaption, buttonsError);

                        if (result == DialogResult.OK)
                        {
                            System.Diagnostics.Process.Start("DeLauncherForm.exe");
                            Environment.Exit(0);
                        }
                    }
                    else
                    {
                        var currentVersion = VersionChecker.GetVersionNumber();
                        var latestVersion = UpdatesLoader.GetLatestVersionNumber().GetAwaiter().GetResult();

                        if (currentVersion < latestVersion && currentVersion != DefaultVersionNumber)
                        {
                            var message = "A new version of the launcher is available. Do you want to download it? \rДоступна новая версия лаунчера. Хотите её скачать?";
                            var caption = "Update is available. Доступно обновление";
                            var buttons = MessageBoxButtons.YesNoCancel;

                            var result = MessageBox.Show(message, caption, buttons);
                            if (result == System.Windows.Forms.DialogResult.Yes)
                                UpdatesLoader.DownloadUpdate().GetAwaiter().GetResult();

                            if (result == System.Windows.Forms.DialogResult.Cancel)
                                Environment.Exit(0);
                        }

                        if (currentVersion == -1)
                            UpdatesLoader.DownloadUpdate().GetAwaiter().GetResult();

                        System.Diagnostics.Process.Start("DeLauncherForm.exe");
                        Environment.Exit(0);
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }
    }
}
