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
        private static string QuickLaunchPath = GetDirectory() + @"\TMRosemite\QuickLaunch";
        public static List<QuickApp> quickApps = new List<QuickApp>();
        private static List<ListViewItem> listViewItemsList = new List<ListViewItem>();
        private ListViewItem title = new ListViewItem();

        public MainWindow()
        {
            InitializeComponent();
            loadApps();
        }

        void loadApps()
        {
            // Clear
            listViewItemsList.Clear();
            quickApps.Clear();

            // Setting up Title
            title.Content = "Added Apps";
            title.HorizontalContentAlignment = HorizontalAlignment.Center;
            title.IsSelected = true;
            listViewItemsList.Add(title);

            SetDirectory();

            // Read Json
            string jsonFromFile;
            using (var reader = new StreamReader(QuickLaunchPath + @"\SavedApps.json"))
                jsonFromFile = reader.ReadToEnd();

            quickApps = JsonConvert.DeserializeObject<List<QuickApp>>(jsonFromFile);

            // Sort List
            quickApps = quickApps.OrderBy(o => o.name).ToList();

            // Load Data to App
            for (int i = 0; i < quickApps.Count; i++)
            {
                ListViewItem listViewItems = new ListViewItem();

                listViewItems.Content = quickApps[i].name;

                listViewItems.MouseDoubleClick += RunApp;
                listViewItems.PreviewKeyDown += RunApp;
                listViewItemsList.Add(listViewItems);
            }

            listView.ItemsSource = listViewItemsList;
        }

        public static void SaveJson(QuickApp quickApp, int index = -1)
        {
            if (index != -1)
            {
                quickApps.RemoveAt(index);
            }

            quickApps.Add(quickApp);

            string json = JsonConvert.SerializeObject(quickApps, Formatting.Indented);
            File.WriteAllText(QuickLaunchPath + @"\SavedApps.json", json);
        }

        private void RunApp(object sender, object e)
        {
            if (e is KeyEventArgs && ((KeyEventArgs)e).Key != Key.Enter)
                return;

            ListViewItem app = (ListViewItem)sender;

            for (int i = 0; i < quickApps.Count; i++)
            {
                if (quickApps[i].name == app.Content.ToString())
                {
                    RunFile(quickApps[i].paths, quickApps[i].tasks);
                    break;
                }
            }
        }

        void RunFile(List<string> filePath, List<string> taskkill)
        {
            bool error = false;
            foreach (string item in filePath)
            {
                try
                {
                    Process.Start(item);
                }
                catch
                {
                    Popup popup = new Popup("Item: " + item + " not found.");
                    popup.Show();

                    error = true;
                }
            }

            // Task kill
            EndTask(taskkill);

            if (!error)
            {
                Application.Current.Shutdown();
            }
        }

        void EndTask(List<string> tasks)
        {
            if (tasks == null)
            {
                return;
            }

            foreach (string item in tasks)
            {
                if (item.Length == 0)
                {
                    continue;
                }

                try
                {
                    Process[] procs = Process.GetProcessesByName(item);
                    foreach (Process p in procs) { p.Kill(); }
                }
                catch
                {
                    Popup popup = new Popup("Item: " + item + " not found.");
                    popup.Show();
                }
            }
        }
        private void AddApp(object sender, RoutedEventArgs e)
        {
            CreateApp createApp = new CreateApp();
            createApp.Show();
        }
        private void EditApp(object sender, RoutedEventArgs e)
        {
            try
            {
                string item = listView.SelectedItem.ToString();
                item = getBetween(item + ":end:", "ListViewItem: ", ":end:");
                for (byte i = 0; i < quickApps.Count; i++)
                {
                    if (quickApps[i].name == item)
                    {
                        CreateApp createApp = new CreateApp(quickApps[i], i);
                        createApp.Show();
                        break;
                    }
                }
            }
            catch
            {
                Popup popup = new Popup("Select the App you want to edit.");
                popup.Show();
            }
        }
        public static void DeleteJsonEntry(byte index)
        {
            quickApps.RemoveAt(index);

            string json = JsonConvert.SerializeObject(quickApps, Formatting.Indented);
            File.WriteAllText(QuickLaunchPath + @"\SavedApps.json", json);
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
        private static void SetDirectory()
        {
            if (!Directory.Exists(QuickLaunchPath))
            {
                Directory.CreateDirectory(QuickLaunchPath);
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
                return Directory.GetParent(path).ToString();
            }
            else
            {
                return "";
            }
        }

        private void OnLoaded(object sender, EventArgs e)
        {
            this.listView.Focus();
        }

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            title.IsSelected = false;
        }
    }

    public class QuickApp
    {
        public string name { get; set; }
        public List<string> paths { get; set; }
        public List<string> tasks { get; set; }
    }
}
