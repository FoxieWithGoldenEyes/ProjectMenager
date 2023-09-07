using APIsDll.Public.APIs;
using APIsDll.Public.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GUI.SubWindows
{
    public partial class ModifyFolderWindow : Window
    {
        private readonly IApp _api;

        public PmFolderModel UpdatedFolder { get; private set; }

        public ModifyFolderWindow(IApp api, PmFolderModel modifyingModel)
        {
            InitializeComponent();

            _api = api;
            UpdatedFolder = modifyingModel;

            this.ContentRendered += ModifyFolderWindow_ContentRendered;
        }

        private void ModifyFolderWindow_ContentRendered(object? sender, EventArgs e)
        {
            textBoxName.Text = UpdatedFolder.Title;
            textBoxDescription.Text = UpdatedFolder.Description;
            textBoxTags.Text = string.Join(',', UpdatedFolder.Tags);

            textBoxName.Focus();
        }


        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            string title = textBoxName.Text;
            string description = textBoxDescription.Text;
            List<string> tags = textBoxTags.Text.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();

            // Don't do like this, becouse properties should be readonly or have defined setters and getters
            // Tera z kolei konstruktor powoduje nadpisanie id. meh
            //UpdatedFolder.Title = title;
            //UpdatedFolder.Description = description;
            //UpdatedFolder.Tags = tags;
            
            // We want to run the ctor
            var newDataFolderModel = new PmFolderModel(title, description, tags);
            UpdatedFolder.Title = newDataFolderModel.Title;
            UpdatedFolder.Path = newDataFolderModel.Path;
            UpdatedFolder.Description = newDataFolderModel.Description;
            UpdatedFolder.Tags = tags;

            this.DialogResult = true;
            this.Close();
        }
        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
