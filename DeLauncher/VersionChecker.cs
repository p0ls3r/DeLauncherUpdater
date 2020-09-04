using System.IO;
using System.Xml;

namespace DeLauncher
{
    static class VersionChecker
    {        
        public static int GetVersionNumber()
        {
            var path = Updater.LauncherFolder;
            if (!Directory.Exists(path))
                CreateLauncherFolder();

            if (!File.Exists(path + Updater.VersionFileName))
                CreateVersionFile();

            return XmlVersionReader.GetVersionFromXml(path + Updater.VersionFileName);
        }

        private static void CreateVersionFile()
        {
            var path = Updater.LauncherFolder;
            var defaultNumber = Updater.DefaultVersionNumber.ToString();

            var xDoc = new XmlDocument();
            var verElem = xDoc.CreateElement("version");
            var numberElem = xDoc.CreateElement("number");

            XmlText ageText = xDoc.CreateTextNode(defaultNumber);

            numberElem.AppendChild(ageText);
            verElem.AppendChild(numberElem);
            xDoc.AppendChild(verElem);

            xDoc.Save(path + Updater.VersionFileName);
        }
        private static void CreateLauncherFolder()
        {
            var path = Updater.LauncherFolder;

            var folder = Directory.CreateDirectory(path);
            folder.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
        }
    }
}
