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
    /// <summary>
    /// Interaction logic for AddOptionWindow.xaml
    /// </summary>
    public partial class AddOptionWindow : Window
    {
        private readonly IApp _api;

        public AddOptionWindow(IApp api)
        {
            InitializeComponent();
            _api = api;
        }



        private void button1OK_Click(object sender, RoutedEventArgs e)
        {

            string name = textBoxName.Text;
            string description = textBoxDescription.Text;
            List<string> tags = textBoxTags.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            PmFolderModel model = new PmFolderModel(name, description, tags);

            _api.InitFolder(model);
            // Manual assign dialogResult if ok button was pressed
            DialogResult = true;
            this.Close();
        }

        private void button2Cancel_Click(object sender, RoutedEventArgs e)
        {
            // This Is automaticy false
            //DialogResult = false;
            this.Close();
        }
    }
}
