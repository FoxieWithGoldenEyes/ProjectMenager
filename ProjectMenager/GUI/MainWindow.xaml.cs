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
using System.Windows.Navigation;
using System.Windows.Shapes;

using GUI.SubWindows;
using System.Windows.Xps.Serialization;

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IApp _api;

        public MainWindow()
        {
            InitializeComponent();

            _api = new AppDll.App();

            SizeChanged += MainWindow_SizeChanged;
            ContentRendered += MainWindow_ContentRendered;

            searchBox.GotFocus += SearchBox_GotFocus;
            searchBox.LostFocus += SearchBox_LostFocus;
        }


        private void MainWindow_ContentRendered(object? sender, EventArgs e)
        {
            RestoreListViewDefaultColumns();
        }
        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            RefreshFoldersListView();
        }


        private void RefreshFoldersListView()
        {
            DisplayFolders(_api.GetAllFoldersInfo());
            listView1TextBlock.Text = "All Folders";
            searchBox.Text = defaultSearchBoxMessage;
        }
        private void DisplayFolders(List<PmFolderModel> folderModels)
        {
            // Clear listView
            listView1.Items.Clear();

            // Fil listView with data
            foreach (var folder in folderModels)
            {
                listView1.Items.Add(folder);
            }

            RestoreListViewDefaultColumns();
        }

        private void SetColumnName(string oldColumnName, string newColumnName)
        {
            GridView gridView = (GridView)listView1.View;

            foreach (GridViewColumn column in gridView.Columns)
            {
                if (column.Header.ToString()?.ToLower() == oldColumnName.ToLower()) // Nazwa kolumny, którą chcesz zmienić
                {
                    column.Header = newColumnName;
                    break;
                }
            }
        }
        private void RestoreListViewDefaultColumns()
        {
            // Set default column names
            GridView gridView = (GridView)listView1.View;
            gridView.Columns.ElementAt(0).Header = "Id";
            gridView.Columns.ElementAt(1).Header = "Title";
            gridView.Columns.ElementAt(2).Header = "Description";
            gridView.Columns.ElementAt(3).Header = "Tags";

            // Set default column size

            // && Chceck if Id column content are calculated correctly
            if(listView1.Items.Count > 0/* && gridView.Columns.ElementAt(0).ActualWidth > 80*/)
            {
                // calculate margin 10px
                //double avariableWidth = listView1.ActualWidth - gridView.Columns.ElementAt(0).ActualWidth - 10;
                double avariableWidth = listView1.ActualWidth - 100 - 10;

                // Ustawienie kolumny Id
                //gridView.Columns.ElementAt(0).Width = double.NaN;
                gridView.Columns.ElementAt(0).Width = 100;

                if (avariableWidth > 0)
                {
                    // Ustawienie pozostałych kolumn
                    double width1 = gridView.Columns.ElementAt(1).Width = 0.25 * avariableWidth;
                    double width2 = gridView.Columns.ElementAt(2).Width = 0.5 * avariableWidth;
                    double width3 = gridView.Columns.ElementAt(3).Width = 0.25 * avariableWidth;
                }
            }
            //else
            //{
            //    // calculate margin 10px
            //    double avariableWidth = listView1.ActualWidth - 10;

            //    foreach (var column in gridView.Columns)
            //        column.Width = avariableWidth / gridView.Columns.Count;
            //}
        }



        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            var addFolderWindow = new GUI.SubWindows.AddOptionWindow(_api);
            bool? result = addFolderWindow.ShowDialog();

            //if (result == true)
            RefreshFoldersListView();
        }
        private void buttonRemove_Click(object sender, RoutedEventArgs e)
        {
            List<PmFolderModel> selectedPmFolders = listView1.SelectedItems.Cast<PmFolderModel>().ToList();
            _api.DeleteFolders(selectedPmFolders);

            RefreshFoldersListView();
        }
        private void buttonModify_Click(object sender, RoutedEventArgs e)
        {
            if(listView1.SelectedItem is PmFolderModel model)
            {
                ModifyFolderWindow window = new ModifyFolderWindow(_api, model);
                bool? dialogResult = window.ShowDialog();

                if (dialogResult is true)
                {
                    PmFolderModel updatingFolderModel = window.UpdatedFolder;
                    _api.UpdateFolder(updatingFolderModel);
                    RefreshFoldersListView();
                }
            }
        }
        private void buttonFind_Click(object sender, RoutedEventArgs e)
        {
            FindFoldersWindow findFolderWnd = new FindFoldersWindow(_api);
            bool? dialogResult = findFolderWnd.ShowDialog();

            if (dialogResult is true)
            {
                var foundFolders = findFolderWnd.FoundFolders;
                DisplayFolders(foundFolders);
                listView1TextBlock.Text = "Searching Results";
                searchBox.Text = "Searching Results";
            }

        }

        private void buttonReset_Click(object sender, RoutedEventArgs e)
        {
            RefreshFoldersListView();
        }
        private void buttonOpenInExplorer_Click(object sender, RoutedEventArgs e)
        {
            if (listView1.SelectedItem is PmFolderModel folder)
                _api.OpenInFileExplorer(folder.Path);
        }


        const string defaultSearchBoxMessage = "Type something...";
        const string defaultSearchResultMessagePrefix = "Searching Results For: ";
        string searchBoxMessageOnFocusLost = defaultSearchBoxMessage;
        private bool isSearchMode
        {
            get => !searchBoxMessageOnFocusLost.StartsWith(defaultSearchResultMessagePrefix);
            set => searchBox.Text = value is true ? defaultSearchResultMessagePrefix + searchBoxMessageOnFocusLost : defaultSearchBoxMessage;
        }
        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            searchBox.Text = string.Empty;
        }
        private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            searchBoxMessageOnFocusLost = searchBox.Text;
            isSearchMode = false;
        }
        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            if (isSearchMode is false) return;
            List<string> words = searchBoxMessageOnFocusLost.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();

            if(words.Count == 0)
            {
                RefreshFoldersListView();
                isSearchMode = false;
            }
            else
            {
                var searchResults = _api.SearchWords(words);
                DisplayFolders(searchResults);
                isSearchMode = true;
            }
        }
    }
}
