using APIsDll.Local.APIs;
using APIsDll.Public.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIsDll.Public.APIs
{
    public interface IApp
    {
        void OpenInFileExplorer(string folderPath);
        List<PmFolderModel> GetAllFoldersInfo();

        bool InitFolder(PmFolderModel folder);
        
        bool DeleteFolder(PmFolderModel folder);
        bool DeleteFolders(List<PmFolderModel> folders);
        
        bool UpdateFolder(PmFolderModel folder);

        List<PmFolderModel> SearchWords(List<string> words);
        List<PmFolderModel> Search(List<string> titleWords, List<string> descriptionWords, List<string> tagsWords);
    }
}
