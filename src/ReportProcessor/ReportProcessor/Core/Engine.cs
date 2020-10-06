namespace ReportProcessor.Core
{
    using System;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Security.Permissions;

    using ReportProcessor.Common;
    using ReportProcessor.Core.Contracts;
    using ReportProcessor.Data.Models;
    using ReportProcessor.Data.Services;
    using ReportProcessor.DataProcessor;
    using ToDataTable;

    public class Engine : IEngine
    {
        [PermissionSet(SecurityAction.Demand, Name = GlobalConstants.PermissionSetFullTrust)]
        public void Run()
        {
            var folders = GlobalConstants.ReportFolders;

            for (int i = 0; i < folders.Length; i++)
            {
                string fullPath = string.Format(GlobalConstants.FolderToWatch, folders[i]);

                // Error logger file
                var date = DateTime.Now.ToString(GlobalConstants.RequiredSqlDateTimeFormat);
                TextWriter log = new StreamWriter(string.Format(GlobalConstants.FilePathLog, date));

                Console.WriteLine(string.Format(InfoMessages.LineHeader, folders[i]));
                int empty = Directory.GetFiles(fullPath).Length;

                if (empty == 0)
                {
                    Console.WriteLine(string.Format(InfoMessages.CheckedFolder, folders[i]));
                    continue;
                }

                CheckFolder(folders[i], fullPath);

                Console.WriteLine(string.Format(InfoMessages.CheckedFolder, folders[i]));
            }
        }

        private void CheckFolder(string reportDir, string fullFolderPath)
        {
            var fileArray = Directory.EnumerateFiles(fullFolderPath, "*.*")
                .Where(fn => fn.ToLower().EndsWith(GlobalConstants.ExcelFileExtension) ||
                             fn.ToLower().EndsWith(GlobalConstants.CSVFileExtension))
                .ToArray();

            Console.WriteLine(string.Format(InfoMessages.CheckingFolder, reportDir));
            Console.WriteLine(string.Format(InfoMessages.FileCountInFolder, fileArray.Length));
            Console.WriteLine(InfoMessages.Processing);

            for (int i = 0; i < fileArray.Length; i++)
            {
                var projectDir = GetProjectDirectory();
                var provider = RetrieveHeadersFromProvider(reportDir, projectDir + GlobalConstants.FolderHeaders);
                var fullFilePath = fileArray[i];
                var currentFileName = Path.GetFileName(fileArray[i]);

                Console.WriteLine(string.Format(InfoMessages.ProcessingFile, currentFileName));
                Console.WriteLine(string.Format(InfoMessages.LineHeader, reportDir));

                try
                {
                    var csvList = Reader.ProcessData(provider, fullFilePath);

                    if (csvList == null || csvList.Count == 0)
                    {
                        Console.WriteLine(string.Format(InfoMessages.FileMoveError, currentFileName));
                        File.Move(fullFilePath, string.Format(GlobalConstants.FolderOnError + currentFileName, reportDir));
                        Console.WriteLine(string.Format(InfoMessages.LineHeader, reportDir));
                        break;
                    }

                    DataTable csvData = csvList.ToDataTable();
                    Console.WriteLine(InfoMessages.RowsCountFile + csvData.Rows.Count);

                    bool isInserted = SqlService.UploadData(csvData);

                    if (!isInserted)
                    {
                        Console.WriteLine(string.Format(InfoMessages.FileMoveError, currentFileName));
                        File.Move(fullFilePath, string.Format(GlobalConstants.FolderOnError + currentFileName, reportDir));
                        File.Delete(fullFilePath);
                        break;
                    }

                    if (!File.Exists(GlobalConstants.FolderOnSuccess + currentFileName))
                    {
                        Console.WriteLine(string.Format(InfoMessages.FileMoveSuccess, currentFileName));
                        File.Move(fullFilePath, string.Format(GlobalConstants.FolderOnSuccess + currentFileName, reportDir));
                        File.Delete(fullFilePath);
                    }
                }
                catch (IOException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private static Provider RetrieveHeadersFromProvider(string reportDir, string baseDir)
            => Deserializer.ImportHeaders(File.ReadAllText(baseDir + string.Format(GlobalConstants.HeadersFileName, reportDir)));

        private static string GetProjectDirectory()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var directoryName = Path.GetFileName(currentDirectory);
            var relativePath = directoryName.StartsWith(GlobalConstants.NetCoreApp) ? @"../../../" : string.Empty;

            return relativePath;
        }

        //Log($"Folder {folder} is checked.");

        //private void Log()
        //{
        //    Console.WriteLine();
        //    log.WriteLine();
        //}

        //Write text
        //private void Log(string strText)
        //{
        //    Console.WriteLine(strText);
        //    log.WriteLine(strText);
        //}
    }
}