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
        string tasks = "";
        string name = "Unnamed";

        int index = -1;

        public CreateApp()
        {
            InitializeComponent();
            Deletebutton.IsEnabled = false;

            load();
        }

        public CreateApp(QuickApp quickApp, byte index)
        {
            InitializeComponent();
            Deletebutton.IsEnabled = true;

            load();

            if (quickApp.paths.Count == 0) { return; }

            this.index = index;
            name = quickApp.name;
            Appname.Text = quickApp.name;

            // paths
            string paths = "";

            for (int i = 0; i < quickApp.paths.Count - 1; i++)
            {
                paths += quickApp.paths[i] + ";\n";
            }

            byte lastIndexpath = Convert.ToByte(quickApp.paths.Count - 1);

            paths += quickApp.paths[lastIndexpath];
            pathTextbox.Text = paths;

            try { if (quickApp.tasks.Count == 0) { return; } } catch { return; }

            // tasks
            string tasks = "";

            for (int i = 0; i < quickApp.tasks.Count - 1; i++)
            {
                tasks += quickApp.tasks[i] + ";\n";
            }

            byte lastIndex = Convert.ToByte(quickApp.tasks.Count - 1);

            tasks += quickApp.tasks[lastIndex];
            tasksTextbox.Text = tasks;
        }

        void load()
        {
            descriptionTaskKill.Content = "- Write the Process without .exe \n- Add a task by Writing the\n ( Process ) not the Program.\n- Split each path with a \";\".\n- Example: WhatsApp; Discord\n  Don't do WhatsApp.exe.";
            description.Content = "- Write or Copy paste the path \n  of the file.\n- Split each path with a \";\".\n- Check out the shown example\n  and delete the example\n  befor adding.";
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
            if (nameisEmpty)
            {
                Popup popup = new Popup("Name can't be Empty");
                popup.Show();
            }
            else if (addedAppsisEmpty)
            {
                Popup popup = new Popup("Path can't be Empty");
                popup.Show();
            }
            else if (CheckList(MainWindow.quickApps, name) && index == -1)
            {
                Popup popup = new Popup("Appname already exists");
                popup.Show();
            }
            else
            {
                // Save App
                string Context;

                paths = paths.Replace(@"""", "");

                List<string> pathsList = new List<string>();

                var arraypath = paths.Split(";");
                pathsList = arraypath.OfType<string>().ToList();

                List<string> tasksList = new List<string>();

                var arraytask = tasks.Split(";");
                tasksList = arraytask.OfType<string>().ToList();

                if (tasksList[0].Contains("Example: "))
                {
                    tasksList = null;
                }

                QuickApp quickApp = new QuickApp() { name = name, paths = pathsList, tasks = tasksList };

                // Replace 
                if (index != -1)
                {
                    MainWindow.SaveJson(quickApp, index: index);
                    Context = "App Edited";
                }
                else
                {
                    MainWindow.SaveJson(quickApp);
                    Context = "App Saved";
                }
                Popup popup = new Popup(Context);
                Close();
                CloseMainWindowNow();
                OpenMainWindowNow();
                popup.Show();
            }
        }

        public void CloseMainWindowNow()
        {
            var mainWindow = (Application.Current.MainWindow as MainWindow);
            if (mainWindow != null)
            {
                mainWindow.Close();
            }
        }

        public void OpenMainWindowNow()
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }

        private bool CheckList(List<QuickApp> quickApps, string name)
        {
            for (int i = 0; i < quickApps.Count; i++)
            {
                if (quickApps[i].name.ToLower() == name.ToLower())
                {
                    return true;
                }
            }
            return false;
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

        private void Delete(object sender, RoutedEventArgs e)
        {
            MainWindow.DeleteJsonEntry((byte)index);
            CloseMainWindowNow();
            OpenMainWindowNow();
            Close();
        }

        private void tasksTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tasks = MainWindow.getBetween(e.Source.ToString() + ":endofstring:", "TextBox: ", ":endofstring:");
            }
            catch
            {
                tasks = "";
                return;
            }
        }
    }
}
