namespace ReportProcessor.Core
{
    using System.IO;
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
                // Linux server ubuntu path
                //string fullPath = Directory.GetDirectoryRoot("home") + string.Format(GlobalConstants.FolderToWatch, folders[i]);

                // Windows path
                string fullPath = string.Format(GlobalConstants.FolderToWatch, folders[i]);
                logger.Info(string.Format(InfoMessages.LineHeader, folders[i]));
                int empty = Directory.GetFiles(fullPath).Length;

                if (empty == 0)
                {
                    logger.Info(string.Format(InfoMessages.CheckedFolder, folders[i]));
                    continue;
                }

                FolderHandler.ProcessFolder(folders[i], fullPath, logger);

                logger.Info(string.Format(InfoMessages.CheckedFolder, folders[i]));
            }
        }
    }
}