using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
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
using Newtonsoft.Json;
using System.IO;
using System.Diagnostics;

namespace QuickLaunch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string QuickLaunchPath = GetDirectory() + @"\TMRosemite\QuickLaunch";
        public List<QuickApp> quickApps = new List<QuickApp>();
        List<ListViewItem> listViewItemsList = new List<ListViewItem>();

        public MainWindow()
        {
            InitializeComponent();
            loadApps();
        }

        void loadApps()
        {
            // Setting up Title
            ListViewItem title = new ListViewItem();
            title.Content = "Added Apps";
            title.HorizontalContentAlignment = HorizontalAlignment.Center;
            listViewItemsList.Add(title);

            SetDirectory();

            // Read Json
            string jsonFromFile;
            using (var reader = new StreamReader(QuickLaunchPath + @"\SavedApps.json"))
            {
                jsonFromFile = reader.ReadToEnd();
            }

            quickApps = JsonConvert.DeserializeObject<List<QuickApp>>(jsonFromFile);

            // Load Data to App
            for (int i = 0; i < quickApps.Count; i++)
            {
                ListViewItem listViewItems = new ListViewItem();

                listViewItems.Content = quickApps[i].name;

                listViewItems.MouseDoubleClick += RunApp;
                listViewItemsList.Add(listViewItems);
            }

            listView.ItemsSource = listViewItemsList;
        }


        private void SetDirectory()
        {
            if (!Directory.Exists(QuickLaunchPath))
            {
                Console.WriteLine("Path is Set");
                Directory.CreateDirectory(QuickLaunchPath);
            }
            else
            {
                Console.WriteLine("Path is okay");
            }

            if (!File.Exists(QuickLaunchPath + @"\SavedApps.json"))
            {
                File.Create(QuickLaunchPath + @"\SavedApps.json");
            }
        }

        private static string GetDirectory()
        {
            string path = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName;
            if (Environment.OSVersion.Version.Major >= 6)
            {
                return path = Directory.GetParent(path).ToString();
            }
            else
            {
                return "";
            }
        }

        private void RunApp(object sender, MouseButtonEventArgs e)
        {
            ListViewItem app = (ListViewItem)sender;

            for (int i = 0; i < quickApps.Count; i++)
            {
                if (quickApps[i].name == app.Content.ToString())
                {
                    RunFile(quickApps[i].paths);
                    break;
                }
            }
        }
        void RunFile(List<string> filePath)
        {
            foreach (var item in filePath)
            {
                try
                {
                    Process.Start(item);
                }
                catch
                {
                    Console.WriteLine("Item " + item + " not found.");
                }
            }
        }
        private void AddApp(object sender, RoutedEventArgs e)
        {
            CreateApp createApp = new CreateApp();
            createApp.Show();
            // string name = "Test";
            // // Add new App to Json
            // List<string> paths = new List<string>() { "C/", "F/" };
            // QuickApp quick = new QuickApp() { name = name, paths = paths };

            // quickApps.Add(quick);

            // string json = JsonConvert.SerializeObject(quickApps, Formatting.Indented);
            // File.WriteAllText(QuickLaunchPath + @"\SavedApps.json", json);

            // // Update ListView dosen't work yet
            // ListViewItem listViewItems = new ListViewItem();

            // listViewItems.Content = name;

            // listViewItems.MouseDoubleClick += RunApp;

            // listViewItemsList.Add(listViewItems);
            // listView.ItemsSource = listViewItemsList;
        }
        private void EditApp(object sender, RoutedEventArgs e)
        {

        }

        public static string getBetween(string strSource, string strStart, string strEnd)
        {
            int Start, End;
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }
            else
            {
                return "";
            }
        }
    }

    public class QuickApp
    {
        public string name { get; set; }
        public List<string> paths { get; set; }
    }
}
