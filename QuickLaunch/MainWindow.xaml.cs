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

namespace QuickLaunch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<QuickApp> quickApps = new List<QuickApp>();
        List<ListViewItem> listViewItemsList = new List<ListViewItem>();

        public MainWindow()
        {
            InitializeComponent();
            loadApps();
        }

        void loadApps()
        {
            // Load Json data
            // List<QuickApp> quickApps = new List<QuickApp>();
            // quickApps = Loaded Json.

            // Load Data to App
            // for (int i = 0; i < quickApps.Count; i++)
            // {
            //     ListViewItem listViewItems = new ListViewItem();

            //     listViewItems.MouseDoubleClick += RunApp;
            //     listViewItemsList.Add(listViewItems);
            // }

            for (int i = 0; i < 2; i++)
            {
                ListViewItem listViewItems = new ListViewItem();
                switch (i)
                {
                    case 0:
                        listViewItems.Content = "TMCode";
                        break;

                    case 1:
                        listViewItems.Content = "Overwatch";
                        break;
                }
                listViewItems.MouseDoubleClick += RunApp;
                listViewItemsList.Add(listViewItems);
            }

            listView.ItemsSource = listViewItemsList;
            string json = JsonConvert.SerializeObject(quickApps);
        }

        private void RunApp(object sender, MouseButtonEventArgs e)
        {
            ListViewItem app = (ListViewItem)sender;
            // Console.WriteLine("hiiii");
            Console.Out.Write("done");
            console.Text = app.Content.ToString();
        }
        private void AddApp(object sender, RoutedEventArgs e)
        {
            ListViewItem app = (ListViewItem)sender;

        }
        private void EditApp(object sender, RoutedEventArgs e)
        {
            ListViewItem app = (ListViewItem)sender;

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
        public string[] paths { get; set; }
    }
}
