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
using System.Windows.Navigation;
using System.Windows.Shapes;
using DLCLibrary;
using System.Text.RegularExpressions;


namespace WpfApplication2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private NavigationWindow _mainWindow;
        DE_IFMO_Checker checker;
        public MainWindow()
        {
            InitializeComponent();
            Run();
           
        }

        public void Run()
        {
         
        }

        public void OnMouseDown(object sender, EventArgs e)
        {
            this.DragMove();
        }

        public void OnCloseClick(object sender, EventArgs e)
        {
            this.Close();
        }

        public void OpenCmdExecuted(object sender, EventArgs e)
        {

        }
    }
}
