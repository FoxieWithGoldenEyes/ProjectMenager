using APIsDll.Local.APIs;
using APIsDll.Public.APIs;
using APIsDll.Public.Models;
using IODll;
using LoggersDll.Loggers;
using OSDll;
using System.Reflection;

namespace AppDll
{
    public class App : IApp
    {
        public static string AppFolder = Environment.CurrentDirectory;
        public static string DataFolder = Path.Combine(AppFolder, "Data");
        public static string LogsFolder = Path.Combine(AppFolder, "Logs");

        public App()
        {
            _logger = new FileLogger(LogsFolder);
            _osFuncProvider = new WindowsFunctionsProvider();
            _pmFoldersData = new AppFile<PmFolderModel>(Path.Combine(DataFolder, "data.json"), AppFile<PmFolderModel>.FileType.JsonData);
        }



        ILogger _logger;
        IOsFunctionsProvider _osFuncProvider;
        AppFile<PmFolderModel> _pmFoldersData;



        public void OpenInFileExplorer(string folderPath)
        {
            _osFuncProvider.RunExplorer(folderPath);
        }
        
        public List<PmFolderModel> GetAllFoldersInfo()
        {
            return _pmFoldersData.ReadAllEntries();
        }


        public bool InitFolder(PmFolderModel folder)
        {
            // Creating pm folder
            if(Directory.Exists(folder.Path))
            {
                _logger.LogMessage($"File {folder.Path} already exist!");
                return false;
            }
            else
            {
                Directory.CreateDirectory(folder.Path);
                _logger.LogMessage($"Successfully Created Directory: {folder.Path}");
            }

            // Add entry
            _pmFoldersData.Add(folder);
            
            _logger.LogMessage($"Succesfully initialized pm folder: {folder.Title}");
            return true;
        }

        public bool DeleteFolder(PmFolderModel folder)
        {
            // 1. check - deleting entry from datafile
            if (!_pmFoldersData.Remove(folder))
            {
                _logger.LogMessage($"There is no specyfic entry {folder}");
                return false;
            }

            // 2. Check - if no problems with deleting folder
            try
            {
                Directory.Delete(folder.Path);
            }
            catch (Exception ex)
            {
                _logger.LogMessage($"Cannot delete folder {folder}");
                _logger.LogMessage("Error: " + ex.Message);
                return false;
            }

            // All ok, return true
            _logger.LogMessage($"Successfully removed folder: {folder}");
            return true;
        }
        public bool DeleteFolders(List<PmFolderModel> list)
        {
            foreach (var folder in list)
                if (DeleteFolder(folder) is false) return false;
            return true;
        }

        public bool UpdateFolder(PmFolderModel folder)
        {
            PmFolderModel oldFolderModel;

            try
            {
                oldFolderModel = _pmFoldersData.First(x => x.Id == folder.Id);
            }
            catch (Exception ex)
            {
                _logger.LogMessage($"There is no entry with specyfic ID: {folder.Id}");
                return false;
            }


            if (oldFolderModel.Path == folder.Path)
                return true;
            else
            {
                try
                {
                    Directory.Move(oldFolderModel.Path, folder.Path);
                    _logger.LogMessage($"Successfully moved directory: {oldFolderModel.Path} to: {folder.Path}");

                    _pmFoldersData.Remove(oldFolderModel);
                    _pmFoldersData.Add(folder);
                    _logger.LogMessage($"Succesfully updated {oldFolderModel} to {folder}");

                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogMessage($"Cannot moved directory: {oldFolderModel.Path} to: {folder.Path}");
                    _logger.LogMessage(ex.Message);
                    return false;
                }
            }
        }

        public List<PmFolderModel> SearchWords(List<string> words)
        {
            var folderList = _pmFoldersData.ReadAllEntries();
            List<PmFolderModel> resultList = new List<PmFolderModel>();

            foreach (var folder in folderList)
            {
                foreach (var word in words)
                {
                    if (folder.Title.Contains(word) && !resultList.Contains(folder))
                        resultList.Add(folder);
                    if (folder.Description.Contains(word) && !resultList.Contains(folder))
                        resultList.Add(folder);
                    if (folder.Tags.Contains(word) && !resultList.Contains(folder))
                        resultList.Add(folder);

                }
            }

            return resultList;
        }
        public List<PmFolderModel> Search(List<string> titleWords, List<string> descriptionWords, List<string> tagsWords)
        {
            var folderList = _pmFoldersData.ReadAllEntries();
            List<PmFolderModel> resultList = new List<PmFolderModel>();

            foreach (var folder in folderList)
            {
                foreach (var word in titleWords)
                    if(folder.Title.Contains(word) && !resultList.Contains(folder))
                        resultList.Add(folder);

                foreach (var word in descriptionWords)
                    if (folder.Description.Contains(word) && !resultList.Contains(folder))
                        resultList.Add(folder);

                foreach (var word in tagsWords)
                    if (folder.Tags.Contains(word) && !resultList.Contains(folder))
                        resultList.Add(folder);
            }

            return resultList;
        }
    }
}