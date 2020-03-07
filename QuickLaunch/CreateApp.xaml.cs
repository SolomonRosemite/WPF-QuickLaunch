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
        public CreateApp()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string paths;
            try
            {
                paths = MainWindow.getBetween(e.Source.ToString() + ":endofstring:", "TextBox:", ":endofstring:");
                addedAppsisEmpty = false;
            }
            catch
            {
                addedAppsisEmpty = true;
                return;
            }

            Console.WriteLine(paths);
        }

        private void Save(object x, dynamic y)
        {
            if (!nameisEmpty)
            {
                if (!addedAppsisEmpty)
                {
                    // Save App
                    Console.WriteLine("App Saved");
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
                Console.WriteLine("name cant be empty");
                // "Play sound"
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
                    Savebutton.Content = "Save " + MainWindow.getBetween(e.Source.ToString() + ":", "TextBox:", ":");
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
