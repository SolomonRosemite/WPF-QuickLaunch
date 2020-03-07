using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Linq;

namespace QuickLaunch
{
    /// <summary>
    /// Interaction logic for CreateApp.xaml
    /// </summary>
    public partial class CreateApp : Window
    {
        bool firstLaunch = true;
        bool nameisEmpty = false;
        bool addedAppsisEmpty = false;
        string paths;
        string name = "Unnamed";
        public CreateApp()
        {
            InitializeComponent();
            description.Content = "";
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                paths = MainWindow.getBetween(e.Source.ToString() + ":endofstring:", "TextBox: ", ":endofstring:");
                addedAppsisEmpty = false;
            }
            catch
            {
                addedAppsisEmpty = true;
                return;
            }
        }

        private void Save(object x, dynamic y)
        {
            if (!nameisEmpty)
            {
                if (!addedAppsisEmpty)
                {
                    // Save App
                    Console.WriteLine("App Saved");

                    List<string> pathsList = new List<string>();
                    var array = (paths.Split(";"));
                    pathsList = array.OfType<string>().ToList();

                    QuickApp quickApp = new QuickApp() { name = name, paths = pathsList };
                    MainWindow.SaveJson(quickApp);
                }
                else
                {
                    // added Apps is Empty
                    Console.WriteLine("Path Cant be Empty");
                }
            }
            else
            {
                // name cant be empty
                // "Play sound"
                Console.WriteLine("name cant be empty");
            }
        }

        private void Appname_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (firstLaunch)
            {
                firstLaunch = false;
            }
            else
            {
                try
                {
                    name = MainWindow.getBetween(e.Source.ToString() + ":", "TextBox: ", ":");
                    Savebutton.Content = "Save " + name;
                    nameisEmpty = false;
                }
                catch
                {
                    Savebutton.Content = "Save Unnamed";
                    nameisEmpty = true;
                }
            }
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
