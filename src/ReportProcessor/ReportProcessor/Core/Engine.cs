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

                int empty = Directory.GetFiles(fullPath).Length;

                if (empty == 0)
                {
                    Console.WriteLine(string.Format(InfoMessages.CheckedFolder, folders[i]));
                    Console.WriteLine(InfoMessages.EndLine);
                    continue;
                }

                CheckFolder(folders[i], fullPath);

                Console.WriteLine(InfoMessages.ProcessedFolder);
                Console.WriteLine(InfoMessages.EndLine);
            }
        }

        private void CheckFolder(string reportDir, string fullFolderPath)
        {
            var fileArray = Directory.EnumerateFiles(fullFolderPath, "*.*")
                .Where(fn => fn.ToLower().EndsWith(GlobalConstants.ExcelFileExtension) ||
                             fn.ToLower().EndsWith(GlobalConstants.CSVFileExtension))
                .ToArray();

            Console.WriteLine($"Folder {reportDir} is being checked.");
            Console.WriteLine($"File count: {fileArray.Length}");
            Console.WriteLine("Processing...");

            for (int i = 0; i < fileArray.Length; i++)
            {
                var projectDir = GetProjectDirectory();
                var provider = RetrieveHeadersFromProvider(reportDir, projectDir + @"Datasets/");
                var fullFilePath = fileArray[i];
                var currentFileName = Path.GetFileName(fileArray[i]);

                Console.WriteLine($"Processing {currentFileName}...");

                try
                {
                    var csvList = Reader.ProcessData(provider, fullFilePath);

                    DataTable csvData = csvList.ToDataTable();
                    Console.WriteLine("Rows count:" + csvData.Rows.Count);

                    //bool isInserted = SqlService.IsDataInsertedDB(csvData);

                    //if (!isInserted)
                    //{
                    //    Console.WriteLine($"File: {currentFileName} moved to Error");
                    //    File.Move(fullFilePath, string.Format(GlobalConstants.FolderOnError + currentFileName, reportDir));
                    //    File.Delete(fullFilePath);
                    //    return;
                    //}

                    //if (!File.Exists(GlobalConstants.FolderOnSuccess + currentFileName))
                    //{
                    //    Console.WriteLine($"File: {currentFileName} moved to Historic");
                    //    File.Move(fullFilePath, string.Format(GlobalConstants.FolderOnSuccess + currentFileName, reportDir));
                    //    File.Delete(fullFilePath);
                    //}
                    //else
                    //{
                    //    File.Move(fullFilePath, string.Format(GlobalConstants.FolderOnError + currentFileName, reportDir), true);
                    //    File.Delete(fullFilePath);
                    //}
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
            => DataProcessor
            .Deserializer
            .ImportHeaders(File.ReadAllText(baseDir + $"headers-{reportDir}.xml"));

        private static string GetProjectDirectory()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var directoryName = Path.GetFileName(currentDirectory);
            var relativePath = directoryName.StartsWith("netcoreapp") ? @"../../../" : string.Empty;

            return relativePath;
        }

        // Error logger file
        //TextWriter log = new StreamWriter(@"Logs\log.txt");

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