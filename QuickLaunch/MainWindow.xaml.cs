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

namespace QuickLaunch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // private string TMPath = GetDirectory() + @"\TMRosemite";
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
            ListViewItem title = new ListViewItem();
            title.Content = "Added Apps";
            title.HorizontalContentAlignment = HorizontalAlignment.Center;
            listViewItemsList.Add(title);

            SetDirectory();

            string jsonFromFile;
            using (var reader = new StreamReader(QuickLaunchPath + @"\SavedApps.json"))
            {
                jsonFromFile = reader.ReadToEnd();
            }

            quickApps = JsonConvert.DeserializeObject<List<QuickApp>>(jsonFromFile);
            // foreach (QuickApp item in quickApps)
            // {
            //     Console.WriteLine(item.name);
            //     foreach (var items in item.paths)
            //     {
            //         Console.WriteLine(items);
            //     }
            //     Console.WriteLine("--");
            // }

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

        // UI Functions
        private void RunApp(object sender, MouseButtonEventArgs e)
        {
            ListViewItem app = (ListViewItem)sender;
            Console.WriteLine("done");
        }
        private void AddApp(object sender, RoutedEventArgs e)
        {
            // Json
            List<string> paths = new List<string>() { "C/", "F/" };
            QuickApp quick = new QuickApp() { name = "Test", paths = paths };

            quickApps.Add(quick);

            string json = JsonConvert.SerializeObject(quickApps, Formatting.Indented);
            File.WriteAllText(QuickLaunchPath + @"\SavedApps.json", json);

            // Update ListView
            ListViewItem listViewItems = new ListViewItem();

            listViewItems.Content = "Test";

            listViewItems.MouseDoubleClick += RunApp;
            listViewItemsList.Add(listViewItems);
            listView.ItemsSource = listViewItemsList;
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
