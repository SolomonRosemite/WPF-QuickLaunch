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

        int index = -1;

        public CreateApp()
        {
            InitializeComponent();
            description.Content = "";
        }

        public CreateApp(QuickApp quickApp)
        {
            InitializeComponent();

            name = quickApp.name;
            Appname.Text = quickApp.name;

            description.Content = "";

            string paths = "";

            for (int i = 0; i < quickApp.paths.Count - 1; i++)
            {
                paths += quickApp.paths[i] + ";\n";
            }
            paths += quickApp.paths[quickApp.paths.Count - 1];
            pathTextbox.Text = paths;
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

                    paths = paths.Replace(@"""", "");

                    List<string> pathsList = new List<string>();
                    var array = (paths.Split(";"));
                    pathsList = array.OfType<string>().ToList();

                    QuickApp quickApp = new QuickApp() { name = name, paths = pathsList };
                    if (index != -1)
                    {
                        MainWindow.SaveJson(quickApp, index: index);
                        return;
                    }

                    MainWindow.SaveJson(quickApp);
                }
                else
                {
                    Popup popup = new Popup("Path can't be Empty");
                    popup.Show();
                }
            }
            else
            {
                Popup popup = new Popup("Name can't be Empty");
                popup.Show();
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
