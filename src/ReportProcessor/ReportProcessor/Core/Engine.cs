namespace ReportProcessor.Core
{
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Security.Permissions;

    using NLog;
    using ReportProcessor.Common;
    using ReportProcessor.Core.Contracts;
    using ReportProcessor.DataProcessor;

    public class Engine : IEngine
    {
        [PermissionSet(SecurityAction.Demand, Name = GlobalConstants.PermissionSetFullTrust)]
        public void Run(Logger logger)
        {
            var folders = GlobalConstants.ReportFolders;

            for (int i = 0; i < folders.Length; i++)
            {
                bool isLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

                string linuxFolderToWatch = isLinux ?
                    GlobalConstants.LinuxFolderToWatch :
                    GlobalConstants.WindowsFolderToWatch;

                string fullPath = string.Format(linuxFolderToWatch, folders[i]);
                logger.Info(string.Format(InfoMessages.LineHeader, folders[i]));
                int empty = Directory.GetFiles(fullPath).Length;

                if (empty == 0)
                {
                    logger.Info(string.Format(InfoMessages.CheckedFolder, folders[i]));
                    continue;
                }

                FolderHandler.ProcessFolder(folders[i], fullPath, logger, isLinux);

                logger.Info(string.Format(InfoMessages.CheckedFolder, folders[i]));
            }
        }
    }
}