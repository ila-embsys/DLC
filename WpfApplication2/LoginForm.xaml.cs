using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


using System.ComponentModel;
using System.IO;

using DLCLibrary;

namespace WpfApplication2
{
    /// <summary>
    /// Логика взаимодействия для Workspace.xaml
    /// </summary>
    public partial class Workspace : Page
    {
        private MainWindow _mainWindow;
        private BackgroundWorker loginWorker;
        private DE_IFMO_Checker checker;
        string password, login;
        private bool success = false;
        public Workspace()
        {
            InitializeComponent();
            loginWorker = new BackgroundWorker();
            loginWorker.WorkerSupportsCancellation = true;
            loginWorker.WorkerReportsProgress = true;
            loginWorker.DoWork += DoLoginWork;
            loginWorker.ProgressChanged += LoginWorkerProgressChanged;
            loginWorker.RunWorkerCompleted += LoginWorkerComplited;
        }


        private void OnLoginButtonClick(object sender, EventArgs e)
        {
            login = Login.Text;
            password = Password.Password;
            if (login.Trim() != String.Empty && password.Trim() != String.Empty)
                loginWorker.RunWorkerAsync();
        }

        private void OnCancelButtonClick(object sender, EventArgs e)
        {
            Login.Text = Password.Password = String.Empty;
        }


        private void LoginPage_Loaded(object sender, RoutedEventArgs e)
        {
            _mainWindow = (MainWindow)Window.GetWindow(this);
            if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments), DE_IFMO_Checker.USER_Filename)))
            {
                Dictionary<string, string> data = (new User()).GetUserData();
                Login.Text = data["LOGIN"];
                Password.Password = data["PASSWD"];
                Submit.Click -= OnLoginButtonClick;
                Cancel.Click -= OnCancelButtonClick;
                loginWorker.RunWorkerAsync();
            }
        }

        private void OnCloseButtonClick(object sender, EventArgs e)
        {
            _mainWindow.Close();
        }

        private void DoLoginWork(object sender, DoWorkEventArgs e)
        {
            loginWorker.ReportProgress(0);
            loginWorker.ReportProgress(35);
            checker = new DE_IFMO_Checker(login, password);
            System.Threading.Thread.Sleep(1500);
            success = checker.GetStatus();
            loginWorker.ReportProgress(100);
        }

        private void LoginWorkerProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Progress.IsIndeterminate = true;
        }

        private void LoginWorkerComplited(object sender, RunWorkerCompletedEventArgs e)
        {
            Progress.IsIndeterminate = false;
            if (success)
                _mainWindow.LoginForm.Navigate(new WorkspacePage(checker));
        }
    }
}
