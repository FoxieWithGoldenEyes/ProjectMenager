using APIsDll.Public.APIs;
using APIsDll.Public.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GUI.SubWindows
{
    public partial class FindFoldersWindow : Window
    {
        private readonly IApp _api;
        public List<PmFolderModel> FoundFolders { get; private set; } = new List<PmFolderModel>();
        
        public FindFoldersWindow(IApp api)
        {
            this.InitializeComponent();

            _api = api;
        }
        private void ModifyFolderWindow_ContentRendered(object? sender, EventArgs e)
        {
            textBoxName.Focus();
        }

        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            List<string> searchingTitleWords = textBoxName.Text.Split('0', StringSplitOptions.RemoveEmptyEntries).ToList();
            List<string> searchingDescriptionWords = textBoxDescription.Text.Split('0', StringSplitOptions.RemoveEmptyEntries).ToList();
            List<string> searchingTags = textBoxTags.Text.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();

            if(searchingTitleWords.Count == 0 &&  searchingDescriptionWords.Count == 0 && searchingTags.Count == 0)
            {
                DialogResult = false;
                return;
            }

            this.FoundFolders = _api.Search(searchingTitleWords, searchingDescriptionWords, searchingTags);

            this.DialogResult = true;
            this.Close();
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
